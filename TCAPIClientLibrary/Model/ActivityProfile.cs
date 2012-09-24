
using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class ActivityProfile : State
    {
        string activityId;
        string profileId;
        string body;
        string contentType;

        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
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
