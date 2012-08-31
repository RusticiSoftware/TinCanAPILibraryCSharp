using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Logic
{
    public class NullableDateTime
    {
        public DateTime Value;
        public NullableDateTime(DateTime Value)
        {
            this.Value = Value;
        }

        public override bool Equals(object obj)
        {
            NullableDateTime other = obj as NullableDateTime;
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
            return Value.GetHashCode();
        }

        public static implicit operator NullableDateTime(DateTime d)
        {
            return new NullableDateTime(d);
        }
    }
}
