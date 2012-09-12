using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class NullableScormTimeSpan
    {
        public ScormTimeSpan Value;
        public NullableScormTimeSpan(ScormTimeSpan value)
        {
            this.Value = value;
        }
        public NullableScormTimeSpan(String value)
        {
            this.Value = new ScormTimeSpan(value);
        }

        public override bool Equals(object obj)
        {
            NullableScormTimeSpan other = obj as NullableScormTimeSpan;
            if (other == null)
            {
                return false;
            }
            else
            {
                return Equals(other.Value.Value, this.Value.Value);
            }
        }
        public override int GetHashCode()
        {
            return (int)(Value.Value & int.MaxValue);
        }
    }
}
