using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace RusticiSoftware.TinCanAPILibrary
{
    public class ActivityDefinitionConverter : JsonTypeConverter
    {
        private Type myType = typeof(ActivityDefinition);
        public Type GetTargetClass()
        {
            return myType;
        }

        public object Deserialize(string value, JsonConverter converter)
        {
            //Integration.Implementation.LogAudit("TinCanActor Deserialize called", null);
            IDictionary objMap = converter.DeserializeJSONToMap(value);
            String typeField = null;
            if (objMap.Contains("type")) {
                typeField = (String)objMap["type"];
            }

            TinCanActivityType activityType = TinCanActivityTypeHelper.Parse(typeField);

            //Avoid infinite loop here, if type is this base class
            Type targetType = typeof(ActivityDefinition_JsonTarget);

            if (activityType == TinCanActivityType.CMI_Interaction){
                targetType = typeof(InteractionDefinition);
            }

            return converter.DeserializeJSON(value, targetType);
        }

        public object Reduce(object value, JsonConverter converter)
        {
            //Avoid infinite loop here, so we don't ever return just a TinCanActivityDefinition type
            return new ActivityDefinition_JsonTarget((ActivityDefinition)value);
        }

        // since TinCanActor is now a concrete class, 
        // provide this to reduce to so serialization doesn't get in an infinite loop
        public class ActivityDefinition_JsonTarget : ActivityDefinition
        {
            public ActivityDefinition_JsonTarget() { }
            public ActivityDefinition_JsonTarget(ActivityDefinition activityDef) : base(activityDef) { }
        }
    }
}
