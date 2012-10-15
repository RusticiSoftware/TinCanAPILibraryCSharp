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
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using RusticiSoftware.TinCanAPILibrary.Helper;
using RusticiSoftware.TinCanAPILibrary.Exceptions;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    /// <summary>
    /// Internal representation of a time span as defined by SCORM
    /// </summary>
    [Serializable]
    public struct ScormTimeSpan
    {

        //Note - This doesn't use the internal .NET TimeSpan type because it that is only equipped to handle values up to a days, not months and years as well

        /// <summary>
        /// Represents the NULL state for this class
        /// </summary>
        public const long UNDEFINED = NullConstants.NullLong;

        /// <summary>
        /// The number of hundredths of a second in one year, where one year is defined as 12 months. See <see cref="HUNDREDTHS_PER_MONTH"/>.
        /// </summary>
        public const long HUNDREDTHS_PER_YEAR = 3155760000;
        /// <summary>
        /// The number of hundredths of a second in one month. Assumed to be an "average" month 
        /// (figures a leap year every 4 years) = ((365*4) + 1) days / 48 months = 30.4375 days per month
        /// </summary>
        public const int HUNDREDTHS_PER_MONTH = 262980000;
        /// <summary>
        /// The number of hundredths of a second in one day.
        /// </summary>
        public const int HUNDREDTHS_PER_DAY = 8640000;
        /// <summary>
        /// The number of hundredths of a second in one hour.
        /// </summary>
        public const int HUNDREDTHS_PER_HOUR = 360000;
        /// <summary>
        /// The number of hundredths of a second in one minute
        /// </summary>
        public const int HUNDREDTHS_PER_MINUTE = 6000;
        /// <summary>
        /// The number of hundredths of a second in one second.
        /// </summary>
        public const int HUNDREDTHS_PER_SECOND = 100;

        /// <summary>
        /// The current value of the class. Represented in hundredths of a second.
        /// This doesn't use the internal .NET TimeSpan type because it that is only equipped to handle
        /// values up to a days, not months and years as well.
        /// </summary>
        public long Value;

        /// <summary>
        /// A description of what the date means or how it is represented
        /// </summary>
        public LanguageString Description;

        /// <summary>
        /// Creates a new instance of the class from an integer representing hundredths of a second. Hundredths of a 
        /// second is how this data type is stored in the database.
        /// </summary>
        /// <param name="hundredths">The value to instantiate this class with</param>
        public ScormTimeSpan(long hundredths)
        {
            this.Value = hundredths;
            this.Description = new LanguageString();
        }

        public ScormTimeSpan(ScormTimeSpan other)
        {
            this.Value = other.Value;
            this.Description = new LanguageString(other.Description);
        }


        /// <summary>
        /// Creates a new instance of the class from an string adhering various SCORM formats. This function will attempt
        /// to use the ISO 8601 format, then the CMI format. If neither of those succeed, then it will try to parse the time span
        /// using the built-in .NET TimeSpan object.
        /// </summary>
        /// <param name="timeSpanValue">The value to instantiate this class with</param>
        /// <param name="allowNull">If true allow empty string to translate to ScormTimeSpan.UNDEFINED</param>
        /// <exception cref="ManifestFormatException">If the format is not recognizable.</exception>
        public ScormTimeSpan(string timeSpanValue, bool allowNull)
        {
            this.Value = ScormTimeSpan.UNDEFINED;
            this.Description = new LanguageString();

            if (timeSpanValue.Length > 0 || !allowNull)
            {
                this.Parse(timeSpanValue);
            }
        }

        /// <summary>
        /// Creates a new instance of the class from an string adhering various SCORM formats. This function will attempt
        /// to use the ISO 8601 format, then the CMI format. If neither of those succeed, then it will try to parse the time span
        /// using the built-in .NET TimeSpan object.
        /// </summary>
        /// <param name="timeSpanValue">The value to instantiate this class with</param>
        /// <exception cref="ManifestFormatException">If the format is not recognizable.</exception>
        public ScormTimeSpan(string timeSpanValue)
        {
            this.Value = ScormTimeSpan.UNDEFINED;
            this.Description = new LanguageString();
            this.Parse(timeSpanValue);
        }

        private void Parse(string timeSpanValue)
        {
            if (String.IsNullOrEmpty(timeSpanValue))
            {
                throw new InvalidArgumentException("Could not parse TimeSpan -- String was empty");
            }
            else
                try
                {
                    if (timeSpanValue.StartsWith("P"))
                    {
                        this.Value = ParseIso8601TimeSpan(timeSpanValue);
                    }
                    else
                    {
                        this.Value = ParseCmiTimeSpan(timeSpanValue);
                    }
                }
                catch (InvalidArgumentException)
                {
                    // Be kind and give them one last shot with the built-in parser,
                    // even though it's not strictly SCORM
                    try
                    {
                        this.Value = (long)TimeSpan.Parse(timeSpanValue).TotalSeconds * HUNDREDTHS_PER_SECOND;
                    }
                    catch
                    {
                        throw new InvalidArgumentException("Could not parse ScormTimeSpan, invalid format");
                    }

                }
            this.Description = new LanguageString();
        }


        /// <summary>
        /// Returns an ISO 8601 TimeInterval Representation
        /// </summary>
        /// <returns>String representation of the class</returns>
        public string ToIso8601String()
        {

            string isoTimeSpanValue = "";

            if (this.Value == ScormTimeSpan.UNDEFINED)
            {
                return "";
            }

            long hundredthsOfASecond;	//decrementing counter - work at the hundreths of a second level because that is all the precision that is required

            long seconds;	// 100 hundreths of a seconds
            long minutes;		// 60 seconds
            long hours;			// 60 minutes
            long days;			// 24 hours
            long months;		// assumed to be an "average" month (figures a leap year every 4 years) = ((365*4) + 1) / 48 days - 30.4375 days per month
            long years;			// assumed to be 12 "average" months

            hundredthsOfASecond = this.Value;

            years = (long)Math.Floor((double)(hundredthsOfASecond / HUNDREDTHS_PER_YEAR));
            hundredthsOfASecond -= (years * HUNDREDTHS_PER_YEAR);

            months = (long)Math.Floor((double)(hundredthsOfASecond / HUNDREDTHS_PER_MONTH));
            hundredthsOfASecond -= (months * HUNDREDTHS_PER_MONTH);

            days = (long)Math.Floor((double)(hundredthsOfASecond / HUNDREDTHS_PER_DAY));
            hundredthsOfASecond -= (days * HUNDREDTHS_PER_DAY);

            hours = (long)Math.Floor((double)(hundredthsOfASecond / HUNDREDTHS_PER_HOUR));
            hundredthsOfASecond -= (hours * HUNDREDTHS_PER_HOUR);

            minutes = (long)Math.Floor((double)(hundredthsOfASecond / HUNDREDTHS_PER_MINUTE));
            hundredthsOfASecond -= (minutes * HUNDREDTHS_PER_MINUTE);

            seconds = (long)Math.Floor((double)(hundredthsOfASecond / HUNDREDTHS_PER_SECOND));
            hundredthsOfASecond -= (Int64)(seconds * HUNDREDTHS_PER_SECOND);


            if (years > 0)
            {
                isoTimeSpanValue += years + "Y";
            }
            if (months > 0)
            {
                isoTimeSpanValue += months + "M";
            }
            if (days > 0)
            {
                isoTimeSpanValue += days + "D";
            }

            //check to see if we have any time before adding the "T"
            if ((hundredthsOfASecond + seconds + minutes + hours) > 0)
            {

                isoTimeSpanValue += "T";

                if (hours > 0)
                {
                    isoTimeSpanValue += hours + "H";
                }

                if (minutes > 0)
                {
                    isoTimeSpanValue += minutes + "M";
                }

                if ((hundredthsOfASecond + seconds) > 0)
                {
                    isoTimeSpanValue += seconds;

                    if (hundredthsOfASecond > 0)
                    {
                        isoTimeSpanValue += "." + hundredthsOfASecond;
                    }

                    isoTimeSpanValue += "S";
                }

            }


            if (isoTimeSpanValue.Length == 0)
            {
                isoTimeSpanValue = "T0H0M0S";
            }

            isoTimeSpanValue = "P" + isoTimeSpanValue;

            return isoTimeSpanValue;
        }


        /// <summary>
        /// Returns an CMI TimeInterval Representation
        /// </summary>
        /// <returns>String representation of the class</returns>
        public string ToCmiString()
        {

            if (this.Value == ScormTimeSpan.UNDEFINED)
            {
                return "";
            }

            long hundredthsOfASecond;	//decrementing counter - work at the hundreths of a second level because that is all the precision that is required

            long seconds;	// 100 hundreths of a seconds
            long minutes;		// 60 seconds
            long hours;			// 60 minutes

            hundredthsOfASecond = this.Value;

            hours = (long)Math.Floor((double)(hundredthsOfASecond / HUNDREDTHS_PER_HOUR));
            hundredthsOfASecond -= (hours * HUNDREDTHS_PER_HOUR);

            minutes = (long)Math.Floor((double)(hundredthsOfASecond / HUNDREDTHS_PER_MINUTE));
            hundredthsOfASecond -= (minutes * HUNDREDTHS_PER_MINUTE);

            seconds = (long)Math.Floor((double)(hundredthsOfASecond / HUNDREDTHS_PER_SECOND));
            hundredthsOfASecond -= (Int64)(seconds * HUNDREDTHS_PER_SECOND);

            if (hours > 9999)
            {
                return "9999:99:99.99";
            }

            char ZERO = "0".ToCharArray()[0];

            string cmiString = hours.ToString().PadLeft(4, ZERO) + ":" + minutes.ToString().PadLeft(2, ZERO) + ":" + seconds.ToString().PadLeft(2, ZERO);

            // OnTime Feature #699 - Only append millis when non-zero (course compatibility issue).  Technically
            // the millis are ok, but there's a bit of problem content that this helps out.
            if (hundredthsOfASecond > 0)
            {
                cmiString += "." + hundredthsOfASecond.ToString().PadLeft(2, ZERO);
            }

            return cmiString;
        }



        private static long ParseCmiTimeSpan(string timeSpanValue)
        {

            int hours = 0;
            int minutes = 0;
            double seconds = 0;

            if (!IsValidCmiTimeSpan(timeSpanValue))
            {
                throw new InvalidArgumentException("The time interval specified " + timeSpanValue + "is not a valid CMI Time Interval. The string format is invalid.");
            }

            string[] aryTimes = timeSpanValue.Split(":".ToCharArray());

            hours = Convert.ToInt32(aryTimes[0]);

            minutes = Convert.ToInt32(aryTimes[1]);

            seconds = Double.Parse(aryTimes[2], NumberFormatInfo.InvariantInfo);

            return (HUNDREDTHS_PER_HOUR * hours) + (HUNDREDTHS_PER_MINUTE * minutes) + Convert.ToInt32(HUNDREDTHS_PER_SECOND * seconds);
        }


        private static bool IsValidCmiTimeSpan(string timeSpanValue)
        {

            //In this implementation, we're allowing only 1 hours digit, technically the runtime requires 2 hours digits, but
            //some metadata elements may only have one
            //TODO: The metadata elements can technically be any IS08601 format, extend this class to make sure that those are supported
            Regex regValid = new Regex(@"^\d?\d?\d?\d:\d\d:\d\d(.\d\d?)?$");

            if (!regValid.IsMatch(timeSpanValue))
            {
                return false;
            }

            return true;
        }


        private static long ParseIso8601TimeSpan(string timeSpanValue)
        {

            //loop through the string
            //keep track of the current string of digits
            //if a letter is found, check for a valid letter delimiter
            //if the letter delimiter represents a time value, put the current digits into their appropirate variable
            //clear the current digits

            bool inDate = false;
            bool inTime = false;

            StringBuilder currentDigits = new StringBuilder();

            int years = 0;
            int months = 0;
            int days = 0;
            int hours = 0;
            int minutes = 0;
            double seconds = 0;

            //validate the string format
            if (!Iso8601TimeSpan(timeSpanValue))
            {
                throw new InvalidArgumentException("The time interval specified " + timeSpanValue + "is not in the correct format.");
            }

            char[] chars = timeSpanValue.ToCharArray();

            foreach (char currentChar in chars)
            {

                //if is numeric or double 
                if (Char.IsDigit(currentChar) || currentChar == '.')
                {
                    currentDigits.Append(currentChar);
                }
                else
                {
                    switch (currentChar)
                    {
                        case ('P'):
                            inDate = true;
                            inTime = false;
                            break;
                        case ('Y'):
                            years = Convert.ToInt32(currentDigits.ToString());
                            break;
                        case ('M'):
                            if (inDate)
                            {
                                months = Convert.ToInt32(currentDigits.ToString());
                            }
                            if (inTime)
                            {
                                minutes = Convert.ToInt32(currentDigits.ToString());
                            }
                            break;
                        case ('D'):
                            days = Convert.ToInt32(currentDigits.ToString());
                            break;
                        case ('T'):
                            inTime = true;
                            inDate = false;
                            break;
                        case ('H'):
                            hours = Convert.ToInt32(currentDigits.ToString());
                            break;
                        case ('S'):
                            seconds = Double.Parse(currentDigits.ToString(), NumberFormatInfo.InvariantInfo);
                            seconds = Math.Round(seconds, 2);
                            break;
                    }

                    currentDigits = new StringBuilder();
                }
            }

            try
            {
                //use the "checked" keyword to ensure that we trap overflow errors

                long hundredths =
                    /* checked
                    ** keving: commented out 'checked' because it freaks out new cs2j, wheras old cs2j just ignored it
                    ** TODO: Should we replace checked with an explicit sanity check?
                    */
                                        (Convert.ToInt64(
                    (years * HUNDREDTHS_PER_YEAR) +
                    (months * HUNDREDTHS_PER_MONTH) +
                    (days * HUNDREDTHS_PER_DAY) +
                    (hours * HUNDREDTHS_PER_HOUR) +
                    (minutes * HUNDREDTHS_PER_MINUTE) +
                    (seconds * HUNDREDTHS_PER_SECOND)));

                return hundredths;
            }
            catch (System.OverflowException)
            {
                throw new InvalidArgumentException("The time interval specified " + timeSpanValue + "exceeds the maximum value that can be stored in the system. This maximum value is almost 3 billion years so something is whacky here.");
            }
        }


        private static bool Iso8601TimeSpan(string timeSpanValue)
        {

            //if just = "P" or if ends with "T" time interval is invalid
            if (timeSpanValue == "P" || (timeSpanValue.LastIndexOf("T") == (timeSpanValue.Length - 1)))
            {
                return false;
            }

            Regex regValid = new Regex(@"^P(\d+Y)?(\d+M)?(\d+D)?(T(\d+H)?(\d+M)?(\d+(.\d\d?)?S)?)?$");

            if (!regValid.IsMatch(timeSpanValue))
            {
                return false;
            }

            return true;
        }
    }
}
