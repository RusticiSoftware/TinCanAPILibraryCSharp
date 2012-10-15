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
    }
}
