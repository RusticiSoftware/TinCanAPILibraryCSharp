using System;
using System.Collections.Generic;
using System.Linq;
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
