using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class BasicHTTPAuth : IAuthenticationConfiguration
    {
        #region Fields
        private String username;
        private String password;
        private String authHeaderValue;
        #endregion

        #region Properties
        public String Username
        {
            get { return username; }
            set
            { 
                username = value;
                GenerateAuthHeader();
            }
        }

        public String Password
        {
            get { return password; }
            set 
            {
                password = value;
                GenerateAuthHeader();
            }
        }

        public String AuthHeaderValue
        {
            get { return authHeaderValue; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates an empty HTTP Auth Object
        /// </summary>
        public BasicHTTPAuth() { }
        
        /// <summary>
        /// Creates an HTTP Auth Object with the necessary parameters
        /// </summary>
        /// <param name="username">Authentication Username</param>
        /// <param name="password">Authentication Password</param>
        public BasicHTTPAuth(string username, string password)
        {
            this.username = username;
            this.password = password;
            GenerateAuthHeader();
        }
        #endregion

        #region Public Methods
        public string GetAuthorization()
        {
            return authHeaderValue;
        }
        #endregion

        #region Private Methods
        private void GenerateAuthHeader()
        {
            byte[] dataToEncode = ASCIIEncoding.ASCII.GetBytes(username + ":" + password);
            authHeaderValue = "Basic " + Convert.ToBase64String(dataToEncode);
        }
        #endregion
    }
}
