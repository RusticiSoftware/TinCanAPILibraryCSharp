using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class Result : Extensible, IValidatable
    {
        #region Fields
        protected Score score;
        protected NullableBoolean success;
        protected NullableBoolean completion;
        protected String response;
        protected NullableScormTimeSpan duration;
        #endregion

        #region Properties
        public Score Score
        {
            get { return score; }
            set { score = value; }
        }

        public NullableBoolean Success
        {
            get { return success; }
            set { success = value; }
        }

        public NullableBoolean Completion
        {
            get { return completion; }
            set { completion = value; }
        }

        public String Response
        {
            get { return response; }
            set { response = value; }
        }

        public NullableScormTimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        #endregion

        #region Constructor
        public Result() { }
        #endregion

        #region Public Methods
        public void Validate()
        {
            Object[] children = new Object[] { score };
            foreach (Object o in children)
                if (o != null && o is IValidatable)
                    ((IValidatable)o).Validate();
        }
        #endregion
    }
}
