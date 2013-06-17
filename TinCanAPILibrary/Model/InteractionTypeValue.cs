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
using RusticiSoftware.TinCanAPILibrary.Exceptions;
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    /// <summary>
    /// All of the possible values a InteractionType may obtain
    /// </summary>
    /// <database table="ScormActivityRTInteraction" column="type" />
    [Serializable]
    public enum InteractionTypeValue
    {
        /// <summary>
        /// The value has not been specified. This NULL value corresponds to <see cref="NullConstants.UndefinedEnum"/>.
        /// </summary>
        Undefined = NullConstants.UndefinedEnum,
        /// <summary>
        /// The interaction is a true/false question
        /// </summary>
        TrueFalse = 1,
        /// <summary>
        /// The interaction is a multiple choice question
        /// </summary>
        Choice = 2,
        /// <summary>
        /// The interaction is a fill in the blank question
        /// </summary>
        FillIn = 3,
        /// <summary>
        /// The interaction is a fill in the blank question with a long answer expected
        /// </summary>
        LongFillIn = 4,
        /// <summary>
        /// The interaction is a likert question. Typically a question that asks you to rate something on a scale (1 to 10, or "strongly agree, agree, disagree, strongly disagree", etc
        /// </summary>
        Likert = 5,
        /// <summary>
        /// The interaction requires the learner to correctly associate items with one another
        /// </summary>
        Matching = 6,
        /// <summary>
        /// The interaction requires the learner to perform a multiple step task
        /// </summary>
        Performance = 7,
        /// <summary>
        /// The interaction requires the learner to place items in the correct order
        /// </summary>
        Sequencing = 8,
        /// <summary>
        /// The interaction expects a response that is a number
        /// </summary>
        Numeric = 9,
        /// <summary>
        /// The interaction is not any of the other supported types
        /// </summary>
        Other = 10
    }

    /// <summary>
    /// Internal representation of the SCORM concept of an Interaction Type
    /// </summary>
    [Serializable]
    public struct InteractionType
    {

        /// <summary>
        /// The current value of the interaction type
        /// </summary>
        public InteractionTypeValue Value;

        /// <summary>
        /// Creates a new instance of the class from a value enumeration
        /// </summary>
        /// <param name="value">The value to instantiate this class with</param>
        public InteractionType(InteractionTypeValue value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Creates a new instance of the class from an integer corresponding to a value enumeration
        /// </summary>
        /// <param name="value">The value to instantiate this class with</param>
        public InteractionType(int value)
        {

            if (Enum.IsDefined(typeof(InteractionTypeValue), value))
            {
                this.Value = (InteractionTypeValue)value;
            }
            else
            {
                throw new InvalidArgumentException("The value " + value + " is not a valid Interaction Type");
            }
        }

        /// <summary>
        /// Creates a new instance of the class from the SCORM or AICC vocabulary
        /// </summary>
        /// <param name="vocab">The value to instantiate this class with</param>
        public InteractionType(string vocab)
        {
            switch (vocab.ToLowerInvariant().Trim())
            {
                case "":
                    this.Value = InteractionTypeValue.Undefined;
                    break;
                case "true-false":
                    this.Value = InteractionTypeValue.TrueFalse;
                    break;
                case "choice":
                    this.Value = InteractionTypeValue.Choice;
                    break;
                case "fill-in":
                    this.Value = InteractionTypeValue.FillIn;
                    break;
                case "long-fill-in":
                    this.Value = InteractionTypeValue.LongFillIn;
                    break;
                case "likert":
                    this.Value = InteractionTypeValue.Likert;
                    break;
                case "matching":
                    this.Value = InteractionTypeValue.Matching;
                    break;
                case "performance":
                    this.Value = InteractionTypeValue.Performance;
                    break;
                case "sequencing":
                    this.Value = InteractionTypeValue.Sequencing;
                    break;
                case "numeric":
                    this.Value = InteractionTypeValue.Numeric;
                    break;
                case "other":
                    this.Value = InteractionTypeValue.Other;
                    break;
                default:

                    // Try out AICC cases
                    switch (vocab.Substring(0, 1).ToLowerInvariant())
                    {
                        case "t":
                            this.Value = InteractionTypeValue.TrueFalse;
                            break;
                        case "c":
                            this.Value = InteractionTypeValue.Choice;
                            break;
                        case "f":
                            this.Value = InteractionTypeValue.FillIn;
                            break;
                        case "m":
                            this.Value = InteractionTypeValue.Matching;
                            break;
                        case "p":
                            this.Value = InteractionTypeValue.Performance;
                            break;
                        case "s":
                            this.Value = InteractionTypeValue.Sequencing;
                            break;
                        case "l":
                            this.Value = InteractionTypeValue.Likert;
                            break;
                        case "n":
                            this.Value = InteractionTypeValue.Numeric;
                            break;

                        default:
                            throw new InvalidArgumentException("The value " + vocab + " is not valid.");
                    }
                    break;
            }
        }


        /// <summary>
        /// Represents this class as a string suitable for human viewing
        /// </summary>
        /// <returns>String representation of the class</returns>
        public override string ToString()
        {
            String ret;

            switch (this.Value)
            {
                case InteractionTypeValue.Undefined:
                    ret = "";
                    break;
                case InteractionTypeValue.TrueFalse:
                    ret = "true-false";
                    break;
                case InteractionTypeValue.Choice:
                    ret = "choice";
                    break;
                case InteractionTypeValue.FillIn:
                    ret = "fill-in";
                    break;
                case InteractionTypeValue.LongFillIn:
                    ret = "long-fill-in";
                    break;
                case InteractionTypeValue.Likert:
                    ret = "likert";
                    break;
                case InteractionTypeValue.Matching:
                    ret = "matching";
                    break;
                case InteractionTypeValue.Performance:
                    ret = "performance";
                    break;
                case InteractionTypeValue.Sequencing:
                    ret = "sequencing";
                    break;
                case InteractionTypeValue.Numeric:
                    ret = "numeric";
                    break;
                case InteractionTypeValue.Other:
                    ret = "other";
                    break;
                default:
                    throw new InvalidArgumentException("The value " + this.Value + " is not a valid Interaction Type");
            }

            return ret;
        }

        /// <summary>
        /// When an explicit cast is done to the internal int representation,
        /// represent "unknown" as a NullInt value. This is used so Null
        /// is properly represented in the database
        /// </summary>
        /// <param name="it">InteractionType to be case to an integer</param>
        /// <returns>The integer result of the cast</returns>
        public static explicit operator int(InteractionType it)
        {
            if (it.Value == InteractionTypeValue.Undefined)
                return NullConstants.NullInt;
            else
                return (int)it.Value;
        }
    }
}
