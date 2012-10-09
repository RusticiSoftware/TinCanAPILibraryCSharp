using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using RusticiSoftware.TinCanAPILibrary.Exceptions;
using RusticiSoftware.TinCanAPILibrary.Model;
using RusticiSoftware.TinCanAPILibrary;
using System.Threading;

namespace RusticiSoftware.TinCanAPILibrary.Helper
{
    public class HttpMethods
    {

        #region Write Request Async State
        /*
        private class WriteRequestState
        {
            WebRequest request;
            Stream dataStream;
            ITCAPICallback tcapiCallback;
            String postData;
            TCAPI.AsyncPostCallback asyncPostCallback;

            /// <summary>
            /// Handle to AsyncPostCallback
            /// </summary>
            public TCAPI.AsyncPostCallback AsyncPostCallback
            {
                get { return asyncPostCallback; }
                set { asyncPostCallback = value; }
            }

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
                get { return tcapiCallback; }
                set { tcapiCallback = value; }
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
        */
        #endregion

        #region Constants
        public const int REATTEMPT_COUNT = 1;
        // public const int ASYNC_TIMEOUT = 20000;
        public const int RETRY_TIMEOUT = 200;
        #endregion

        /// <summary>
        /// Sends a POST request
        /// </summary>
        /// <param name="postData">A string of data to post</param>
        /// <param name="authentification">An IAuthentificationConfiguration.  Only Basic is currently supported.</param>
        /// <param name="endpoint">The endpoint to send the statement to</param>
        /// <returns>A response string</returns>
        public static string PostRequest(string postData, string endpoint, IAuthenticationConfiguration authentification, string x_experience_api_version)
        {
            return PostRequest(postData, endpoint, authentification, null, x_experience_api_version);
        }

        /// <summary>
        /// Sends a POST request.
        /// </summary>
        /// <param name="postData">A string of data to post</param>
        /// <param name="authentification">An IAuthentificationConfiguration.  Only Basic is currently supported.</param>
        /// <param name="endpoint">The endpoint to send the statement to</param>
        /// <param name="callback">The asynchronous callback</param>
        /// <returns>The returned stream</returns>
        /// <remarks>Providing null for the callback means this is intended to be synchronous.  Any exceptions will be thrown and must be handled in the main thread.</remarks>
        public static string PostRequest(string postData, string endpoint, IAuthenticationConfiguration authentification, TCAPI.AsyncPostCallback callback, string x_experience_api_version)
        {
            string result = null;
            byte[] postDataByteArray = Encoding.UTF8.GetBytes(postData);

            for (int attempt = 0; attempt <= REATTEMPT_COUNT; attempt++)
            {

                WebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = postDataByteArray.Length;
                AddExperienceVersionHeader(request.Headers, x_experience_api_version);
                AddAuthHeader(request.Headers, authentification);

                try
                {
                    result = SendRequest(request, postDataByteArray);
                }
                catch (WebException e)
                {
                    if (attempt < REATTEMPT_COUNT)
                    {
                        try
                        {
                            ThrowHttpException(e);
                        }
                        catch (InternalServerErrorException)
                        {
                            Thread.Sleep(RETRY_TIMEOUT);
                            continue;
                        }
                        catch (ConnectionFailedException)
                        {
                            Thread.Sleep(RETRY_TIMEOUT);
                            continue;
                        }
                        catch (Exception)
                        {
                            // Suppress it for now
                        }
                    }
                    if (callback == null)
                    {
                        // Synchronous request, let the implementer decide
                        throw e;
                    }
                    else
                    {
                        try
                        {
                            ThrowHttpException(e);
                        }
                        catch (InternalServerErrorException)
                        {
                            // Connectivity Issues
                            callback.AsyncPostConnectionFailed(e);
                        }
                        catch (ConnectionFailedException)
                        {
                            // Connectivity Issues
                            callback.AsyncPostConnectionFailed(e);
                        }
                        catch (Exception)
                        {
                            // Permanent failure
                            callback.AsyncPostFailure(e);
                        }
                    }
                }
                break;
            }
            return result;
        }

        #region Async Post
        /*
        /// <summary>
        /// Issues a post request asynchronously
        /// </summary>
        /// <param name="postData">A string of data to post</param>
        /// <param name="authentification">An IAuthentificationConfiguration.  Only Basic is currently supported.</param>
        /// <param name="endpoint">The endpoint to send the statement to</param>
        /// <param name="callback">An ITCAPI object to handle callbacks</param>
        public static void PostRequestAsync(string postData, string endpoint, IAuthenticationConfiguration authentification, ITCAPICallback callback, TCAPI.AsyncPostCallback asyncPostCallback)
        {
            byte[] postDataByteArray = Encoding.UTF8.GetBytes(postData);
            //WebClient webClient = new WebClient();
            WriteRequestState state = new WriteRequestState();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.AllowWriteStreamBuffering = true;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = postDataByteArray.Length;
            request.Timeout = ASYNC_TIMEOUT;
            // Needs OAuth support which will certainly change this line
            request.Headers["Authorization"] = authentification.GetAuthorization();

            Stream dataStream = request.GetRequestStream();

            state.Request = request;
            state.Callback = callback;
            state.DataStream = dataStream;
            state.PostData = postData;
            state.AsyncPostCallback = asyncPostCallback;

            dataStream.BeginWrite(postDataByteArray, 0, postDataByteArray.Length, BeginRequest, state);
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
                    state.AsyncPostCallback.AsyncPostComplete(statements);
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
        */
        #endregion

