using System;
using System.Collections.Generic;
using System.Text;
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.Logic;

namespace RusticiSoftware.TinCanAPILibrary.TypeConverters
{
    public class NullableScormTimeSpanJsonConverter : JsonTypeConverter
    {
        private Type myType = typeof(NullableScormTimeSpan);
        public Type GetTargetClass()
        {
            return myType;
        }

        public object Deserialize(string value, JsonConverter converter)
        {
            return String.IsNullOrEmpty(value) ? null : new NullableScormTimeSpan(value);
        }

        public object Reduce(object value, JsonConverter converter)
        {
            return ((NullableScormTimeSpan)value).Value.ToIso8601String();
        }
    }
}
