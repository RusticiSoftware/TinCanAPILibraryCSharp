using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;
using RusticiSoftware.TinCanAPILibrary.Json;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace RusticiSoftware.TinCanAPILibrary
{
    public class TinCanJsonConverter : JsonConverterBase
    {
        private static JsonConverter instance = null;

        public static JsonConverter Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (typeof(TinCanJsonConverter))
                    {
                        if (instance == null)
                        {
                            instance = new TinCanJsonConverter();
                        }
                    }
                }
                return instance;
            }
        }

        public TinCanJsonConverter() : base()
        {
            //on the Java side we need to register default converters for concrete subclasses of 
            //interfaces and abstract classes registered with a custom converter

            this.RegisterTypeConverter(typeof(TinCanAPILibrary.Model.StatementTarget), new TinCanStatementTargetJsonConverter());
            this.RegisterDefaultConverter(typeof(TinCanActivity));
            this.RegisterDefaultConverter(typeof(TargetedStatement));

            this.RegisterTypeConverter(typeof(Actor), new TinCanActorJsonConverter());
            this.RegisterDefaultConverter(typeof(TinCanActorJsonConverter.TinCanActor_JsonTarget));
            this.RegisterDefaultConverter(typeof(Person));
            this.RegisterDefaultConverter(typeof(Group));

            this.RegisterTypeConverter(typeof(LanguageMap), new LanguageMapConverter());

            this.RegisterTypeConverter(typeof(ActivityDefinition), new ActivityDefinitionConverter());
            this.RegisterDefaultConverter(typeof(ActivityDefinitionConverter.ActivityDefinition_JsonTarget));
            this.RegisterDefaultConverter(typeof(InteractionDefinition));
            
        }
    }
}
