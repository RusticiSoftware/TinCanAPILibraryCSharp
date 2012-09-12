using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Json
{
    public interface JsonTypeConverter
    {
        Type GetTargetClass();
        object Deserialize(string value, JsonConverter converter);
        //Instead of serializing directly, just reduce to something serializable
        object Reduce(object value, JsonConverter converter); 
    }
}
