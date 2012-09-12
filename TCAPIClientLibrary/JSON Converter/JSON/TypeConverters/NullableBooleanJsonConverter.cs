using System;
using System.Collections.Generic;
using System.Text;
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace RusticiSoftware.TinCanAPILibrary.TypeConverters
{
    public class NullableBooleanJsonConverter : JsonTypeConverter
    {
        private Type myType = typeof(NullableBoolean);
        public Type GetTargetClass()
        {
            return myType;
        }

        public object Deserialize(string value, JsonConverter converter)
        {
            return String.IsNullOrEmpty(value) ? null : new NullableBoolean(Boolean.Parse(value));
        }

        public object Reduce(object value, JsonConverter converter)
        {
            return ((NullableBoolean)value).Value;
        }
    }
}
