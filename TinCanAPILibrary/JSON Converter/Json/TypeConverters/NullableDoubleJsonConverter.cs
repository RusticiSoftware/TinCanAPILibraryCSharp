using System;
using System.Collections.Generic;
using System.Text;
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace RusticiSoftware.TinCanAPILibrary.TypeConverters
{
    public class NullableDoubleJsonConverter : JsonTypeConverter
    {
        private Type myType = typeof(NullableDouble);
        public Type GetTargetClass()
        {
            return myType;
        }

        public object Deserialize(string value, JsonConverter converter)
        {
            return String.IsNullOrEmpty(value) ? null : new NullableDouble(Double.Parse(value));
        }

        public object Reduce(object value, JsonConverter converter)
        {
            return ((NullableDouble)value).Value;
        }
    }
}
