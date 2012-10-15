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

namespace RusticiSoftware.TinCanAPILibrary.Model
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
