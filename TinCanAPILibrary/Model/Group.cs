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
    public class Group : Actor
    {
        #region Constants
        protected static new readonly string OBJECT_TYPE = "Group";
        #endregion

        #region Fields
        private Actor[] member;
        #endregion

        #region Properties
        public override string ObjectType
        {
            get { return OBJECT_TYPE; }
        }

        public Actor[] Member
        {
            get { return member; }
            set { member = value; }
        }
        #endregion

        #region Constructor
        public Group() : base() { }
        #endregion

        #region Public Methods
        public override IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (member == null || member.Length == 0)
            {
                failures.Add(new ValidationFailure("Group must be populated"));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            else
            {
                foreach (Actor a in member)
                {
                    failures.AddRange(a.Validate(earlyReturnOnFailure));
                    if (earlyReturnOnFailure && failures.Count != 0)
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
