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

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    /// <summary>
    /// Definition of any extensible TinCanAPI Statement.
    /// Allows for arbitrary key/value pairs to be attached to
    /// an object.
    /// </summary>
    public class Extensible
    {
        protected Dictionary<Uri, object> extensions;
        
        public Dictionary<Uri, object> Extensions
        {
            get { return extensions; }
            set { extensions = value; }
        }

        public Extensible() { }
        public Extensible(Extensible extensible)
        {
            this.extensions = extensible.extensions;
        }
    }
}
