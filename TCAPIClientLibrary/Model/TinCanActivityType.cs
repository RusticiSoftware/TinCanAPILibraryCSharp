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

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    /// <summary>
    /// Enum of the ActivityTypes supported by the TinCanAPI
    /// </summary>
    public enum TinCanActivityType
    {
        Undefined,
        Course,
        Module,
        Meeting,
        Media,
        Performance,
        Simulation,
        Assessment,
        Interaction,
        CMI_Interaction,
        Question,
        Objective,
        Link,
    }

    public class TinCanActivityTypeHelper
    {
        public static String StringValue(TinCanActivityType activityType)
        {
            if (activityType == TinCanActivityType.Undefined)
            {
                return null;
            }
            String val = activityType.ToString().ToLower();
            if (val == "cmi_interaction")
            {
                val = "cmi.interaction";
            }
            return val;
        }

        public static TinCanActivityType Parse(String activityTypeStr)
        {
            if (activityTypeStr == null)
            {
                return TinCanActivityType.Undefined;
            }
            activityTypeStr = activityTypeStr.ToLower();
            if (activityTypeStr == "cmi.interaction")
            {
                activityTypeStr = "cmi_interaction";
            }
            return (TinCanActivityType)Enum.Parse(typeof(TinCanActivityType), activityTypeStr, true);
        }
    }
}