using System;
using System.Collections.Generic;

using System.Text;
using RusticiSoftware.TinCanAPILibrary.Exceptions;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class Score : IValidatable
    {
        #region Fields
        protected NullableDouble scaled;
        protected NullableDouble raw;
        protected NullableDouble min;
        protected NullableDouble max;
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
        public void Validate()
        {
            if (scaled != null && (scaled.Value < 0.0 || scaled.Value > 1.0))
            {
                throw new ValidationException("Scaled score must be between 0.0 and 1.0");
            }
            if ((min != null && max != null) && (max.Value < min.Value))
            {
                throw new ValidationException("Max score cannot be lower than min score");
            }
            if (raw != null)
            {
                if (max != null && raw.Value > max.Value)
                {
                    throw new ValidationException("Raw score cannot be greater than max score");
                }
                if (min != null && raw.Value < min.Value)
                {
                    throw new ValidationException("Raw score cannot be less than min score");
                }
            }
        }
        #endregion
    }
}
