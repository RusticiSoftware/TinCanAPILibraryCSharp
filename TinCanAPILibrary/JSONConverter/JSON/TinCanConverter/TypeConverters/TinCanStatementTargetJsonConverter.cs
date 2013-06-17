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
    public class TinCanStatementTargetJsonConverter : JsonTypeConverter
    {
        private Type myType = typeof(TinCanAPILibrary.Model.StatementTarget);
        public Type GetTargetClass()
        {
            return myType;
        }

        public object Deserialize(string value, JsonConverter converter)
        {
            //Integration.Implementation.LogAudit("TinCanStatementTarget Deserialize called", null);
            IDictionary objMap = converter.DeserializeJSONToMap(value);
            String typeField = null;
            if (objMap.Contains("objectType")) {
                typeField = (String)objMap["objectType"];
            }

            TypeFieldJsonHelper typeFieldHelper = new TypeFieldJsonHelper();
            Type targetType = typeFieldHelper.GetTypeFromString(typeField, typeof(TinCanActivity));
            return converter.DeserializeJSON(value, targetType);
        }

        public object Reduce(object value, JsonConverter converter)
        {
            //This should never be called, since TinCanStatementTarget is just an interface, and true should be known for serialization
            throw new Exception("Reduce called on TinCanStatement converter");

            /*Integration.Implementation.LogAudit("TinCanStatementTarget Reduce called", null);

            Type targetType = value.GetType();
            if (targetType.IsAssignableFrom(typeof(TinCanActivity))){
                return (TinCanActivity)value;
            }
            else if (targetType.IsAssignableFrom(typeof(TinCanActor))){
                return (TinCanActor)value;
            }
            throw new Exception("Statement target type " + value.GetType().ToString() + "  was unrecognized during serialization");*/
        }
    }
}
