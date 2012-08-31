using System.Text.RegularExpressions;
using System;

namespace RusticiSoftware.TinCanAPILibrary.Helper
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates an email address
        /// </summary>
        /// <param name="email">The email to validate</param>
        /// <returns>True if valid, otherwise false</returns>
        public static bool IsValidEmailAddress(String email)
        {
            return Regex.IsMatch(email, "^([a-zA-Z0-9_\\.\\-\\+])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{2,4})+$");
        }

        /// <summary>
        /// Determines if a string is a valid UUID.
        /// </summary>
        /// <param name="potential">The string to test</param>
        /// <returns>True if valid, otherwise false</returns>
        public static bool IsValidUUID(String potential)
        {
            if (potential == null)
            {
                return false;
            }
            return Regex.IsMatch(potential.ToLower(), "^[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}$");
        }
    }
}
