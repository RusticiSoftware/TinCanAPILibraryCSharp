#region License
/*
Copyright 2012 Rustici Software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class ActivityState : State
    {
        private string activityId;
        private Actor actor;
        private string registrationId;
        private string body;
        private string contentType;
        private string stateId;

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
        { }

        public ActivityState(string activityId, string stateId, Actor actor, string body)
            : this(activityId, stateId, actor, body, DEFAULT_HEADER)
        { }

        public ActivityState(string activityId, string stateId, Actor actor, string body, string contentType)
            : this(activityId, stateId, actor, body, contentType, null)
        {
        }

        public ActivityState(string activityId, string stateId, Actor actor, string body, string contentType, string registrationId)
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
