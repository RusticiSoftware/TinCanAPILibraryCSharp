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
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.Model;

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
