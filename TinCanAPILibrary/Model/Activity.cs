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

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class Activity : StatementTarget, IValidatable
    {
        #region Constants
        protected static readonly string OBJECT_TYPE = "Activity";
        #endregion

        #region Fields
        private string id;
        private ActivityDefinition definition;
        #endregion

        #region Properties
        public override string ObjectType
        {
            get { return OBJECT_TYPE; }
        }

        public string Id
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
        public Activity() { }

        public Activity(string id)
        {
            this.id = id;
        }

        public Activity(string id, ActivityDefinition definition)
        {
            this.id = id;
            this.definition = definition;
        }
        #endregion

        #region Public Methods
        public IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (id == null)
            {
                failures.Add(new ValidationFailure("Activity does not have an identifier"));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            if (definition != null && definition is IValidatable)
            {
                failures.AddRange(((IValidatable)definition).Validate(earlyReturnOnFailure));
                if (earlyReturnOnFailure && failures.Count > 0)
                {
                    return failures;
                }
            }
            return failures;
        }
        #endregion
    }
}
