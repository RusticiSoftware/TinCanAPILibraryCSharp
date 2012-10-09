using System;
using System.Collections.Generic;

using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model.TinCan090
{
    public class StatementResult
    {
        #region Fields
        protected Statement[] statements = new Statement[0];
        protected String continueToken;
        protected String more;
        #endregion

        #region Properties
        public Statement[] Statements
        {
            get { return statements; }
            set { statements = value; }
        }

        public String ContinueToken
        {
            get { return continueToken; }
            set { continueToken = value; }
        }

        public String More
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
