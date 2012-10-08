using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace RusticiSoftware.TinCanAPILibrary
{
    public class TinCanActorJsonConverter : JsonTypeConverter
    {
        private Type myType = typeof(Actor);
        public Type GetTargetClass()
        {
            return myType;
        }

        public object Deserialize(string value, JsonConverter converter)
        {
            //Integration.Implementation.LogAudit("TinCanActor Deserialize called", null);
            IDictionary objMap = converter.DeserializeJSONToMap(value);
            String typeField = null;
            if (objMap.Contains("objectType")) {
                typeField = (String)objMap["objectType"];
            }

            TypeFieldJsonHelper typeFieldHelper = new TypeFieldJsonHelper();
            Type targetType = typeFieldHelper.GetTypeFromString(typeField, typeof(Actor));

            //Avoid infinite loop here, if type is this base class
            if (targetType.Equals(typeof(Actor))) {
                targetType = typeof(TinCanActor_JsonTarget);
            }

            return converter.DeserializeJSON(value, targetType);
        }

        public object Reduce(object value, JsonConverter converter)
        {
            //Avoid infinite loop here, so we don't ever return just a TinCanActor type
            return new TinCanActor_JsonTarget((Actor) value);
        }

        // since TinCanActor is now a concrete class, 
        // provide this to reduce to so serialization doesn't get in an infinite loop
        public class TinCanActor_JsonTarget : Actor
        {
            public TinCanActor_JsonTarget(){}
            public TinCanActor_JsonTarget(Actor actor) : base(actor) {}
        }

        public class TinCan090Actor_JsonTarget : Model.TinCan090.Actor
        {
            public TinCan090Actor_JsonTarget(){}
            public TinCan090Actor_JsonTarget(Model.TinCan090.Actor actor) : base(actor){}
        }
    }
}
