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
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace RusticiSoftware.TinCanAPILibrary.Model.TinCan090
{
    public class Actor : StatementTarget, IValidatable
    {
        #region Constants
        protected static readonly string OBJECT_TYPE = "Agent";

        private const string account_key = "account";
        private const string mbox_key = "mbox";
        private const string mbox_sha1sum_key = "mbox_sha1sum";
        private const string openid_key = "openid";
        #endregion

        #region Fields
        private string[] name;
        private string[] mbox;
        private string[] mbox_sha1sum;
        private string[] openid;
        private AgentAccount[] account;
        #endregion

        #region Properties
        /// <summary>
        /// ObjectType accessor
        /// </summary>
        public override string ObjectType
        {
            get { return OBJECT_TYPE; }
        }

        /// <summary>
        /// Array of names for the actor
        /// </summary>
        public string[] Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Array of mailboxes for the actor
        /// </summary>
        public string[] Mbox
        {
            get { return mbox; }
            set
            {
                string mboxPrefix = "mailto:";
                string[] normalized = CommonFunctions.ArrayToLower(value);
                if (normalized != null)
                {
                    foreach (string s in normalized)
                    {
                        if (!s.StartsWith(mboxPrefix))
                        {
                            throw new ArgumentException(
                                "Mbox value " + s + " must begin with mailto: prefix",
                                "value");
                        }
                        if (!ValidationHelper.IsValidEmailAddress(s.Substring(mboxPrefix.Length)))
                        {
                            throw new ArgumentException(
                                "Mbox value " + s + " is not a valid email address.",
                                "value");
                        }
                    }
                }
                mbox = normalized;
            }
        }

        /// <summary>
        /// Array of email sha1sums for the actor
        /// </summary>
        public string[] Mbox_sha1sum
        {
            get { return mbox_sha1sum; }
            set
            {
                mbox_sha1sum = CommonFunctions.ArrayToLower(value);
            }
        }

        /// <summary>
        /// Array of OpenIDs for the actor
        /// </summary>
        public string[] Openid
        {
            get { return openid; }
            set
            {
                openid = CommonFunctions.ArrayToLower(value);
            }
        }

        /// <summary>
        /// A list of accounts for the actor
        /// </summary>
        public AgentAccount[] Account
        {
            get { return account; }
            set
            {
                if (value != null)
                {
                    // TODO - reconsider whether to deep-validate in setters
                    var failures = new List<ValidationFailure>();
                    foreach (AgentAccount a in value)
                    {
                        if (a == null)
                        {
                            throw new ArgumentException("Invalid null AgentAccount member supplied to Account");
                        }
                        else
                        {
                            failures.AddRange(a.Validate(earlyReturnOnFailure: true));
                            if (failures.Count > 0)
                            {
                                throw new ArgumentException(failures[0].Error);
                            }
                        }
                    }
                }
                account = value;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new actor
        /// </summary>
        public Actor() : base() { }

        /// <summary>
        /// Constructor invoked by the Person Subclass
        /// </summary>
        /// <param name="mbox"></param>
        /// <param name="mbox_sha1sum"></param>
        /// <param name="openid"></param>
        /// <param name="account"></param>
        protected Actor(string mbox, string mbox_sha1sum, string openid, AgentAccount account)
        {
            if (mbox != null)
            {
                this.Mbox = new string[] { mbox };
            }
            if (mbox_sha1sum != null)
            {
                this.Mbox_sha1sum = new string[] { mbox_sha1sum };
            }
            if (openid != null)
            {
                this.Openid = new string[] { openid };
            }
            if (account != null)
            {
                this.Account = new AgentAccount[] { account };
            }
        }
        /// <summary>
        /// Constructor invoked by the Person Subclass
        /// </summary>
        /// <param name="mbox"></param>
        /// <param name="mbox_sha1sum"></param>
        /// <param name="openid"></param>
        /// <param name="account"></param>
        protected Actor(string[] mbox, string[] mbox_sha1sum, string[] openid, AgentAccount[] account)
        {
            this.mbox = mbox;
            this.mbox_sha1sum = mbox_sha1sum;
            this.openid = openid;
            this.account = account;
            // TODO - Strongly reconsider deep validation on construction
            var failures = new List<ValidationFailure>(this.Validate(earlyReturnOnFailure: true));
            if (failures.Count > 0)
            {
                throw new ArgumentException(failures[0].Error);
            }
        }
        /// <summary>
        /// Creates a new actor.
        /// </summary>
        /// <param name="name">Actors name</param>
        /// <param name="mbox">Actors e-mail (inverse functional property)</param>
        /// <param name="mbox_sha1sum">Actors e-mail sha1sum (inverse functional property)</param>
        /// <param name="openid">Actors openid (inverse functional property)</param>
        /// <param name="account">Actors agent account (inverse functional property)</param>
        public Actor(string name, string mbox, string mbox_sha1sum, string openid, AgentAccount account)
        {
            this.name = new string[] { name };
            // Properties are used to force validation and normalization
            if (mbox != null)
            {
                this.Mbox = new string[] { mbox };
            }
            if (mbox_sha1sum != null)
            {
                this.Mbox_sha1sum = new string[] { mbox_sha1sum };
            }
            if (openid != null)
            {
                this.Openid = new string[] { openid };
            }
            if (account != null)
            {
                this.Account = new AgentAccount[] { account };
            }
            // TODO - Strongly reconsider deep validation on construction
            var failures = new List<ValidationFailure>(this.Validate(earlyReturnOnFailure: true));
            if (failures.Count > 0)
            {
                throw new ArgumentException(failures[0].Error);
            }
        }
        /// <summary>
        /// Creates a new actor.
        /// </summary>
        /// <param name="name">Actors name</param>
        /// <param name="mbox">Actors e-mail (inverse functional property)</param>
        public Actor(string name, string mbox)
            : this(name, mbox, null, null, null)
        {
        }

        /// <summary>
        /// Creates a new actor, specifying arrays for all properties.
        /// </summary>
        /// <param name="name">Actor name array</param>
        /// <param name="mbox">Actor mbox array</param>
        /// <param name="mbox_sha1sum">Actor mbox_sha1sum array</param>
        /// <param name="openid">Actor openid array</param>
        /// <param name="account">Actor account array</param>
        public Actor(string[] name, string[] mbox, string[] mbox_sha1sum, string[] openid, AgentAccount[] account)
        {
            this.name = name;
            this.mbox = mbox;
            this.mbox_sha1sum = mbox_sha1sum;
            this.openid = openid;
            this.account = account;
            // TODO - Strongly reconsider deep validation on construction
            var failures = new List<ValidationFailure>(this.Validate(earlyReturnOnFailure: true));
            if (failures.Count > 0)
            {
                throw new ArgumentException(failures[0].Error);
            }
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="actor">Actor to copy</param>
        protected Actor(Actor source)
        {
            this.name = cloneArray(source.Name);
            this.mbox = cloneArray(source.Mbox);
            this.mbox_sha1sum = cloneArray(source.Mbox_sha1sum);
            this.openid = cloneArray(source.Openid);
            this.account = cloneArray(source.Account);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates that the object abides by its rules
        /// </summary>
        public virtual IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            if (!ValidateArray(name) && !ValidateArray(mbox) &&
                !ValidateArray(mbox_sha1sum) && !ValidateArray(openid) &&
                !ValidateArray(account))
            {
                return new List<ValidationFailure>() { new ValidationFailure("At least once inverse functional property must be defined") };
            }
            return new List<ValidationFailure>();
        }

        /// <summary>
        /// Gets the Hash Code of the object.  However, due to the odd nature of the
        /// Actor object, the properties of a Hash cannot be fulfilled, so it is
        /// not recommended to use this object in a HashTable.
        /// </summary>
        /// <returns>0.  object is not effectively hashable</returns>
        public override int GetHashCode()
        {
            return 0;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Validates an Array (determines if all array entries are non-null and non-empty)
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private bool ValidateArray(object[] array)
        {
            if (array == null)
            {
                return false;
            }
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                {
                    return false;
                }
            }
            if (array is string[])
            {
                array = array as string[];
                foreach (string s in array)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// clones an array, w/o caller needing to cast the array after cloning.
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="src">source array</param>
        /// <returns>copied array</returns>
        protected T[] cloneArray<T>(T[] src)
        {
            if (src == null)
            {
                return null;
            }
            return (T[])src.Clone();
        }
        #endregion

        #region TinCan 0.95 Promotion
        /// <summary>
        /// Promotes a 0.90 actor to a 0.95 actor
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static explicit operator Model.Actor(Actor source)
        {
            if (source.account != null && source.account.Length > 0)
            {
                return new Model.Actor(source.Name[0], source.Account[0]);
            }
            if (source.openid != null && source.openid.Length > 0)
            {
                Model.Actor result = new Model.Actor();
                result.Name = source.Name[0];
                result.Openid = source.openid[0];
                return result;
            }
            if (source.mbox != null && source.mbox.Length > 0)
            {
                return new Model.Actor(source.Name[0], source.Mbox[0]);
            }
            return null;
        }
        #endregion
    }
}
