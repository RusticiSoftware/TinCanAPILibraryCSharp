using System;
using RusticiSoftware.TinCanAPILibrary.Exceptions;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class StatementVerb
    {
        string id;
        LanguageMap display;

        /// <summary>
        /// Creates a new statement verb with the provided ID and Display fields
        /// </summary>
        /// <param name="id"></param>
        /// <param name="display"></param>
        public StatementVerb(Uri id, LanguageMap display)
        {
            this.id = id.ToString();
            this.display = display;
        }

        /// <summary>
        /// Creates a new statement verb with the provided ID and creates a language map with a single entry
        /// </summary>
        /// <param name="id"></param>
        /// <param name="locale"></param>
        /// <param name="name"></param>
        public StatementVerb(Uri id, string locale, string name)
        {
            this.id = id.ToString();
            display = new LanguageMap();
            display[locale] = name;
        }

        /// <summary>
        /// Creates a new statement verb given a URI String that is validated and a language map with a single entry
        /// </summary>
        /// <param name="id"></param>
        /// <param name="locale"></param>
        /// <param name="name"></param>
        public StatementVerb(string id, string locale, string name)
        {
            if (IsUri(id))
                this.id = id;
            else
                throw new ValidationException("The URI " + id + " is malformed.");
            display = new LanguageMap();
            display[locale] = name;
        }

        /// <summary>
        /// Creates a Statement Verb from the predefined list of verbs
        /// </summary>
        /// <param name="verb"></param>
        public StatementVerb(PredefinedVerbs verb)
        {
            this.id = "http://adlnet.gov/expapi/verbs/" + verb.ToString().ToLower();
            this.display = new LanguageMap();
            this.display["en-US"] = verb.ToString().ToLower();
        }

        private bool IsUri(string source) {
          if(!string.IsNullOrEmpty(source) && Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute)){
                Uri tempValue;
                return (Uri.TryCreate(source, UriKind.RelativeOrAbsolute, out tempValue));
            }
            return (false);
        }

        public bool IsVoided()
        {
            foreach (string s in display.Values)
            {
                if (s.ToLower().Equals("voided"))
                return true;
            }
            return false;
        }
    }

    public enum PredefinedVerbs
    {
        Experienced,
        Attended,
        Attempted,
        Completed,
        Passed,
        Failed,
        Answered,
        Interacted,
        Imported,
        Created,
        Shared,
        Voided,
    }
}
