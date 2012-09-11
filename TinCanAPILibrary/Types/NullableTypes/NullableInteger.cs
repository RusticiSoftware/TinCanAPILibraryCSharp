using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class NullableInteger
    {
        public int Value;
        public NullableInteger(int Value)
        {
            this.Value = Value;
        }

        public static implicit operator NullableInteger(int i)
        {
            return new NullableInteger(i);
        }
    }
}
