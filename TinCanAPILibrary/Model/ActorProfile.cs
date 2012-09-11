using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class ActorProfile
    {
        Actor actor;
        string profileId;
        string contents;

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

        public string Contents
        {
            get { return this.contents; }
            set { this.contents = value; }
        }
    }
}
