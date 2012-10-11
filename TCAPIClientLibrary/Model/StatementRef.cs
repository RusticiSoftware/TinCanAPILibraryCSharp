using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class StatementRef : StatementTarget
    {
        #region Constants
        protected static readonly String OBJECT_TYPE = "StatementRef";
        #endregion

        #region Fields
        private String id;
        #endregion

        #region Properties
        public String Id
        {
            get { return id; }
            set { id = value; }
        }
        public override string ObjectType
        {
            get { return OBJECT_TYPE; }
        }
        #endregion

        #region Constructor
        public StatementRef(string id)
        {
            this.id = id;
        }
        #endregion
    }
}