        /// <summary>
        /// Sends a PUT request with the default content-type of "application/json"
        /// </summary>
        /// <param name="putData">The data to PUT to the server</param>
        /// <param name="queryParameters">A name-value pair collection of query parameters</param>
        /// <returns>The response string</returns>
        public static string PutRequest(string putData, NameValueCollection queryParameters, string endpoint, IAuthenticationConfiguration authentification, string x_experience_api_version)
        {
            return PutRequest(putData, queryParameters, endpoint, authentification, "application/json", x_experience_api_version);
        }

        /// <summary>
        /// Sends a PUT request
        /// </summary>
        /// <param name="putData">The data to PUT to the server</param>
        /// <param name="queryParameters">A name-value pair collection of query parameters</param>
        /// <param name="headers">Additional headers</param>
        /// <returns>The response string</returns>
        public static string PutRequest(string putData, NameValueCollection queryParameters, string endpoint, IAuthenticationConfiguration authentification, string contentType, string x_experience_api_version)
        {
            return PutRequest(putData, queryParameters, endpoint, authentification, contentType, string.Empty, x_experience_api_version);
        }
        
        /// <summary>
        /// Sends a PUT request
        /// </summary>
        /// <param name="putData">The data to PUT to the server</param>
        /// <param name="queryParameters">A name-value pair collection of query parameters</param>
        /// <param name="headers">Additional headers</param>
        /// <returns>The response string</returns>
        public static string PutRequest(string putData, NameValueCollection queryParameters, string endpoint, IAuthenticationConfiguration authentification, string contentType, string eTag, string x_experience_api_version)
        {
            string result = null;
            if (queryParameters != null)
                endpoint += ToQueryString(queryParameters);
            for (int attempt = 0; attempt < REATTEMPT_COUNT; attempt++)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                byte[] putDataByteArray = Encoding.UTF8.GetBytes(putData);
                request.Method = "PUT";
                request.ContentType = contentType;
                request.ContentLength = putDataByteArray.Length;
                AddExperienceVersionHeader(request.Headers, x_experience_api_version);
                if (!String.IsNullOrEmpty(eTag))
                    request.Headers["If-Match"] = "\"" + eTag + "\"";
                AddAuthHeader(request.Headers, authentification);

                try
                {
                    result = SendRequest(request, putDataByteArray);
                }
                catch (InternalServerErrorException e)
                {
                    if (attempt < REATTEMPT_COUNT)
                    {
                        Thread.Sleep(RETRY_TIMEOUT);
                        continue;
                    }
                    throw e;
                }
                catch (ConnectionFailedException e)
                {

                    if (attempt < REATTEMPT_COUNT)
                    {
                        Thread.Sleep(RETRY_TIMEOUT);
                        continue;
                    }
                    throw e;
                }
                catch (WebException e)
                {
                    ThrowHttpException(e);
                }
                break;
            }
            return result;
        }

        /// <summary>
        /// Sends a GET request
        /// </summary>
        /// <param name="getData">A name-value pair collection of query parameters</param>
        /// <returns>The response string</returns>
        public static string GetRequest(NameValueCollection getData, string endpoint, IAuthenticationConfiguration authentification, string x_experience_api_version)
        {
            string result = null;
            if (getData != null)
                endpoint += ToQueryString(getData);
            for (int attempt = 0; attempt <= REATTEMPT_COUNT; attempt++)
            {
                WebRequest request = WebRequest.Create(endpoint);
                request.Method = "GET";
                AddAuthHeader(request.Headers, authentification);
                AddExperienceVersionHeader(request.Headers, x_experience_api_version);

                try
                {
                    result = SendRequest(request);
                }
                catch (InternalServerErrorException e)
                {
                    if (attempt < REATTEMPT_COUNT)
                    {
                        Thread.Sleep(RETRY_TIMEOUT);
                        continue;
                    }
                    throw e;
                }
                catch (ConnectionFailedException e)
                {

                    if (attempt < REATTEMPT_COUNT)
                    {
                        Thread.Sleep(RETRY_TIMEOUT);
                        continue;
                    }
                    throw e;
                }
                catch (WebException e)
                {
                    ThrowHttpException(e);
                }
                break;
            }
            return result;
        }

