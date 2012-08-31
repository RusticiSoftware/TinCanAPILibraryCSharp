using System;

namespace RusticiSoftware.TinCanAPILibrary.Logic
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