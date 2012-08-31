using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Logic
{
    public class ActivityState : State
    {
        string activityId;
        Actor actor;
        string registration;
        string contents;
        string stateId;

        public string StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }

        public string Contents
        {
            get { return this.contents; }
            set { this.contents = value; }
        }

        public string ActivityId
        {
            get { return activityId; }
            set { activityId = value; }
        }

        public Actor Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        public string Registration
        {
            get { return registration; }
            set { registration = value; }
        }
    }
}
