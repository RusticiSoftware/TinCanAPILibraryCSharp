using System;
using System.Collections.Generic;
using System.Text;
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace RusticiSoftware.TinCanAPILibrary.TypeConverters
{
    public class NullableIntegerJsonConverter : JsonTypeConverter
    {
        private Type myType = typeof(NullableInteger);
        public Type GetTargetClass()
        {
            return myType;
        }

        public object Deserialize(string value, JsonConverter converter)
        {
            return String.IsNullOrEmpty(value) ? null : new NullableInteger(Int32.Parse(value));
        }

        public object Reduce(object value, JsonConverter converter)
        {
            return ((NullableInteger)value).Value;
        }
    }
}
