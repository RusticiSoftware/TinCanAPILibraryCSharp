using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public interface IOfflineStorage
    {
        void AddToStatementQueue(Statement[] statements);
        Statement[] GetQueuedStatements(int count);
        void RemoveStatementsFromQueue(int count);
    }
}
