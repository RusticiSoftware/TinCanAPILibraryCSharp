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
            : this(text, "") { }

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
                return "";
            }
            else
            {
                return this.text;
            }
        }
    }
}
