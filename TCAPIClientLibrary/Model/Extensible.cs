using System;
using System.Collections.Generic;

using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    /// <summary>
    /// Definition of any extensible TinCanAPI Statement.
    /// Allows for arbitrary key/value pairs to be attached to
    /// an object.
    /// </summary>
    public class Extensible
    {
        protected Dictionary<string, object> extensions;

        public Dictionary<string, object> Extensions
        {
            get { return extensions; }
            set { extensions = value; }
        }
    }
}
