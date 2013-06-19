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
    public class Score : IValidatable
    {
        #region Fields
        private NullableDouble scaled;
        private NullableDouble raw;
        private NullableDouble min;
        private NullableDouble max;
        #endregion

        #region Properties
        public NullableDouble Scaled
        {
            get { return scaled; }
            set { scaled = value; }
        }

        public NullableDouble Raw
        {
            get { return raw; }
            set { raw = value; }
        }

        public NullableDouble Min
        {
            get { return min; }
            set { max = value; }
        }

        public NullableDouble Max
        {
            get { return max; }
            set { max = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Score() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scaled">The value for scaled, the recommended property</param>
        public Score(double scaled)
        {
            this.scaled = scaled;
        }

        /// <summary>
        /// Constructor for raw score and the score range
        /// </summary>
        /// <param name="raw">The score</param>
        /// <param name="min">The lowest permissible value</param>
        /// <param name="max">The greatest permissible value</param>
        public Score(double raw, double min, double max)
        {
            this.raw = raw;
            this.min = min;
            this.max = max;
        }
        #endregion

        #region Public Methods
        public IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (scaled != null && (scaled.Value < 0.0 || scaled.Value > 1.0))
            {
                failures.Add(new ValidationFailure("Scaled score must be between 0.0 and 1.0"));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            if ((min != null && max != null) && (max.Value < min.Value))
            {
                failures.Add(new ValidationFailure("Max score cannot be lower than min score"));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            if (raw != null)
            {
                if (max != null && raw.Value > max.Value)
                {
                    failures.Add(new ValidationFailure("Raw score cannot be greater than max score"));
                    if (earlyReturnOnFailure)
                    {
                        return failures;
                    }
                }
                if (min != null && raw.Value < min.Value)
                {
                    failures.Add(new ValidationFailure("Raw score cannot be less than min score"));
                    if (earlyReturnOnFailure)
                    {
                        return failures;
                    }
                }
            }
            return failures;
        }
        #endregion
    }
}
