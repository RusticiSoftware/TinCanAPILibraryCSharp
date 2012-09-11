using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class OAuthAuthentication : IAuthenticationConfiguration
    {
        #region Fields
        private String consumerKey;
        private String consumerSecret;
        private String tokenId;
        private String tokenSecret;
        #endregion

        #region Properties
        public String ConsumerKey
        {
            get { return consumerKey; }
            set { consumerKey = value; }
        }

        public String ConsumerSecret
        {
            get { return consumerSecret; }
            set { consumerSecret = value; }
        }

        public String TokenId
        {
            get { return tokenId; }
            set { tokenId = value; }
        }

        public String TokenSecret
        {
            get { return tokenSecret; }
            set { tokenSecret = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates an empty OAuth object
        /// </summary>
        public OAuthAuthentication() { }

        /// <summary>
        /// Creates an OAuth object with the necessary fields set
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="tokenId"></param>
        /// <param name="tokenSecret"></param>
        public OAuthAuthentication(string consumerKey, string consumerSecret, string tokenId, string tokenSecret)
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.tokenId = tokenId;
            this.tokenSecret = tokenSecret;
        }
        #endregion

        #region Public Methods
        public string GetAuthorization()
        {
            return "";
        }
        #endregion
    }
}
