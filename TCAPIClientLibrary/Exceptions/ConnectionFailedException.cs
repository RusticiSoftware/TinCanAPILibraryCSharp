using System;

namespace RusticiSoftware.TinCanAPILibrary.Exceptions
{
    class ConnectionFailedException : Exception
    {
        public ConnectionFailedException() : base() { }
        public ConnectionFailedException(string message) : base(message) { }
    }
}
