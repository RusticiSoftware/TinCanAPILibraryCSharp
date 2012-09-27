using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class Interaction : Activity
    {
        private InteractionDefinition definition;

        public new InteractionDefinition Definition
        {
            get { return definition; }
            set { definition = value; }
        }
    }
}
