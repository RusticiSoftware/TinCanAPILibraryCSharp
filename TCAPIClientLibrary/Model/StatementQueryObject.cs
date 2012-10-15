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
using System.Collections.Specialized;
using RusticiSoftware.TinCanAPILibrary.Helper;

namespace RusticiSoftware.TinCanAPILibrary.Model
{
    public class StatementQueryObject
    {
        #region Fields
        protected String verb;
        protected StatementTarget targetObject;
        protected String registration;
        protected bool context;
        protected Actor actor;
        protected NullableDateTime since;
        protected NullableDateTime until;
        protected int limit = 25;
        protected bool authoritative;
        protected bool sparse;
        protected Actor instructor;
        protected bool ascending;
        protected String continueToken;
        protected bool historical;
        #endregion

        #region Constructor
        public StatementQueryObject() { }
        #endregion

        #region Properties
        public String Verb
        { 
            get 
            { 
                return verb; 
            } 
            set 
            { 
                verb = value; 
            } 
        }
        public StatementTarget TargetObject 
        { 
            get 
            { 
                return targetObject; 
            } 
            set 
            { 
                targetObject = value; 
            } 
        }
        public String Registration
        {
            get { return registration; }
            set { registration = value; }
        }
        public bool Context
        {
            get { return context; }
            set { context = value; }
        }

        public Actor Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        public NullableDateTime Since
        {
            get { return since; }
            set { since = value; }
        }

        public NullableDateTime Until
        {
            get { return until; }
            set { until = value; }
        }

        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        public bool Authoritative
        {
            get { return authoritative; }
            set { authoritative = value; }
        }

        public bool Sparse
        {
            get { return sparse; }
            set { sparse = value; }
        }

        public Actor Instructor
        {
            get { return instructor; }
            set { instructor = value; }
        }

        public Boolean Ascending
        {
            get { return ascending; }
            set { ascending = value; }
        }

        public String ContinueToken
        {
            get { return continueToken; }
            set { continueToken = value; }
        }

        public Boolean Historical
        {
            get { return historical; }
            set { historical = value; }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Transforms the Query object into a NameValueCollection, which will be transformed into a query string.
        /// </summary>
        /// <param name="version">The TCAPI Version to serialize the statement as.</param>
        /// <returns></returns>
        public NameValueCollection ToNameValueCollection(TCAPIVersion version)
        {
            NameValueCollection nvc = new NameValueCollection();
            TinCanJsonConverter converter = new TinCanJsonConverter();

            if (!String.IsNullOrEmpty(verb))
                nvc["verb"] = verb.ToLower();
            if (targetObject != null)
                nvc["object"] = converter.SerializeToJSON(targetObject);
            if (!String.IsNullOrEmpty(registration))
                nvc["registration"] = registration;
            nvc["context"] = context.ToString();
            if (actor != null)
                switch( version)
                {
                    case TCAPIVersion.TinCan090:
                        nvc["actor"] = converter.SerializeToJSON((Model.TinCan090.Actor)actor);
                        break;
                    default:
                        nvc["actor"] = converter.SerializeToJSON(actor);
                        break;
                }
            if (since != null)
                nvc["since"] = converter.SerializeToJSON(since);
            if (until != null)
                nvc["unti"] = converter.SerializeToJSON(until);
            nvc["limit"] = limit.ToString();
            nvc["authoritative"] = authoritative.ToString();
            nvc["sparse"] = sparse.ToString();
            if (instructor != null)
                nvc["instructor"] = converter.SerializeToJSON(instructor);
            nvc["ascending"] = ascending.ToString();
            if (!String.IsNullOrEmpty(continueToken))
                nvc["continueToken"] = continueToken;
            nvc["historical"] = historical.ToString();


            return nvc;
        }
        #endregion
    }
}
