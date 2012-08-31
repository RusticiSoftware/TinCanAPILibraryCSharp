using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RusticiSoftware.TinCanAPILibrary.Logic;

namespace UnitTests
{
    public class TCAPICallback : ITCAPICallback
    {
        /// <summary>
        /// </summary>
        /// <param name="failedBatch"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        /// <remarks>A normal implementation should use the OfflineStorage interface
        /// to persist the statements depending on the exception
        /// Statements here are typically WebExceptions</remarks>
        public bool StatementsFailed(Statement[] failedBatch, Exception e)
        {
            throw e;
        }

        /// <summary>
        /// </summary>
        /// <param name="statements"></param>
        /// <remarks>This is just an indication of success.  What you do here
        /// is entirely up to you.</remarks>
        public void StatementsStored(Statement[] statements)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        /// <remarks>This method just eats a generic exception
        /// This exception could be converter failure, thread abort,
        /// or any other issue that may arise in async methods</remarks>
        public void PostFailException(Exception e)
        {
            throw e;
        }
    }
}
