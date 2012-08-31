using System;

namespace RusticiSoftware.TinCanAPILibrary.Exceptions
{
    /// <summary>
    /// Exception Class for validation errors.
    /// </summary>
    class ValidationException : Exception
    {
        /// <summary>
        /// Throws a ValidationException.
        /// </summary>
        public ValidationException() : base() { }
        /// <summary>
        /// Throws a ValidationException with the provided message.
        /// </summary>
        /// <param name="message">Message explaining the exception</param>
        public ValidationException(String message) : base(message) { }
    }
}
