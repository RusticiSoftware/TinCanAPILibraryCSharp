﻿#region License
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
    public class Result : Extensible, IValidatable
    {
        #region Fields
        private Score score;
        private NullableBoolean success;
        private NullableBoolean completion;
        private string response;
        private NullableScormTimeSpan duration;
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

        public string Response
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
        public IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            object[] children = new object[] { score };
            var failures = new List<ValidationFailure>();
            foreach (object o in children)
            {
                if (o != null && o is IValidatable)
                {
                    failures.AddRange(((IValidatable)o).Validate(earlyReturnOnFailure));
                    if (earlyReturnOnFailure && failures.Count > 0)
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
