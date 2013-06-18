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
using RusticiSoftware.TinCanAPILibrary.Exceptions;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class StatementVerb
    {
        private string id;
        private LanguageMap display;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public LanguageMap Display
        {
            get { return display; }
            set { display = value; }
        }

        /// <summary>
        /// Creates a new StatementVerb that is empty.  Used by the JSON Serializer.
        /// </summary>
        public StatementVerb()
        {
        }

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
        /// Creates a new statement verb given a URI string that is validated and a language map with a single entry
        /// </summary>
        /// <param name="id"></param>
        /// <param name="locale"></param>
        /// <param name="name"></param>
        public StatementVerb(string id, string locale, string name)
        {
            if (IsUri(id))
            {
                this.id = id;
            }
            else
            {
                throw new ValidationException("The URI " + id + " is malformed.");
            }
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

        /// <summary>
        /// Creates a statement verb from the 0.90 set of verbs.
        /// </summary>
        /// <param name="verb"></param>
        /// <remarks>You really shouldn't be using this method.  It's simply used as an easy way to promote the
        /// verb enum to the verb class.</remarks>
        public StatementVerb(Model.TinCan090.StatementVerb verb)
            : this((PredefinedVerbs)Enum.Parse(typeof(PredefinedVerbs), verb.ToString(), true))
        {
        }

        private bool IsUri(string source)
        {
            if (!string.IsNullOrEmpty(source) && Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute))
            {
                Uri tempValue;
                return Uri.TryCreate(source, UriKind.RelativeOrAbsolute, out tempValue);
            }
            return false;
        }

        public bool IsVoided()
        {
            foreach (string s in display.Values)
            {
                if (s.ToLower().Equals("voided"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Demotes a 0.95 verb to a 0.90 verb.
        /// </summary>
        /// <param name="verb">A 0.95 verb.  It MUST have an en-US entry in the display field.</param>
        /// <returns></returns>
        /// <remarks>If no en-US entry is in the display map, this method will always fail and throw an exception.
        /// The core verbs from 0.90 are adl provided verbs in 0.95 to maintain some form of verb mapping.</remarks>
        public static explicit operator Model.TinCan090.StatementVerb(StatementVerb verb)
        {
            try
            {
                return (Model.TinCan090.StatementVerb)Enum.Parse(typeof(Model.TinCan090.StatementVerb), verb.display["en-US"], true);
            }
            catch (ArgumentException)
            {
                throw new InvalidArgumentException("The verb " + verb.display["en-US"] + " has no 0.90 verb representation.");
            }
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
