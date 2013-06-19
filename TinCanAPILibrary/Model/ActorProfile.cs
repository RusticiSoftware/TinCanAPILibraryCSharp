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
    public class ActorProfile
    {
        private Actor actor;
        private string profileId;
        private string body;
        private string contentType;

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
