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
