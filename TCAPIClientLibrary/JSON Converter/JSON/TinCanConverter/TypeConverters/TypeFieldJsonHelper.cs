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
                return typeof(Model.TinCan090.Person);
            }
            if (String.Compare(typeField, "Group", true) == 0) {
                return typeof(Group);
            }
            if (String.Compare(typeField, "Statement", true) == 0) {
                return typeof(Model.TinCan090.TargetedStatement);
            }
            if (String.Compare(typeField, "StatementRef", true) == 0)
            {
                return typeof(StatementRef);
            }
            throw new ArgumentException("Invalid type field specified");
        }
    }
}
