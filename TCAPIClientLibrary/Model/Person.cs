using System;
using System.Collections.Generic;

using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class Person : Actor
    {
        #region Constants
        protected new static readonly String OBJECT_TYPE = "Person";
        #endregion

        #region Fields
        protected String[] familyName;
        protected String[] givenName;
        protected String[] lastName;
        protected String[] firstName;
        #endregion

        #region Constructor
        public Person() { }
        /// <summary>
        /// Creates a person, specifying a givenName and familyName
        /// </summary>
        /// <param name="givenName">The persons given name</param>
        /// <param name="familyName">The persons family name</param>
        /// <param name="mbox">The persons mailbox</param>
        /// <param name="mbox_sha1sum">The persons mbox sha1sum</param>
        /// <param name="openid">The persons openid</param>
        /// <param name="account">The persons verification account</param>
        public Person(string givenName, string familyName, string mbox, string mbox_sha1sum, string openid, AgentAccount account)
            : this(false, givenName, familyName, mbox, mbox_sha1sum, openid, account)
        {
        }

        /// <summary>
        /// Constructor specifies whether using first and last name or family name
        /// </summary>
        /// <param name="isFirstLastName">If this should assign first and last name</param>
        /// <param name="firstName">The persons first or given name</param>
        /// <param name="lastName">The persons last or family name</param>
        /// <param name="mbox">The persons mailbox</param>
        /// <param name="mbox_sha1sum">The persons mbox sha1sum</param>
        /// <param name="openid">The persons openid</param>
        /// <param name="account">The persons account</param>
        public Person(bool isFirstLastName, string firstName, string lastName, string mbox, string mbox_sha1sum, string openid, AgentAccount account)
            : base(mbox, mbox_sha1sum, openid, account)
        {
            if (!isFirstLastName)
            {
                this.lastName = new String[] { lastName };
                this.firstName = new String[] { firstName };
            }
            else
            {
                this.familyName = new String[] { lastName };
                this.givenName = new String[] { firstName };
            }
        }

        public Person(bool isFirstLastName, String[] firstName, String[] lastName, String[] mbox, String[] mbox_sha1sum, String[] openid, AgentAccount[] account)
            : base(mbox, mbox_sha1sum, openid, account)
        {
            if (!isFirstLastName)
            {
                this.lastName = lastName;
                this.firstName = firstName;
            }
            else
            {
                this.familyName = lastName;
                this.givenName = firstName;
            }
        }
        #endregion

        #region Properties
        public override String ObjectType
        {
            get { return OBJECT_TYPE; }
        }

        public String[] FamilyName
        {
            get { return familyName; }
            set { familyName = value; }
        }

        public String[] GivenName
        {
            get { return givenName; }
            set { givenName = value; }
        }

        public String[] LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public String[] FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        #endregion
    }
}
