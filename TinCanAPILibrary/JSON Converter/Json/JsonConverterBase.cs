using System;
using System.Collections.Generic;
using System.Text;
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.TypeConverters;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace RusticiSoftware.TinCanAPILibrary
{
    public class JsonConverterBase : JsonConverter
    {
        public JsonConverterBase() : base()
        {
            this.RegisterTypeConverter(typeof(NullableBoolean), new NullableBooleanJsonConverter());
            this.RegisterTypeConverter(typeof(NullableInteger), new NullableIntegerJsonConverter());
            this.RegisterTypeConverter(typeof(NullableDouble), new NullableDoubleJsonConverter());
            this.RegisterTypeConverter(typeof(NullableDateTime), new NullableDateTimeJsonConverter());
            this.RegisterTypeConverter(typeof(NullableScormTimeSpan), new NullableScormTimeSpanJsonConverter());
        }
    }
}
