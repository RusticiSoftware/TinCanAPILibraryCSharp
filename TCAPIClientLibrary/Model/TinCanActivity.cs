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
