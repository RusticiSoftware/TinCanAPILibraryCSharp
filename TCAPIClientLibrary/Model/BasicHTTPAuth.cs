#region License
/*
Copyright 2012 Rustici Software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion
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
            byte[] dataToEncode = Encoding.UTF8.GetBytes(username + ":" + password);
            authHeaderValue = "Basic " + Convert.ToBase64String(dataToEncode);
        }
        #endregion
    }
}
