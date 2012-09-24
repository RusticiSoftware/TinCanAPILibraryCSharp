using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class ActivityState : State
    {
        string activityId;
        Actor actor;
        string registrationId;
        string body;
        string contentType;
        string stateId;

        public const string DEFAULT_HEADER = "text/plain";

        public string StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }

        public string Body
        {
            get { return this.body; }
            set { this.body = value; }
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

        public string RegistrationId
        {
            get { return registrationId; }
            set { registrationId = value; }
        }

        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        public ActivityState()
        {}

        public ActivityState(string activityId, string stateId, Actor actor, string body)
            : this(activityId, stateId, actor, body, DEFAULT_HEADER)
        {}

        public ActivityState(string activityId, string stateId, Actor actor, string body, string contentType)
            : this(activityId, stateId, actor, body, contentType, null)
        {
        }

        public ActivityState(string activityId, string stateId, Actor actor, string body, string contentType, string regristrationId)
        {
            this.activityId = activityId;
            this.stateId = stateId;
            this.actor = actor;
            this.body = body;
            this.contentType = contentType;
            this.registrationId = registrationId;
        }
    }
}
