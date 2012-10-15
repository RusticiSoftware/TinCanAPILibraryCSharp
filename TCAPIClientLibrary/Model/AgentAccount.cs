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
using RusticiSoftware.TinCanAPILibrary.Exceptions;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    /// <summary>
    /// An agent account to verify the uniqueness of the agent
    /// </summary>
    public class AgentAccount : IValidatable
    {
        protected String accountServiceHomePage;
        protected String accountName;
        private String hashString;

        /// <summary>
        /// The home page of the agent account
        /// </summary>
        public String AccountServiceHomePage
        {
            get { return accountServiceHomePage; }
            set
            {
                accountServiceHomePage = value;
                hashString = null;
            }
        }

        /// <summary>
        /// The username used by the agent to log into this account page
        /// </summary>
        public String AccountName
        {
            get { return accountName; }
            set
            {
                accountName = value;
                hashString = null;
            }
        }

        /// <summary>
        /// Creates a new agent account with no set values
        /// </summary>
        public AgentAccount() { }

        /// <summary>
        /// Creates a new agent account
        /// </summary>
        /// <param name="homePage">Agent account home page</param>
        /// <param name="id">Agent acount ID used to log into the page</param>
        public AgentAccount(string homePage, string id)
        {
            accountServiceHomePage = homePage;
            accountName = id;
        }

        /// <summary>
        /// Generates a hashcode representation of the object
        /// </summary>
        /// <returns>Object hash</returns>
        public override int GetHashCode()
        {
            if (hashString == null)
            {
                hashString = accountServiceHomePage + accountName;
            }
            return hashString.GetHashCode();
        }

        /// <summary>
        /// Validates the object.
        /// </summary>
        /// <exception cref="ValidationException">Thrown when either field is null or empty.</exception>
        public void Validate()
        {
            if (String.IsNullOrEmpty(accountServiceHomePage))
            {
                throw new ValidationException("Account service homepage cannot be null");
            }
            if (String.IsNullOrEmpty(accountName))
            {
                throw new ValidationException("Account name cannot be null");
            }
        }
    }
}
