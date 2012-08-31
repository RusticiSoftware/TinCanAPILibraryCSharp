using System;

namespace RusticiSoftware.TinCanAPILibrary.Exceptions
{
    class NotFoundException : Exception
    {
        public NotFoundException() : base() { }
        public NotFoundException(string message) : base(message) { }
    }
}
