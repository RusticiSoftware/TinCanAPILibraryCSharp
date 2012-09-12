using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class InteractionComponent
    {
        protected String id;
        protected LanguageMap description;

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public LanguageMap Description
        {
            get
            {
                return description;
            }
            //Including for use by deserialization code. Do not use.
            set
            {
                if (description == null || description.Count == 0)
                {
                    description = value;
                }
                else
                {
                    throw new InvalidOperationException("Can't overwrite populated description.");
                }
            }
        }
    }
}
