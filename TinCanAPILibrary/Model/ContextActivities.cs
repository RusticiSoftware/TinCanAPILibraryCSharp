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
    public class ContextActivities : IValidatable
    {
        #region Fields
        protected TinCanActivity parent;
        protected TinCanActivity grouping;
        protected TinCanActivity other;
        #endregion

        #region Properties
        public TinCanActivity Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public TinCanActivity Grouping
        {
            get { return grouping; }
            set { grouping = value; }
        }

        public TinCanActivity Other
        {
            get { return other; }
            set { other = value; }
        }
        #endregion

        #region Constructor
        public ContextActivities() { }
        #endregion

        public void Validate()
        {
            //Validate children
            Object[] children = new Object[] { parent, grouping, other };
            foreach (Object child in children)
            {
                if (child != null && child is IValidatable)
                {
                    ((IValidatable)child).Validate();
                }
            }
        }
    }
}
