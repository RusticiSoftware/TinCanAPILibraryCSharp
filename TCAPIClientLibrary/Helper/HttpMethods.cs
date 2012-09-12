using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using RusticiSoftware.TinCanAPILibrary.Exceptions;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace RusticiSoftware.TinCanAPILibrary.Helper
{
    public class HttpMethods
    {
        #region Write Request Async State
        private class WriteRequestState
        {
            WebRequest request;
            Stream dataStream;
            ITCAPICallback callback;
            String postData;

            /// <summary>
            /// The original post request
            /// </summary>
            public String PostData
            {
                get { return postData; }
                set { postData = value; }
            }

            /// <summary>
            /// TCAPI Reference
            /// </summary>
            public ITCAPICallback Callback
            {
                get { return callback; }
                set { callback = value; }
            }

            /// <summary>
            /// The datastream for closing
            /// </summary>
            public Stream DataStream
            {
                get { return dataStream; }
                set { dataStream = value; }
            }

            /// <summary>
            /// The original request made
            /// </summary>
            public WebRequest Request
            {
                get { return request; }
                set { request = value; }
            }
        }
        #endregion

        #region Constants
        public const int REATTEMPT_COUNT = 1;
        #endregion

        /// <summary>
        /// Sends a POST request
        /// </summary>
        /// <param name="postData">A string of data to post</param>
        /// <param name="authentification">An IAuthentificationConfiguration.  Only Basic is currently supported.</param>
        /// <param name="endpoint">The endpoint to send the statement to</param>
        /// <returns>A response string</returns>
        public static string PostRequest(string postData, string endpoint, IAuthenticationConfiguration authentification)
        {
            string result = null;
            for (int attempt = 0; attempt <= REATTEMPT_COUNT; attempt++)
            {
                char[] postDataCharArray = postData.ToCharArray();
                WebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = Encoding.ASCII.GetByteCount(postDataCharArray);
                // Needs OAuth support which will certainly change this line
                request.Headers["Authorization"] = authentification.GetAuthorization();

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(Encoding.ASCII.GetBytes(postDataCharArray), 0, Encoding.ASCII.GetByteCount(postDataCharArray));
                dataStream.Close();
                try
                {
                    WebResponse response = request.GetResponse();
                    Stream returnStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(returnStream);
                    result = reader.ReadToEnd();
                    response.Close();
                }
                catch (WebException e)
                {
                    try
                    {
                        ThrowHttpException(e);
                    }
                    catch (InternalServerErrorException)
                    {
                        if (attempt > REATTEMPT_COUNT)
                            ThrowHttpException(e);
                        else
                            continue; // Reattempt on an internal server error
                    }
                }
                break; // upon success, break
            }
            return result;
        }

        /// <summary>
        /// Issues a post request asynchronously
        /// </summary>
        /// <param name="postData">A string of data to post</param>
        /// <param name="authentification">An IAuthentificationConfiguration.  Only Basic is currently supported.</param>
        /// <param name="endpoint">The endpoint to send the statement to</param>
        /// <param name="callback">An ITCAPI object to handle callbacks</param>
        public static void PostRequestAsync(string postData, string endpoint, IAuthenticationConfiguration authentification, ITCAPICallback callback)
        {
            char[] postDataCharArray = postData.ToCharArray();
            //WebClient webClient = new WebClient();
            WriteRequestState state = new WriteRequestState();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.AllowWriteStreamBuffering = true;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = Encoding.ASCII.GetByteCount(postDataCharArray);
            // Needs OAuth support which will certainly change this line
            request.Headers["Authorization"] = authentification.GetAuthorization();

            Stream dataStream = request.GetRequestStream();

            state.Request = request;
            state.Callback = callback;
            state.DataStream = dataStream;
            state.PostData = postData;

            dataStream.BeginWrite(Encoding.ASCII.GetBytes(postDataCharArray), 0, Encoding.ASCII.GetByteCount(postDataCharArray), BeginRequest, state);
        }

        private static void BeginRequest(IAsyncResult result)
        {
            WriteRequestState state = (WriteRequestState)result.AsyncState;
            try
            {
                WebRequest request = state.Request;
                state.DataStream.Close();

                WebResponse response = request.GetResponse(); Stream returnStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(returnStream);
                string responseString = reader.ReadToEnd();
                Statement[] statements = null;
                try
                {
                    TinCanJsonConverter converter = new TinCanJsonConverter();
                    statements = (Statement[])converter.DeserializeJSON(state.PostData, typeof(Statement[]));
                    state.Callback.StatementsStored(statements);
                }
                catch (Exception e)  // If it fails to convert back to a statement the TCAPICallback doesn't provide default behavior.
                {
                    state.Callback.PostFailException(e);
                }
                //state.Callback.StatementsStored(statements);
            }
            catch (Exception e)
            {
                Statement[] statements = null;
                try
                {
                    TinCanJsonConverter converter = new TinCanJsonConverter();
                    statements = (Statement[])converter.DeserializeJSON(state.PostData, typeof(Statement[]));
                }
                catch (Exception ex) // Again, if it can't make a statement
                {
                    state.Callback.PostFailException(ex);
                }
                state.Callback.StatementsFailed(statements, e);
            }
        }

        /// <summary>
        /// Sends a PUT request
        /// </summary>
        /// <param name="putData">The data to PUT to the server</param>
        /// <param name="queryParameters">A name-value pair collection of query parameters</param>
        /// <returns>The response string</returns>
        public static string PutRequest(string putData, NameValueCollection queryParameters, string endpoint, IAuthenticationConfiguration authentification)
        {
            string result = null;
            using (WebClient webClient = new WebClient())
            {
                if (queryParameters != null)
                    endpoint += ToQueryString(queryParameters);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.ContentLength = Encoding.ASCII.GetByteCount(putData.ToCharArray());
                // Needs OAuth support which will certainly change this line
                request.Headers["Authorization"] = authentification.GetAuthorization();

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(Encoding.ASCII.GetBytes(putData.ToCharArray()), 0, Encoding.ASCII.GetByteCount(putData.ToCharArray()));
                dataStream.Close();
                WebResponse response;
                try
                {
                    response = request.GetResponse();
                    Stream returnStream = request.GetResponse().GetResponseStream();
                    StreamReader reader = new StreamReader(returnStream);
                    result = reader.ReadToEnd();
                    response.Close();
                }
                catch (WebException e)
                {
                    ThrowHttpException(e);
                }
            }
            return result;
        }
        /// <summary>
        /// Sends a GET request
        /// </summary>
        /// <param name="getData">A name-value pair collection of query parameters</param>
        /// <returns>The response string</returns>
        public static string GetRequest(NameValueCollection getData, string endpoint, IAuthenticationConfiguration authentification)
        {
            string result = null;
            if (getData != null)
                endpoint += ToQueryString(getData);
            WebRequest request = WebRequest.Create(endpoint);
            request.Method = "GET";
            // OAuth support needs to be added here
            request.Headers["Authorization"] = authentification.GetAuthorization();

            try
            {
                WebResponse response = request.GetResponse();
                Stream returnStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(returnStream);
                result = reader.ReadToEnd();
                response.Close();
            }
            catch (WebException e)
            {
                ThrowHttpException(e);
            }

            return result;
        }

        /// <summary>
        /// Sends a DELETE request
        /// </summary>
        /// <param name="deleteData">A name-value pair collection of query parameters</param>
        /// <returns>The response string</returns>
        public static string DeleteRequest(NameValueCollection deleteData, string endpoint, IAuthenticationConfiguration authentification)
        {
            string result = null;
            string end = endpoint + ToQueryString(deleteData);
            WebRequest request = WebRequest.Create(endpoint + ToQueryString(deleteData));
            request.Method = "DELETE";
            // OAuth support needs to be added here
            request.Headers["Authorization"] = authentification.GetAuthorization();

            try
            {
                WebResponse response = request.GetResponse();
                Stream returnStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(returnStream);
                result = reader.ReadToEnd();
                response.Close();
            }
            catch (WebException e)
            {
                ThrowHttpException(e);
            }

            return result;
        }

        private static string ToQueryString(NameValueCollection nvc)
        {
            StringBuilder queryString = new StringBuilder("?");
            foreach (string key in nvc)
            {
                queryString.Append(HttpUtility.UrlEncode(key));
                queryString.Append("=");
                queryString.Append(HttpUtility.UrlEncode(nvc[key]));
                queryString.Append("&");
            }
            return queryString.ToString().Substring(0, queryString.Length - 1);
        }

        /// <summary>
        /// Handles interpreting WebClient exceptions
        /// </summary>
        /// <param name="e">The original exception thrown</param>
        public static void ThrowHttpException(WebException e)
        {
            using (WebResponse r = e.Response)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)r;
                Stream responseStream = r.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string response = reader.ReadToEnd();
                switch (((HttpWebResponse)httpResponse).StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new InvalidArgumentException("Error 400 " + response);
                    case HttpStatusCode.Unauthorized:
                        throw new UnauthorizedException("Error 401" + response);
                    case HttpStatusCode.NotFound:
                        throw new NotFoundException("Error 404" + response);
                    case HttpStatusCode.Conflict:
                        throw new ConflictException("Error 409" + response);
                    case HttpStatusCode.PreconditionFailed:
                        throw new PreconditionFailedException("Error 412" + response);
                    case HttpStatusCode.InternalServerError:
                        throw new InternalServerErrorException("An unknown error occured (500)." + response);
                }
            }
        }

        private class InternalServerErrorException : Exception
        {
            public InternalServerErrorException() : base() { }
            public InternalServerErrorException(string message) : base(message) { }
        }
    }
}
