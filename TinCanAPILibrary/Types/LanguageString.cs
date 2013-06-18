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
    /// <summary>
    /// Internal representation of SCORM Metadata elements that derived from vocabulary elements. The elements of
    /// the vocabulary are defined by the particular "source" for that vocabulary.
    /// </summary>
    [Serializable]
    public struct LanguageString
    {
        private string language;
        private string text;

        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public LanguageString(string text)
            : this(text, string.Empty) { }

        public LanguageString(string text, string language)
        {
            this.text = text;
            this.language = language;
        }

        public LanguageString(LanguageString source)
        {
            this.text = source.text;
            this.language = source.language;
        }

        /// <summary>
        /// Allows for an explicit cast to a string
        /// </summary>
        /// <param name="langString">The LangSTring value to be converted to a string</param>
        /// <returns>The string representation of this object</returns>
        public static explicit operator string(LanguageString langString)
        {
            return langString.Text;
        }

        /// <summary>
        /// Returns the Text property as the string representation of this object. 
        /// </summary>
        /// <returns>Text property -- When null, returns empty string</returns>
        public override string ToString()
        {
            if (this.text == null)
            {
                return string.Empty;
            }
            else
            {
                return this.text;
            }
        }
    }
}
