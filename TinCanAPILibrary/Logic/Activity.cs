using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Logic
{
    public class Activity : StatementTarget
    {
        private const string OBJECT_TYPE = "activity";

        string id;
        ActivityDefinition definition;

        /// <summary>
        /// The activity ID
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// The Activity Definition
        /// </summary>
        public ActivityDefinition Definition
        {
            get { return definition; }
            set { definition = value; }
        }

        /// <summary>
        /// The object type for statement posts
        /// </summary>
        public override String ObjectType
        {
            get { return OBJECT_TYPE; }
        }
    }
}
