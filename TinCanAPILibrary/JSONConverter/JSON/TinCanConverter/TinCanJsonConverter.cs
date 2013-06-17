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
            this.RegisterDefaultConverter(typeof(StatementRef));

            this.RegisterTypeConverter(typeof(Actor), new TinCanActorJsonConverter());
            this.RegisterDefaultConverter(typeof(TinCanActorJsonConverter.TinCanActor_JsonTarget));
            this.RegisterDefaultConverter(typeof(Actor));
            this.RegisterDefaultConverter(typeof(Group));

            this.RegisterTypeConverter(typeof(LanguageMap), new LanguageMapConverter());

            this.RegisterTypeConverter(typeof(ActivityDefinition), new ActivityDefinitionConverter());
            this.RegisterDefaultConverter(typeof(ActivityDefinitionConverter.ActivityDefinition_JsonTarget));
            this.RegisterDefaultConverter(typeof(InteractionDefinition));

            // TinCan 90
            this.RegisterDefaultConverter(typeof(Model.TinCan090.Actor));
            this.RegisterDefaultConverter(typeof(Model.TinCan090.Person));
            this.RegisterDefaultConverter(typeof(Model.TinCan090.TargetedStatement));
        }
    }
}
