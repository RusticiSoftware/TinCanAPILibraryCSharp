using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class NullableDouble
    {
        public double Value;
        public NullableDouble(double Value)
        {
            this.Value = Value;
        }

        public override bool Equals(object obj)
        {
            NullableDouble dbl = obj as NullableDouble;
            if (dbl == null)
            {
                return false;
            }
            else
            {
                return Equals(dbl.Value, this.Value);
            }
        }
        public override int GetHashCode()
        {
            return (int) (Value % int.MaxValue);
        }

        public static implicit operator NullableDouble(double d)
        {
            return new NullableDouble(d);
        }
    }
}
