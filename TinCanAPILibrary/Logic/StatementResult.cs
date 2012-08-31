using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Logic
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

    }
}
