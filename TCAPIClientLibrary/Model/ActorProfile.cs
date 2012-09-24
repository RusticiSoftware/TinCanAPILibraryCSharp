using System;
using System.Collections.Generic;

using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class ActorProfile
    {
        Actor actor;
        string profileId;
        string body;
        string contentType;

        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        public string ProfileId
        {
            get { return profileId; }
            set { profileId = value; }
        }
        public Actor Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        public string Body
        {
            get { return this.body; }
            set { this.body = value; }
        }
    }
}
