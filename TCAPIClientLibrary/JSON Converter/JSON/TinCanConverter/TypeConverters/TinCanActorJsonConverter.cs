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
using System.Collections;
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace RusticiSoftware.TinCanAPILibrary
{
    public class TinCanActorJsonConverter : JsonTypeConverter
    {
        private Type myType = typeof(Actor);
        public Type GetTargetClass()
        {
            return myType;
        }

        public object Deserialize(string value, JsonConverter converter)
        {
            //Integration.Implementation.LogAudit("TinCanActor Deserialize called", null);
            IDictionary objMap = converter.DeserializeJSONToMap(value);
            String typeField = null;
            if (objMap.Contains("objectType")) {
                typeField = (String)objMap["objectType"];
            }

            TypeFieldJsonHelper typeFieldHelper = new TypeFieldJsonHelper();
            Type targetType = typeFieldHelper.GetTypeFromString(typeField, typeof(Person));

            //Avoid infinite loop here, if type is this base class
            if (targetType.Equals(typeof(Actor))) {
                targetType = typeof(TinCanActor_JsonTarget);
            }

            return converter.DeserializeJSON(value, targetType);
        }

        public object Reduce(object value, JsonConverter converter)
        {
            //Avoid infinite loop here, so we don't ever return just a TinCanActor type
            return new TinCanActor_JsonTarget((Actor) value);
        }

        // since TinCanActor is now a concrete class, 
        // provide this to reduce to so serialization doesn't get in an infinite loop
        public class TinCanActor_JsonTarget : Actor
        {
            public TinCanActor_JsonTarget(){}
            public TinCanActor_JsonTarget(Actor actor) : base(actor) {}
        }
    }
}
