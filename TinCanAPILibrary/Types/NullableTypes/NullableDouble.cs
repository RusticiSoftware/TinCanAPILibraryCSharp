#region License
/*
Copyright 2012 Rustici Software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion
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
            return (int)(Value % int.MaxValue);
        }

        public static implicit operator NullableDouble(double d)
        {
            return new NullableDouble(d);
        }
    }
}
