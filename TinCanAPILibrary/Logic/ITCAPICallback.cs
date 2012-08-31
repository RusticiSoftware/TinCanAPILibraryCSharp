using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Logic
{
    public interface ITCAPICallback
    {
        bool StatementsFailed(Statement[] failedBatch, Exception e);
        void StatementsStored(Statement[] statements);
        void PostFailException(Exception e);
    }
}
