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
using RusticiSoftware.TinCanAPILibrary.Exceptions;
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace RusticiSoftware.TinCanAPILibrary.Model.TinCan090
{
    public class Statement : IValidatable
    {
        #region Fields
        protected String id;
        protected Actor actor;
        protected StatementVerb verb;
        protected bool inProgress;
        protected StatementTarget _object;
        protected Result result;
        protected Context context;
        protected NullableDateTime timestamp;
        protected Actor authority;
        protected NullableBoolean voided;
        #endregion

        #region Properties
        /// <summary>
        /// String representation of the statement verb
        /// </summary>
        public virtual String Verb
        {
            get
            {
                return verb == StatementVerb.Undefined ? null : verb.ToString().ToLower();
            }
            set
            {
                if (value == null)
                    throw new InvalidArgumentException("Verb may not be null");
                String normalized = value.ToLower();
                try
                {
                    verb = (StatementVerb)Enum.Parse(typeof(StatementVerb), normalized, true);
                }
                catch (Exception)
                {
                    throw new InvalidArgumentException("Verb " + normalized + " is not valid");
                }
            }
        }
        /// <summary>
        /// The statements ID
        /// </summary>
        public String Id
        {
            get { return id; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    id = null;
                }
                else
                {
                    String normalized = value.ToLower();
                    if (!ValidationHelper.IsValidUUID(normalized))
                    {
                        throw new InvalidArgumentException("Statement ID must be UUID");
                    }
                    id = normalized;
                }
            }
        }

        /// <summary>
        /// The statements actor
        /// </summary>
        public Actor Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        /// <summary>
        /// Returns the statement verb in its internal enum format
        /// </summary>
        /// <returns>Statement verb as an enum</returns>
        public StatementVerb GetVerbAsEnum()
        {
            return verb;
        }

        /// <summary>
        /// The target object of this statement
        /// </summary>
        public StatementTarget Object
        {
            get { return _object; }
            set { _object = value; }
        }

        /// <summary>
        /// The result of this statement
        /// </summary>
        public Result Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// Context information for this statement
        /// </summary>
        public Context Context
        {
            get { return context; }
            set { context = value; }
        }

        /// <summary>
        /// The timestamp of this statement
        /// </summary>
        public NullableDateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        /// <summary>
        /// The authority for this statement
        /// </summary>
        public Actor Authority
        {
            get { return authority; }
            set { authority = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public NullableBoolean Voided
        {
            get { return voided; }
            set { voided = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool InProgress
        {
            get { return inProgress; }
            set { inProgress = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Statement() { }

        /// <summary>
        /// Creates a Statement with the minimum suggested properties
        /// </summary>
        /// <param name="actor">The actor in this statement</param>
        /// <param name="verb">The verb in this statement</param>
        /// <param name="statementTarget">The target of this statement</param>
        public Statement(Actor actor, StatementVerb verb, StatementTarget statementTarget)
        {
            this.actor = actor;
            this.verb = verb;
            this._object = statementTarget;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates the statement, ensuring required fields are used
        /// and any necessary information (such as a result for specific verbs)
        /// is provided and valid.
        /// </summary>
        public virtual IEnumerable<ValidationFailure> Validate(bool earlyReturnOnFailure)
        {
            var failures = new List<ValidationFailure>();
            if (actor == null && verb != StatementVerb.Voided)
            {
                failures.Add(new ValidationFailure("Statement " + id + " does not have an actor"));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            if (Verb == null)
            {
                failures.Add(new ValidationFailure("Statement " + id + " does not have a verb"));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            if (_object == null)
            {
                failures.Add(new ValidationFailure("Statement " + id + " does not have an object"));
                if (earlyReturnOnFailure)
                {
                    return failures;
                }
            }
            if (verb == StatementVerb.Voided)
            {
                bool objectStatementIdentified = (_object is TargetedStatement) && !String.IsNullOrEmpty(((TargetedStatement)_object).Id);
                if (!objectStatementIdentified)
                {
                    failures.Add(new ValidationFailure("Statement " + id + " has verb 'voided' but does not properly identify a statement as its object"));
                    if (earlyReturnOnFailure)
                    {
                        return failures;
                    }
                }
            }

            Object[] children = new Object[] { actor, verb, _object, result, context, timestamp, authority };
            foreach (Object o in children)
            {
                if (o != null && o is IValidatable)
                {
                    failures.AddRange(((IValidatable)o).Validate(earlyReturnOnFailure));
                    if (earlyReturnOnFailure && failures.Count > 0)
                    {
                        return failures;
                    }
                }
            }
            return failures;
        }
        #endregion

        #region Verb Handling
        /// <summary>
        /// Handles verbs with special requirements
        /// </summary>
        public void HandleSpecialVerbs()
        {
            if (this.Verb.Equals("passed"))
            {
                result = (result == null) ? new Result() : result;
                VerifySuccessAndCompletionValues(result, "passed", true, true);
                result.Success = true;
                result.Completion = true;
            }
            else if (this.Verb.Equals("failed"))
            {
                result = (result == null) ? new Result() : result;
                VerifySuccessAndCompletionValues(result, "failed", false, true);
                result.Success = false;
                result.Completion = true;
            }
            else if (this.Verb.Equals("completed"))
            {
                result = (result == null) ? new Result() : result;
                VerifyCompletionValue(result, "completed", true);
                result.Completion = true;
            }
        }

        /// <summary>
        /// Validates both success and completion
        /// </summary>
        /// <param name="result">The result object</param>
        /// <param name="verb">The statement verb</param>
        /// <param name="expectedSuccess">The expected success value</param>
        /// <param name="expectedCompletion">The expected completion value</param>
        protected void VerifySuccessAndCompletionValues(Result result, String verb, bool expectedSuccess, bool expectedCompletion)
        {
            VerifySuccessValue(result, verb, expectedSuccess);
            VerifyCompletionValue(result, verb, expectedCompletion);
        }
        /// <summary>
        /// Validates expect success values
        /// </summary>
        /// <param name="result">The result object</param>
        /// <param name="verb">The verb for the statement</param>
        /// <param name="expectedSuccess">What value the success should be</param>
        protected void VerifySuccessValue(Result result, String verb, bool expectedSuccess)
        {
            if (result.Success != null && result.Success.Value != expectedSuccess)
            {
                throw new InvalidArgumentException("Specified verb \"" + verb + "\" but with a result success value of " + result.Success.Value);
            }
        }

        /// <summary>
        /// Validates expected completion values
        /// </summary>
        /// <param name="result">The result object</param>
        /// <param name="verb">The statement verb</param>
        /// <param name="expectedCompletion">What value the completion should be</param>
        protected void VerifyCompletionValue(Result result, String verb, bool expectedCompletion)
        {
            if (result.Completion != null && result.Completion.Value != expectedCompletion)
            {
                throw new InvalidArgumentException("Specified verb \"" + verb + "\" but with a result completion value of " + result.Completion.Value);
            }
        }
        #endregion

        #region TinCan 0.95 Promotion
        /// <summary>
        /// Promotes a TinCan 0.9 statement to a 0.95 statement.  This can be a lossy conversion.
        /// </summary>
        /// <param name="source">A TinCan 0.9 Statement</param>
        /// <returns>A representation of the statement for TinCan 0.95</returns>
        /// <remarks>This method returns a shallow-copy-like conversion.  Any
        /// fields that could be used as reference parameters are, and as
        /// such the two instances of the statement are inextricably linked.</remarks>
        public static explicit operator Model.Statement(Statement source)
        {
            Model.Statement result = new Model.Statement();
            result.Id = source.Id;
            result.Actor = (Model.Actor)source.Actor;
            result.Verb = new Model.StatementVerb(source.verb);
            result.Object = source.Object;
            result.Result = source.Result;
            result.Context = source.Context;
            result.Timestamp = source.Timestamp;
            if (source.Authority != null)
                result.Authority = (Model.Actor)source.Authority;
            result.Voided = source.Voided;

            return result;
        }
        #endregion
    }
}