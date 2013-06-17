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

namespace RusticiSoftware.TinCanAPILibrary.Helper
{
    public class Constants
    {
        /// <summary>
        /// ISO8601 date format, suitable as parameter to DateTime.ToString()
        /// </summary>
        public const string ISO8601_DATE_FORMAT = "yyyy-MM-ddTHH:mm:ss.fffZ";
    }

    /// <summary>
    /// Many data types that we use aren't allowed to be NULL in C#. For these data types, the constants defined
    /// in this class will indicate that the value is null. They are usually the minimum value permitted by the 
    /// data type...a very large negative number.
    /// </summary>
    public class NullConstants
    {
        /// <summary>
        /// A constant value that the application uses internally to represent an Int value that is NULL
        /// </summary>
        public const int NullInt = int.MinValue;
        /// <summary>
        /// A constant value that the application uses internally to represent a Long value that is NULL
        /// </summary>
        public const long NullLong = long.MinValue;
        /// <summary>
        /// A constant value that the application uses internally to represent a Double value that is NULL
        /// </summary>
        public const double NullDouble = double.MinValue;
        /// <summary>
        /// A constant value that the application uses internally to represent a DateTime value that is NULL
        /// </summary>
        public static DateTime NullDateTime = new DateTime(1900, 1, 1);
        /// <summary>
        /// A constant value that represents the position in this application's enumerations where the NULL or Undefined value resides.
        /// </summary>
        public const int UndefinedEnum = 0;
    }
}
