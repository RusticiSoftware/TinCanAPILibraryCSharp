using System;
using System.Globalization;

namespace RusticiSoftware.ScormContentPlayer.Util
{
    /// <summary>
    /// Helper class for parsing various data formats from string formats
    /// </summary>
    public class ParserUtil
    {
        /// <summary>
        /// Takes a string that represents an ISO date and tries to parse it.
        /// </summary>
        /// <param name="isoTimeValue">String representing the date</param>
        /// <returns>A populated DateTime object</returns>
        /// <exception cref="System.FormatException">If the string does not represent a valid date.</exception>
        public static DateTime GetDateTimeFromIsoTimeString(string isoTimeValue)
        {

            DateTimeFormatInfo currentCultureFormat = CultureInfo.InvariantCulture.DateTimeFormat;

            //note, help on formatting is under DateTimeFormatInfoClass

            String[] iso8601Formats = new String[14];
            iso8601Formats[0] = "yyyy-MM-ddTHH:mm:ss.FFFFFFF'Z'";
            iso8601Formats[1] = "yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz";
            iso8601Formats[2] = "yyyy-MM-ddTHH:mm:ss.FFFFFFFzz";
            iso8601Formats[3] = "yyyy-MM-ddTHH:mm:ss.FFFFFFF";
            iso8601Formats[4] = "yyyy-MM-ddTHH:mm:ssZ";
            iso8601Formats[5] = "yyyy-MM-ddTHH:mm:ss";
            iso8601Formats[6] = "yyyy-MM-ddTHH:mm";
            iso8601Formats[7] = "yyyy-MM-ddTHH";
            iso8601Formats[8] = "yyyy-MM-dd";
            iso8601Formats[9] = "yyyy-MM";
            iso8601Formats[10] = "yyyy";

            try
            {
                DateTime parsed = DateTime.ParseExact(isoTimeValue, iso8601Formats, currentCultureFormat, DateTimeStyles.AdjustToUniversal);
                if (isoTimeValue.EndsWith("Z"))
                {
                    parsed = DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
                }
                return parsed;
            }
            catch (FormatException fe)
            {
                throw new FormatException("'" + isoTimeValue + "' is an invalid String representation of a DateTime value.", fe);
            }

        }

        /// <summary>
        /// Parses a date that must be in the UniversalSortableDateTimePattern format
        /// </summary>
        /// <param name="s">String representing the date to parse</param>
        /// <returns>DateTime object representing the date</returns>
        public static DateTime ParseExact(string s)
        {
            try
            {
                return DateTime.ParseExact(s, DateTimeFormatInfo.InvariantInfo.UniversalSortableDateTimePattern, CultureInfo.InvariantCulture);
            }
            catch (FormatException fe)
            {
                throw new FormatException("'" + s + "' is an invalid String representation of a DateTime value.", fe);
            }
        }

        /// <summary>
        /// Determines if a string represents a valid Double data type
        /// </summary>
        /// <param name="s">String to check</param>
        /// <returns>Boolean indicating validity</returns>
        public static bool IsValidDouble(string s)
        {
            double d;
            return Double.TryParse(s, NumberStyles.Any, new NumberFormatInfo(), out d);
        }

        /// <summary>
        /// Determines if a string represents a valid Doubld data type, allowing for specific multiple number formats
        /// </summary>
        /// <param name="s">String to check</param>
        /// <param name="style"></param>
        /// <param name="info"></param>
        /// <returns>Boolean indicating validity</returns>
        public static bool IsValidDouble(string s, NumberStyles style, NumberFormatInfo info)
        {
            double d;
            return Double.TryParse(s, style, info, out d);
        }

        /// <summary>
        /// Converts a string into a Double data type
        /// </summary>
        /// <param name="s">String to convert</param>
        /// <returns>Converted double data type</returns>
        public static double ParseDouble(string s)
        {
            return Double.Parse(s, NumberStyles.Any, new NumberFormatInfo());
        }
    }
}
