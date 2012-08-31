using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Logic
{
    public class LanguageMap : Dictionary<string, string>
    {
        public const string Undefined_Language_Code = "und";

        public LanguageMap() { }

        /// <summary>
        /// Select the best language, based on ordered array of languages
        /// </summary>
        /// <param name="orderedLangs">language preferences, as in HTTP accept-lananguage</param>
        /// <param name="fallbackToAny">if no match is found according to language preference string, should the first item in the list (if any) be reutrned?</param>
        /// <returns></returns>
        public LanguageString GetBestLanguageMatch(string[] orderedLangs, bool fallbackToAny)
        {
            if (this.Count == 0)
            {
                return new LanguageString();
            }

            foreach (string indexLang in orderedLangs)
            {
                string lang = indexLang.ToLower().Replace("_", "-");
                foreach (string key in this.Keys)
                {
                    if (key.ToLower().Equals(lang) || lang.Equals("*") ||
                        (key.IndexOf('-') >= 0 && key.Substring(0, key.IndexOf('-') - 1).ToLower().Equals(lang)))
                    {
                        return new LanguageString(this[key], key);
                    }
                }
            }

            if (this.Count > 0 && fallbackToAny)
            {
                foreach (string key in this.Keys)
                {
                    return new LanguageString(this[key], key);
                }
            }

            return new LanguageString();
        }
    }
}
