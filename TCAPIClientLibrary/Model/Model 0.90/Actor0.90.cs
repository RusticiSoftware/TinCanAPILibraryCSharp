using System;
using System.Collections.Generic;
using System.Text;
using RusticiSoftware.TinCanAPILibrary.Exceptions;
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace RusticiSoftware.TinCanAPILibrary.Model.TinCan090
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
        protected String[] name;
        protected String[] mbox;
        protected String[] mbox_sha1sum;
        protected String[] openid;
        protected AgentAccount[] account;
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
        public String[] Name
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
        public String[] Mbox
        {
            get { return mbox; }
            set
            {
                String mboxPrefix = "mailto:";
                String[] normalized = CommonFunctions.ArrayToLower(value);
                if (normalized != null)
                {
                    foreach (String s in normalized)
                    {
                        if (!s.StartsWith(mboxPrefix))
                        {
                            throw new InvalidArgumentException(
                                "Mbox value " + s + " must begin with mailto: prefix");
                        }
                        if (!ValidationHelper.IsValidEmailAddress(s.Substring(mboxPrefix.Length)))
                        {
                            throw new InvalidArgumentException(
                                "Mbox value " + s + " is not a valid email address.");
                        }
                    }
                }
                mbox = normalized;
            }
        }

        /// <summary>
        /// Array of email sha1sums for the actor
        /// </summary>
        public String[] Mbox_sha1sum
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
        public String[] Openid
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
                    foreach (AgentAccount a in value)
                        a.Validate();
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
                this.Mbox = new String[] { mbox };
            if (mbox_sha1sum != null)
                this.Mbox_sha1sum = new String[] { mbox_sha1sum };
            if (openid != null)
                this.Openid = new String[] { openid };
            if (account != null)
                this.Account = new AgentAccount[] { account };
        }
        /// <summary>
        /// Constructor invoked by the Person Subclass
        /// </summary>
        /// <param name="mbox"></param>
        /// <param name="mbox_sha1sum"></param>
        /// <param name="openid"></param>
        /// <param name="account"></param>
        protected Actor(String[] mbox, String[] mbox_sha1sum, String[] openid, AgentAccount[] account)
        {
            this.mbox = mbox;
            this.mbox_sha1sum = mbox_sha1sum;
            this.openid = openid;
            this.account = account;
            Validate();
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
            this.name = new String[] { name };
            // Properties are used to force validation and normalization
            if (mbox != null)
                this.Mbox = new String[] { mbox };
            if (mbox_sha1sum != null)
                this.Mbox_sha1sum = new String[] { mbox_sha1sum };
            if (openid != null)
                this.Openid = new String[] { openid };
            if (account != null)
                this.Account = new AgentAccount[] { account };
            Validate();
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
        public Actor(String[] name, String[] mbox, String[] mbox_sha1sum, String[] openid, AgentAccount[] account)
        {
            this.name = name;
            this.mbox = mbox;
            this.mbox_sha1sum = mbox_sha1sum;
            this.openid = openid;
            this.account = account;
            Validate();
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
        public virtual void Validate()
        {
            if (!ValidateArray(name) && !ValidateArray(mbox) &&
                !ValidateArray(mbox_sha1sum) && !ValidateArray(openid) &&
                !ValidateArray(account))
            {
                throw new ValidationException("At least once inverse functional property must be defined");
            }
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

        #region Private Methods
        /// <summary>
        /// Validates an Array (determines if all array entries are non-null and non-empty)
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private bool ValidateArray(object[] array)
        {
            if (array == null)
                return false;
            for (int i = 0; i < array.Length; i++)
                if (array[i] == null)
                    return false;
            if (array is String[])
            {
                array = (array as String[]);
                foreach (String s in array)
                {
                    if (String.IsNullOrEmpty(s))
                        return false;
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
    }
}
