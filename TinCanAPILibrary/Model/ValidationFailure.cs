using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class ValidationFailure
    {
        public string Error { get; private set; }

        public ValidationFailure(string error)
        {
            this.Error = error;
        }
    }
}
