using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace RusticiSoftware.TinCanAPILibrary
{
    public class LanguageMapConverter : JsonTypeConverter
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
