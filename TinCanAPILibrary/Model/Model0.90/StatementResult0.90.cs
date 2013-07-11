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

namespace RusticiSoftware.TinCanAPILibrary.Model.TinCan090
{
    public class StatementResult
    {
        #region Fields
        private Statement[] statements = new Statement[0];
        private string continueToken;
        private string more;
        #endregion

        #region Properties
        public Statement[] Statements
        {
            get { return statements; }
            set { statements = value; }
        }

        public string ContinueToken
        {
            get { return continueToken; }
            set { continueToken = value; }
        }

        public string More
        {
            get { return more; }
            set { more = value; }
        }
        #endregion

        #region Constructor
        public StatementResult() { }

        public StatementResult(StatementResult source)
        {
            this.statements = source.Statements;
        }
        #endregion

        #region TinCan 0.95 Promotion
        public static explicit operator Model.StatementResult(StatementResult source)
        {
            Model.StatementResult result = new Model.StatementResult();
            result.ContinueToken = source.ContinueToken;
            result.More = source.More;
            result.Statements = new Model.Statement[source.Statements.Length];
            for (int i = 0; i < result.Statements.Length; i++)
            {
                result.Statements[i] = (Model.Statement)source.Statements[i];
            }

            return result;
        }
        #endregion

    }
}
