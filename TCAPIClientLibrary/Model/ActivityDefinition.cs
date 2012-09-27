using System;
using System.Collections.Generic;
using System.Text;
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    /// <summary>
    /// Definition of an activity, a core statement piece.
    /// </summary>
    public class ActivityDefinition : Extensible
    {
        /// <summary>
        /// A collection of language codes and their activity name.
        /// </summary>
        private LanguageMap name;

        protected LanguageMap Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// A collection of language codes and their activity description.
        /// </summary>
        private LanguageMap description;

        protected LanguageMap Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// The type of activity
        /// </summary>
        private TinCanActivityType type;

        protected TinCanActivityType Type
        {
            get { return type; }
            set { type = value; }
        }

        private String interactionType;

        public virtual String InteractionType
        {
            get { return interactionType; }
            set { interactionType = value; }
        }

        public ActivityDefinition()
        {

            this.name = new LanguageMap();
            this.description = new LanguageMap();
        }

        /// <summary>
        /// Simplified constructor to create an activity with a
        /// single language code, name, and description.
        /// </summary>
        /// <param name="name">The Activity Name</param>
        /// <param name="description">The Activity Description</param>
        /// <param name="languageCode">The Activity language code</param>
        /// <param name="type">The Activity Type</param>
        /// <param name="interactionType">The Interaction Type</param>
        public ActivityDefinition(string name, string description,
            string languageCode, TinCanActivityType type,
            string interactionType) : this()
        {
            this.name.Add(languageCode, name);
            this.description.Add(languageCode, description);
            this.type = type;
            this.interactionType = interactionType;
        }

        public ActivityDefinition(LanguageMap name,
            LanguageMap description,
            TinCanActivityType type, string interactionType)
        {
            this.name = name;
            this.description = description;
            this.type = type;
            this.interactionType = interactionType;
        }

        public ActivityDefinition(ActivityDefinition activityDefinition)
        {
        }

        public virtual bool Update(ActivityDefinition def)
        {
            bool updated = false;
            if (def == null)
            {
                return false;
            }

            if (/*def.Type != null &&*/ !def.type.Equals(this.type))
            {
                this.Type = def.Type;
                updated = true;
            }
            if (def.Name != null && def.Name.Count > 0 && !CommonFunctions.AreDictionariesEqual(this.name, def.name))
            {
                this.name = def.Name;
                updated = true;
            }
            if (def.description != null && def.description.Count > 0 && !CommonFunctions.AreDictionariesEqual(this.description, def.description))
            {
                this.description = def.Description;
                updated = true;
            }
            if (def.InteractionType != null && !def.InteractionType.Equals(this.InteractionType))
            {
                this.InteractionType = def.InteractionType;
                updated = true;
            }

            return updated;
        }
    }
}
