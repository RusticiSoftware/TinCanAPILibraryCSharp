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
        public Type GetTypeFromString(String typeField, Type defaultType)
        {
            if (String.IsNullOrEmpty(typeField)) {
                return defaultType;
            }
            if (String.Compare(typeField, "Activity", true) == 0){
                return typeof(TinCanActivity);
            }
            if (String.Compare(typeField, "Agent", true) == 0) {
                return typeof(Actor);
            }
            if (String.Compare(typeField, "Person", true) == 0) {
                return typeof(Person);
            }
            if (String.Compare(typeField, "Group", true) == 0) {
                return typeof(Group);
            }
            if (String.Compare(typeField, "Statement", true) == 0) {
                return typeof(TargetedStatement);
            }
            throw new ArgumentException("Invalid type field specified");
        }
    }
}