        /// <summary>
        /// Sends a GET request
        /// </summary>
        /// <param name="getData">A name-value pair collection of query parameters</param>
        /// <param name="whc">Allows for retrieval of the webheaders if they are needed.</param>
        /// <returns>The response string</returns>
        public static string GetRequest(NameValueCollection getData, string endpoint, IAuthenticationConfiguration authentification, out WebHeaderCollection whc, string x_experience_api_version)
        {
            string result = null;
            whc = null;
            if (getData != null)
                endpoint += ToQueryString(getData);
            for (int attempt = 0; attempt <= REATTEMPT_COUNT; attempt++)
            {
                WebRequest request = WebRequest.Create(endpoint);
                request.Method = "GET";
                AddAuthHeader(request.Headers, authentification);
                AddExperienceVersionHeader(request.Headers, x_experience_api_version);

                try
                {
                    result = SendRequest(request, null, out whc);
                }
                catch (InternalServerErrorException e)
                {
                    if (attempt < REATTEMPT_COUNT)
                    {
                        Thread.Sleep(RETRY_TIMEOUT);
                        continue;
                    }
                    throw e;
                }
                catch (ConnectionFailedException e)
                {

                    if (attempt < REATTEMPT_COUNT)
                    {
                        Thread.Sleep(RETRY_TIMEOUT);
                        continue;
                    }
                    throw e;
                }
                catch (WebException e)
                {
                    ThrowHttpException(e);
                }
                break;
            }
            return result;
        }

        /// <summary>
        /// Sends a DELETE request
        /// </summary>
        /// <param name="deleteData">A name-value pair collection of query parameters</param>
        /// <returns>The response string</returns>
        public static string DeleteRequest(NameValueCollection deleteData, string endpoint, IAuthenticationConfiguration authentification, string x_experience_api_version)
        {
            string result = null;
            string end = endpoint + ToQueryString(deleteData);
            for (int attempt = 0; attempt <= REATTEMPT_COUNT; attempt++)
            {
                WebRequest request = WebRequest.Create(endpoint + ToQueryString(deleteData));
                request.Method = "DELETE";
                AddAuthHeader(request.Headers, authentification);
                AddExperienceVersionHeader(request.Headers, x_experience_api_version);

                try
                {
                    result = SendRequest(request);
                }
                catch (InternalServerErrorException e)
                {
                    if (attempt < REATTEMPT_COUNT)
                    {
                        Thread.Sleep(RETRY_TIMEOUT);
                        continue;
                    }
                    throw e;
                }
                catch (ConnectionFailedException e)
                {

                    if (attempt < REATTEMPT_COUNT)
                    {
                        Thread.Sleep(RETRY_TIMEOUT);
                        continue;
                    }
                    throw e;
                }
                catch (WebException e)
                {
                    ThrowHttpException(e);
                }
                break;
            }
            return result;
        }

        private static string SendRequest(WebRequest request)
        {
            return SendRequest(request, null);
        }

        private static string SendRequest(WebRequest request, byte[] data)
        {
            string result = string.Empty;
            try
            {
                if (data != null)
                {
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(data, 0, data.Length);
                    dataStream.Close();
                }

                WebResponse response = request.GetResponse();
                Stream returnStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(returnStream);
                result = reader.ReadToEnd();
                response.Close();
                returnStream.Close();
                reader.Close();
            }
            catch (WebException e)
            {
                ThrowHttpException(e);
            }
            return result;
        }

        private static string SendRequest(WebRequest request, byte[] data, out WebHeaderCollection whc)
        {
            string result = string.Empty;
            whc = null;
            try
            {
                if (data != null)
                {
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(data, 0, data.Length);
                    dataStream.Close();
                }

                WebResponse response = request.GetResponse();
                Stream returnStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(returnStream);
                result = reader.ReadToEnd();

                whc = response.Headers;

                response.Close();
                returnStream.Close();
                reader.Close();
            }
            catch (WebException e)
            {
                ThrowHttpException(e);
            }
            return result;
        }

        private static void AddAuthHeader(WebHeaderCollection nvc, IAuthenticationConfiguration auth)
        {
            if (auth is BasicHTTPAuth)
            {
                nvc["Authorization"] = auth.GetAuthorization();
            }
            else if (auth is OAuthAuthentication)
            {
            }
        }

        private static void AddExperienceVersionHeader(WebHeaderCollection whc, string x_experience_api_version)
        {
            if (!String.IsNullOrEmpty(x_experience_api_version))
            {
                whc["X-Experience-API-Version"] = x_experience_api_version;
            }
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
                if (r != null)
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
                        case HttpStatusCode.BadGateway:
                        case HttpStatusCode.GatewayTimeout:
                        case HttpStatusCode.RequestTimeout:
                        case HttpStatusCode.ServiceUnavailable:
                            throw new ConnectionFailedException("Bad connection " + ((HttpWebResponse)httpResponse).StatusCode + ".  " + response);
                    }
                }
                else
                {
                    throw new ConnectionFailedException("No internet connection.");
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
