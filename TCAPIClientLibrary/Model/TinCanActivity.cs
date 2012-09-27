using System;
using System.Collections.Generic;

using System.Text;
using RusticiSoftware.TinCanAPILibrary.Exceptions;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class TinCanActivity : StatementTarget, IValidatable
    {
        #region Constants
        protected static readonly String OBJECT_TYPE = "Activity";
        #endregion

        #region Fields
        protected String id;
        protected ActivityDefinition definition;
        #endregion

        #region Properties
        public override String ObjectType
        {
            get { return OBJECT_TYPE; }
        }

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public ActivityDefinition Definition
        {
            get { return definition; }
            set { definition = value; }
        }
        #endregion

        #region Constructor
        public TinCanActivity() { }

        public TinCanActivity(String id)
        {
            this.id = id;
        }
        #endregion

        #region Public Methods
        public void Validate()
        {
            if (id == null)
                throw new ValidationException("Activity does not have an identifier");
            if (definition != null && definition is IValidatable)
                ((IValidatable)definition).Validate();
        }
        #endregion
    }
}
