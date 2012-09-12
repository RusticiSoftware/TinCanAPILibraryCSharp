using System;
using System.Collections.Generic;

using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class StatementTarget
    {
        private string objectType;

        public virtual string ObjectType
        {
            get { return objectType; }
            private set { objectType = value; }
        }
    }
}
