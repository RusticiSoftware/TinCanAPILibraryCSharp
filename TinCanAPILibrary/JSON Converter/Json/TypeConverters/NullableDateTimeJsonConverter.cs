using System;
using System.Collections.Generic;
using System.Text;
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.Logic;
using System.Globalization;
using RusticiSoftware.TinCanAPILibrary.Helper;
using RusticiSoftware.ScormContentPlayer.Util;

namespace RusticiSoftware.TinCanAPILibrary.TypeConverters
{
    public class NullableDateTimeJsonConverter : JsonTypeConverter
    {
        private Type myType = typeof(NullableDateTime);
        public Type GetTargetClass()
        {
            return myType;
        }

        public object Deserialize(string value, JsonConverter converter)
        {
            return String.IsNullOrEmpty(value) ? null : new NullableDateTime(ParserUtil.GetDateTimeFromIsoTimeString(value));
        }

        public object Reduce(object value, JsonConverter converter)
        {
            return ((NullableDateTime)value).Value.ToString(Constants.ISO8601_DATE_FORMAT);
        }
    }
}
