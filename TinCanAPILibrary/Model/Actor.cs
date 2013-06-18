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
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class Actor : StatementTarget, IValidatable
    {
        #region Constants
        protected static readonly String OBJECT_TYPE = "Agent";

        private const string account_key = "account";
        private const string mbox_key = "mbox";
        private const string mbox_sha1sum_key = "mbox_sha1sum";
        private const string openid_key = "openid";
        #endregion

        #region Fields
        protected String name;
        protected String mbox;
        protected String mbox_sha1sum;
        protected String openid;
        protected AgentAccount account;
        #endregion

        #region Properties
        /// <summary>
        /// ObjectType accessor
        /// </summary>
        public override String ObjectType
        {
            get { return OBJECT_TYPE; }
        }

        /// <summary>
        /// Array of names for the actor
        /// </summary>
        public String Name
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
        public String Mbox
        {
            get { return mbox; }
            set
            {
                String mboxPrefix = "mailto:";
                String normalized = value.ToLower();
                if (normalized != null)
                {
                    if (!normalized.StartsWith(mboxPrefix))
                    {
                        throw new InvalidArgumentException(
                            "Mbox value " + normalized + " must begin with mailto: prefix");
                    }
                    if (!ValidationHelper.IsValidEmailAddress(normalized.Substring(mboxPrefix.Length)))
                    {
                        throw new InvalidArgumentException(
                            "Mbox value " + normalized + " is not a valid email address.");
                    }
                }
                mbox = normalized;
            }
        }

        /// <summary>
        /// Array of email sha1sums for the actor
        /// </summary>
        public String Mbox_sha1sum
        {
            get { return mbox_sha1sum; }
            set
            {
                mbox_sha1sum = value.ToLower();
            }
        }

        /// <summary>
        /// Array of OpenIDs for the actor
        /// </summary>
        public String Openid
        {
            get { return openid; }
            set
            {
                openid = value.ToLower();
            }
        }

        /// <summary>
        /// A list of accounts for the actor
        /// </summary>
        public AgentAccount Account
        {
            get { return account; }
            set
            {
                if (value != null)
                {

                    // TODO - reconsider whether to deep-validate in setters
                    var failures = new List<ValidationFailure>(value.Validate(earlyReturnOnFailure: true));
                    if (failures.Count > 0)
                    {
                        throw new ArgumentException(failures[0].Error);
                    }
                    account = value;
                }
            }
        }

        #endregion

        #region Constructor
        public Actor() { }
        public Actor(string name, string email)
        {
            this.name = name;
            this.Mbox = email;
        }

        public Actor(string name, AgentAccount account)
        {
            this.name = name;
            this.account = account;
        }

        public Actor(Actor src)
        {
            this.name = src.Name;
            this.Mbox = src.mbox;
            this.mbox_sha1sum = src.mbox_sha1sum;
            this.openid = src.openid;
            this.account = src.account;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates that the object abides by its rules
        /// </summary>
        public virtual IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            int properties = 0;
            if (!String.IsNullOrEmpty(mbox))
            {
                properties++;
            }
            if (!String.IsNullOrEmpty(mbox_sha1sum))
            {
                properties++;
            }
            if (!String.IsNullOrEmpty(openid))
            {
                properties++;
            }
            if (account != null)
            {
                properties++;
            }
            if (properties != 1)
            {
                return new List<ValidationFailure>() 
                { 
                    new ValidationFailure("Exactly 1 inverse functional properties must be defined.  However, " + properties + " are defined.") 
                };
            }
            return new List<ValidationFailure>();
        }

        /// <summary>
        /// Gets the Hash Code of the object.  However, due to the odd nature of the
        /// Actor object, the properties of a Hash cannot be fulfilled, so it is
        /// not recommended to use this object in a HashTable.
        /// </summary>
        /// <returns>0.  Object is not effectively hashable</returns>
        public override int GetHashCode()
        {
            return 0;
        }

        #endregion

        #region TinCan 0.90 Conversion
        /// <summary>
        /// Down-converts to a TinCan 0.90 Actor
        /// </summary>
        /// <param name="source">A tincan 0.95 Actor</param>
        /// <returns>A tincan 0.90 actor</returns>
        /// <remarks></remarks>
        public static explicit operator Model.TinCan090.Actor(Actor source)
        {
            return new Model.TinCan090.Actor(source.Name, source.Mbox, source.Mbox_sha1sum, source.Openid, source.Account);
        }
        #endregion
    }
}