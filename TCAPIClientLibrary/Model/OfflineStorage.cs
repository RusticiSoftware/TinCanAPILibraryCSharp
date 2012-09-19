using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    /// <summary>
    /// A class for persistent storage of the statementQueue.  This implementation persists the queue to RAM.
    /// </summary>
    public class OfflineStorage : IOfflineStorage
    {
        #region Fields
        private List<Statement> statementList;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates an OfflineStorage with an empty queue
        /// </summary>
        public OfflineStorage()
        {
            this.statementList = new List<Statement>();
        }
        #endregion

        #region IOfflineStorage Members
        /// <summary>
        /// Adds the set of statements to the Queue
        /// </summary>
        /// <param name="statements">Statements to enqueue</param>
        public void AddToStatementQueue(Statement[] statements)
        {
            foreach (Statement s in statements)
                statementList.Add(s);
        }

        /// <summary>
        /// Returns the specified number of statements off the top of the queue as a statement array
        /// </summary>
        /// <param name="count">The maximum number of statements to pull</param>
        /// <returns>An array of statements from the queue or null if empty</returns>
        /// <remarks>Returning null or an empty array is up to the implementer.  Our TCAPI implementation supports either method.</remarks>
        public Statement[] GetQueuedStatements(int count)
        {
            if (count > statementList.Count)
                count = statementList.Count;
            if (count == 0)
                return null;
            Statement[] statements = new Statement[count];
            for (int i = 0; i < count; i++)
                statements[i] = statementList[i];
            return statements;
        }

        /// <summary>
        /// Removes statements from the top of the queue that
        /// </summary>
        /// <param name="count">The number of statements to remove</param>
        public void RemoveStatementsFromQueue(int count)
        {
            for (int i = 0; i < count; i++)
                statementList.RemoveAt(0);
        }
        #endregion
    }
}
