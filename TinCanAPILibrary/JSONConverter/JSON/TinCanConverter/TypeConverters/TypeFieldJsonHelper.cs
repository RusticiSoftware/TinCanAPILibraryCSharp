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
using RusticiSoftware.TinCanAPILibrary.Model;

namespace RusticiSoftware.TinCanAPILibrary
{
    public class TypeFieldJsonHelper
    {
        public Type GetTypeFromString(string typeField, Type defaultType)
        {
            if (string.IsNullOrEmpty(typeField))
            {
                return defaultType;
            }
            if (string.Compare(typeField, "Activity", true) == 0)
            {
                return typeof(TinCanActivity);
            }
            if (string.Compare(typeField, "Agent", true) == 0)
            {
                return typeof(Actor);
            }
            if (string.Compare(typeField, "Person", true) == 0)
            {
                return typeof(Model.TinCan090.Person);
            }
            if (string.Compare(typeField, "Group", true) == 0)
            {
                return typeof(Group);
            }
            if (string.Compare(typeField, "Statement", true) == 0)
            {
                return typeof(Model.TinCan090.TargetedStatement);
            }
            if (string.Compare(typeField, "StatementRef", true) == 0)
            {
                return typeof(StatementRef);
            }
            throw new ArgumentException("Invalid type field specified");
        }
    }
}
