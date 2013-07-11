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
    public class LanguageMapConverter : IJsonTypeConverter
    {
        private Type myType = typeof(LanguageMap);

        #region JsonTypeConverter Members

        public Type GetTargetClass()
        {
            return myType;
        }

        public object Deserialize(string value, JsonConverter converter)
        {
            LanguageMap langDict = new LanguageMap();
            IDictionary objMap = converter.DeserializeJSONToMap(value);
            foreach (object key in objMap.Keys)
            {
                langDict.Add((string)key, (string)objMap[key]);
            }

            return langDict;
        }

        public object Reduce(object value, JsonConverter converter)
        {
            return (new Dictionary<string, string>((IDictionary<string, string>)value));
        }

        #endregion
    }
}
