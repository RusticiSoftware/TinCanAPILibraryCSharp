using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RusticiSoftware.TinCanAPILibrary.Helper;
using RusticiSoftware.TinCanAPILibrary.Exceptions;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    /// <summary>
    /// Class represents the context of a TinCan Statement
    /// </summary>
    public class Context : Extensible, IValidatable
    {
        #region Fields
        protected String registration;
        protected Actor instructor;
        protected Actor team;
        protected ContextActivities contextActivities;
        protected String revision;
        protected String platform;
        protected Statement statement;
        #endregion

        #region Properties
        /// <summary>
        /// The Registration UUID
        /// </summary>
        public String Registration
        {
            get { return registration; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    registration = null;
                }
                else
                {
                    String normalized = value.ToLower();
                    if (!ValidationHelper.IsValidUUID(normalized))
                    {
                        throw new InvalidArgumentException("Registration must be UUID");
                    }
                    registration = normalized;
                }
            }
        }

        /// <summary>
        /// The instructor in this context
        /// </summary>
        public Actor Instructor
        {
            get { return instructor; }
            set { instructor = value; }
        }

        /// <summary>
        /// The team in this context
        /// </summary>
        public Actor Team
        {
            get { return team; }
            set { team = value; }
        }

        /// <summary>
        /// The Activities in this Context
        /// </summary>
        public ContextActivities ContextActivities
        {
            get { return contextActivities; }
            set { contextActivities = value; }
        }

        /// <summary>
        /// The revision
        /// </summary>
        public String Revision
        {
            get { return revision; }
            set { revision = value; }
        }

        /// <summary>
        /// The platform
        /// </summary>
        public String Platform
        {
            get { return platform; }
            set { platform = value; }
        }

        /// <summary>
        /// The statement
        /// </summary>
        public Statement Statement
        {
            get { return statement; }
            set { statement = value; }
        }
        #endregion

        #region Constructor
        public Context() { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates the context
        /// </summary>
        public void Validate()
        {
            Object[] children = new Object[] { registration, instructor, team, 
                contextActivities, revision, platform, statement };
            foreach (Object o in children)
            {
                if (o != null && o is IValidatable)
                {
                    ((IValidatable)o).Validate();
                }
            }
        }
        #endregion
    }
}
