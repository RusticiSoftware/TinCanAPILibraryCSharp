using System;

namespace RusticiSoftware.TinCanAPILibrary.Exceptions
{
    class PreconditionFailedException : Exception
    {
        public PreconditionFailedException() : base() { }
        public PreconditionFailedException(string message) : base(message) { }
    }
}
