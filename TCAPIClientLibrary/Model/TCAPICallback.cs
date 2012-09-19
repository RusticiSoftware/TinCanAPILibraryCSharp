using System;
using RusticiSoftware.TinCanAPILibrary;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    /// <summary>
    /// This is simply a sample implementation of the TCAPI callback that doesn't provide a lot of functionality.
    /// This class should be extended for functionality.
    /// </summary>
    public class TCAPICallback : ITCAPICallback
    {
        #region Fields
        private TCAPI tcapi;
        #endregion

        #region Properties
        /// <summary>
        /// The TCAPI field
        /// </summary>
        public TCAPI TCAPI
        {
            get { return tcapi; }
            set { tcapi = value; }
        }
        #endregion
        /// <summary>
        /// Handles the case in which the statements fail to post
        /// </summary>
        /// <param name="failedBatch">The batch of statements that failed</param>
        /// <param name="e">The thrown exception, usually a webexception</param>
        public void StatementsFailed(Statement[] failedBatch, Exception e)
        {
            throw e;
        }

        /// <summary>
        /// Indicator of storing success.  This will allow the TCAPI object to continue Asynchronous flushing.
        /// </summary>
        /// <param name="statements"></param>
        public void StatementsStored(Statement[] statements)
        {
        }

        /// <summary>
        /// This method catches generic exceptions (not WebExceptions).  In case this is occuring,
        /// it is most likely that the thread is terminating quickly and the conversion of statements from string to statement also failed.
        /// </summary>
        /// <param name="e">The thrown exception</param>
        public void PostFailException(Exception e)
        {
            throw e;
        }
    }
}
