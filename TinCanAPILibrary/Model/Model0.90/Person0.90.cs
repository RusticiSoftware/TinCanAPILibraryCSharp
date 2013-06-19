﻿#region License
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

namespace RusticiSoftware.TinCanAPILibrary.Model.TinCan090
{
    public class Person : Actor
    {
        #region Constants
        protected static new readonly string OBJECT_TYPE = "Person";
        #endregion

        #region Fields
        private string[] familyName;
        private string[] givenName;
        private string[] lastName;
        private string[] firstName;
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
                this.lastName = new string[] { lastName };
                this.firstName = new string[] { firstName };
            }
            else
            {
                this.familyName = new string[] { lastName };
                this.givenName = new string[] { firstName };
            }
        }

        public Person(bool isFirstLastName, string[] firstName, string[] lastName, string[] mbox, string[] mbox_sha1sum, string[] openid, AgentAccount[] account)
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
        public override string ObjectType
        {
            get { return OBJECT_TYPE; }
        }

        public string[] FamilyName
        {
            get { return familyName; }
            set { familyName = value; }
        }

        public string[] GivenName
        {
            get { return givenName; }
            set { givenName = value; }
        }

        public string[] LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string[] FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        #endregion
    }
}
