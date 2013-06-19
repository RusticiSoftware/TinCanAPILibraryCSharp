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
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class InteractionDefinition : ActivityDefinition
    {
        private List<string> correctResponsesPattern;
        private InteractionType interactionType = new InteractionType(InteractionTypeValue.Undefined);

        //Each of these is optional, depending on the type of interaction
        private List<InteractionComponent> choices;
        private List<InteractionComponent> scale;
        private List<InteractionComponent> source;
        private List<InteractionComponent> target;
        private List<InteractionComponent> steps;

        public InteractionDefinition() { }

        public InteractionDefinition(ActivityDefinition def)
        {
            this.Update(def);
        }

        public override bool Update(ActivityDefinition activityDef)
        {
            bool updated = base.Update(activityDef);
            if (!(activityDef is InteractionDefinition))
            {
                return updated;
            }

            InteractionDefinition def = (InteractionDefinition)activityDef;

            if (NotNullAndNotEqual(def.CorrectResponsesPattern, this.CorrectResponsesPattern))
            {
                this.CorrectResponsesPattern = def.CorrectResponsesPattern;
                updated = true;
            }

            if (NotNullAndNotEqual(def.Choices, this.Choices))
            {
                this.Choices = def.Choices;
                updated = true;
            }

            if (NotNullAndNotEqual(def.Scale, this.Scale))
            {
                this.Scale = def.Scale;
                updated = true;
            }

            if (NotNullAndNotEqual(def.Source, this.Source))
            {
                this.Source = def.Source;
                updated = true;
            }

            if (NotNullAndNotEqual(def.Target, this.Target))
            {
                this.Target = def.Target;
                updated = true;
            }

            if (NotNullAndNotEqual(def.Steps, this.Steps))
            {
                this.Steps = def.Steps;
                updated = true;
            }

            return updated;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        protected bool NotNullAndNotEqual<T>(List<T> list1, List<T> list2)
        {
            return list1 != null && list1.Count > 0 && !CommonFunctions.AreListsEqual(list1, list2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is InteractionDefinition))
            {
                return false;
            }
            InteractionDefinition otherDef = (InteractionDefinition)obj;
            return base.Equals(obj)
                        && CommonFunctions.AreListsEqual(correctResponsesPattern, otherDef.correctResponsesPattern)
                        && CommonFunctions.AreListsEqual(this.Choices, otherDef.Choices)
                        && CommonFunctions.AreListsEqual(this.Scale, otherDef.Scale)
                        && CommonFunctions.AreListsEqual(this.Source, otherDef.Source)
                        && CommonFunctions.AreListsEqual(this.Target, otherDef.Target)
                        && CommonFunctions.AreListsEqual(this.Steps, otherDef.Steps);
        }

        public static bool IsValidComponent(InteractionTypeValue interactionType, InteractionComponentName componentName)
        {
            if (interactionType == InteractionTypeValue.Choice || interactionType == InteractionTypeValue.Sequencing)
            {
                return componentName == InteractionComponentName.Choices;
            }
            else if (interactionType == InteractionTypeValue.Likert)
            {
                return componentName == InteractionComponentName.Scale;
            }
            else if (interactionType == InteractionTypeValue.Matching)
            {
                return componentName == InteractionComponentName.Source || componentName == InteractionComponentName.Target;
            }
            else if (interactionType == InteractionTypeValue.Performance)
            {
                return componentName == InteractionComponentName.Steps;
            }
            return false;
        }

        protected List<InteractionComponent> ProtectedGet(InteractionComponentName componentName, List<InteractionComponent> componentList)
        {
            if (!IsValidComponent(this.interactionType.Value, componentName))
            {
                return null;
            }
            return componentList;
        }

        protected void CheckComponentSet(InteractionComponentName componentName, List<InteractionComponent> componentList)
        {
            if (componentList == null)
            {
                return;
            }
            if (!IsValidComponent(this.interactionType.Value, componentName))
            {
                throw new ArgumentException(componentName.ToString().ToLower() + " is not a valid interaction component for the given interactionType", "componentName");
            }
        }

        #region Properties

        public override string InteractionType
        {
            get { return (interactionType.Value == InteractionTypeValue.Undefined) ? null : interactionType.ToString(); }
            set { this.interactionType = new InteractionType(value.ToLower()); }
        }

        public List<string> CorrectResponsesPattern
        {
            get { return correctResponsesPattern; }
            set { correctResponsesPattern = value; }
        }

        public List<InteractionComponent> Choices
        {
            get { return ProtectedGet(InteractionComponentName.Choices, choices); }
            set
            {
                CheckComponentSet(InteractionComponentName.Choices, value);
                choices = value;
            }
        }
        public List<InteractionComponent> Scale
        {
            get { return ProtectedGet(InteractionComponentName.Scale, scale); }
            set
            {
                CheckComponentSet(InteractionComponentName.Scale, value);
                scale = value;
            }
        }
        public List<InteractionComponent> Source
        {
            get { return ProtectedGet(InteractionComponentName.Source, source); }
            set
            {
                CheckComponentSet(InteractionComponentName.Source, value);
                source = value;
            }
        }
        public List<InteractionComponent> Target
        {
            get { return ProtectedGet(InteractionComponentName.Target, target); }
            set
            {
                CheckComponentSet(InteractionComponentName.Target, value);
                target = value;
            }
        }
        public List<InteractionComponent> Steps
        {
            get { return ProtectedGet(InteractionComponentName.Steps, steps); }
            set
            {
                CheckComponentSet(InteractionComponentName.Steps, value);
                steps = value;
            }
        }

        #endregion
    }
}
