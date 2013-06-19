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
