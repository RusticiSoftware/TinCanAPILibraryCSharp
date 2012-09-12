using System;

namespace RusticiSoftware.TinCanAPILibrary.Exceptions
{
    class ConflictException : Exception
    {
        public ConflictException() : base() { }
        public ConflictException(string message) : base(message) { }
    }
}
