using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RusticiSoftware.TinCanAPILibrary.Exceptions;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class Group : Actor
    {
        #region Constants
        protected static readonly String OBJECT_TYPE = "Group";
        #endregion

        #region Fields
        protected Actor[] member;
        #endregion

        #region Properties
        public String ObjectType
        {
            get { return OBJECT_TYPE; }
        }

        public Actor[] Member
        {
            get { return member; }
            set { member = value; }
        }
        #endregion

        #region Constructor
        public Group() : base() { }
        #endregion

        #region Public Methods
        public override void Validate()
        {
            if (member == null || member.Length == 0)
            {
                throw new ValidationException("Group must be populated");
            }
            foreach (Actor a in member)
            {
                a.Validate();
            }
        }
        #endregion
    }
}
