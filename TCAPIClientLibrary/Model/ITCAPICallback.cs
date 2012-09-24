using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public interface ITCAPICallback
    {
        void StatementsFailed(Statement[] failedBatch, Exception e);
        void StatementsStored(Statement[] statements);
    }
}
