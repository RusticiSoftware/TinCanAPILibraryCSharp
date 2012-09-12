
using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class ActivityProfile : State
    {
        string activityId;
        string profileId;
        string contents;

        public string Contents
        {
            get { return contents; }
            set { contents = value; }
        }

        public string ProfileId
        {
            get { return profileId; }
            set { profileId = value; }
        }

        public string ActivityId
        {
            get { return activityId; }
            set { activityId = value; }
        }
    }
}
