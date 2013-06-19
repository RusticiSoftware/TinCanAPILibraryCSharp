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
    public class NullableScormTimeSpan
    {
        public ScormTimeSpan Value;
        public NullableScormTimeSpan(ScormTimeSpan value)
        {
            this.Value = value;
        }
        public NullableScormTimeSpan(string value)
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
