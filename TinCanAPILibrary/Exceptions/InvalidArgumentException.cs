using System;

namespace RusticiSoftware.TinCanAPILibrary.Exceptions
{
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException() : base() { }
        public InvalidArgumentException(string message) : base(message) { }
    }
}
