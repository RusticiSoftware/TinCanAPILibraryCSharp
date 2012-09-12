using System;

namespace RusticiSoftware.TinCanAPILibrary.Exceptions
{
    class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base() { }
        public UnauthorizedException(string message) : base(message) { }
    }
}
