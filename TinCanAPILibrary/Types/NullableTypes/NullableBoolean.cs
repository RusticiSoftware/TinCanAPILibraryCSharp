using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class NullableBoolean
    {
        public bool Value;
        public NullableBoolean(bool Value)
        {
            this.Value = Value;
        }

        public override bool Equals(object obj)
        {
            NullableBoolean other = obj as NullableBoolean;
            if (other == null)
            {
                return false;
            }
            else
            {
                return Equals(other.Value, this.Value);
            }
        }
        public override int GetHashCode()
        {
            return Value ? 1 : 0;
        }

        public static implicit operator NullableBoolean(bool b)
        {
            return new NullableBoolean(b);
        }
    }
}
