using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class StatementTarget
    {
        /// <summary>
        /// Can be any Agent Type (Agent, Person, Group), or Activity (default)
        /// </summary>
        public virtual String ObjectType
        {
            get;
            private set;
        }
    }
}
