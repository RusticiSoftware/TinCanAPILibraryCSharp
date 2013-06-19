﻿#region License
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
/*using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Timers;
using System.Collections.Specialized;
using RusticiSoftware.TinCanAPILibrary.Exceptions;
using RusticiSoftware.TinCanAPILibrary.Helper;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using RusticiSoftware.TinCanAPILibrary.Model;
using RusticiSoftware.TCAPIClientLibrary.Helper;

namespace RusticiSoftware.TinCanAPILibrary.TinCan090
{
    /// <summary>
    /// The TCAPI object which handles the bulk of the logic and communication
    /// Note the read only properties, which must be assigned in the constructor
    /// are the asynchronous support variables.  If a TCAPI with no AsyncSupport is created,
    /// batching statements for async launch will just populate the offlinestorage indefinitely,
    /// unless a manual call to Flush happens.
    /// </summary>
    public class TCAPI : ITCAPI
    {
        #region Endpoints
        public const string STATEMENTS = "/statements";
        public const string ACTIVITY_STATE = "/activities/state";
        public const string ACTOR_PROFILE = "/actors/profile";
        public const string ACTIVITY_PROFILE = "/activities/profile";
        public const string ACTIVITY = "/activities";
        public const string ACTORS = "/actors";
        #endregion

        #region Fields
        private readonly string endpoint;
        private int statementPostInterval;
        private IAuthenticationConfiguration authentification;
        private readonly IOfflineStorage offlineStorage;
        private Actor adminActor;
        private readonly ITCAPICallback tcapiCallback;
        private int maxBatchSize = 50;
        private Timer asyncPostTimer;
        private AsyncPostCallback asyncPostCallback;
        private object flushLock = new Object();
        private bool isAsyncFlushing;
        #endregion

        #region Properties
        /// <summary>
        /// Delay between automatic posting of the statement Queue.  Use 0 to disable.
        /// </summary>
        public int StatementPostInterval
        {
            get { return statementPostInterval; }
            set 
            {
                asyncPostTimer.Stop();
                statementPostInterval = value;
                asyncPostTimer.Interval = this.statementPostInterval;
                if (!isAsyncFlushing)
                {
                    asyncPostTimer.Enabled = this.statementPostInterval > 0;
                    asyncPostTimer.Start();
                }
            }
        }
        /// <summary>
        /// URL Endpoint to send statements to
        /// </summary>
        public string Endpoint
        {
            get { return endpoint; }
        }
        /// <summary>
        /// The authentification object used to build authorization headers
        /// </summary>
        public IAuthenticationConfiguration Authentification
        {
            get { return authentification; }
            set { authentification = value; }
        }

        /// <summary>
        /// Represents the administrator actor.  Used in voiding statements
        /// </summary>
        public Actor AdminActor
        {
            get { return adminActor; }
            set { adminActor = value; }
        }

        /// <summary>
        /// Offline Storage interface to save statements that fail to post
        /// </summary>
        public IOfflineStorage OfflineStorage
        {
            get { return offlineStorage; }
        }

        /// <summary>
        /// TCAPI Callback to handle AsyncPost success/failure.
        /// </summary>
        public ITCAPICallback TCAPICallback
        {
            get { return tcapiCallback; }
        }

        /// <summary>
        /// Post batch size
        /// </summary>
        public int MaxBatchSize
        {
            get { return maxBatchSize; }
            set { maxBatchSize = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructs a TCAPI object with no asynchronous support.
        /// </summary>
        /// <param name="endpoint">The endpoint for the TCAPI</param>
        /// <param name="authentification">The authentification object</param>
        public TCAPI(string endpoint, IAuthenticationConfiguration authentification)
        {
            this.endpoint = endpoint;
            this.Authentification = authentification;
            this.tcapiCallback = null;
            this.offlineStorage = null;
        }

        /// <summary>
        /// Construct a TCAPI object with asynchronous support with a default post interval of 500ms and a default maxBatchSize of 10.
        /// </summary>
        /// <param name="endpoint">The LRS endpoint</param>
        /// <param name="authentification">Authentification object</param>
        /// <param name="tcapiCallback">Asynchornous callback object</param>
        /// <param name="offlineStorage">Offline Storage object</param>
        public TCAPI(string endpoint, IAuthenticationConfiguration authentification, ITCAPICallback tcapiCallback, IOfflineStorage offlineStorage)
            : this(endpoint, authentification, tcapiCallback, offlineStorage, 500, 10)
        {
        }

        /// <summary>
        /// Construct a TCAPI object with asynchronous support and a default maxBatchSize of 10.
        /// </summary>
        /// <param name="endpoint">The LRS endpoint</param>
        /// <param name="authentification">Authentification object</param>
        /// <param name="tcapiCallback">Asynchornous callback object</param>
        /// <param name="offlineStorage">Offline Storage object</param>
        /// <param name="statementPostInterval">Interval for asynchronous operations to take place, in seconds</param>
        public TCAPI(string endpoint, IAuthenticationConfiguration authentification, ITCAPICallback tcapiCallback, IOfflineStorage offlineStorage, int statementPostInterval)
            : this(endpoint, authentification, tcapiCallback, offlineStorage, statementPostInterval, 10)
        {
        }

        /// <summary>
        /// Construct a TCAPI object with asynchronous support.
        /// </summary>
        /// <param name="endpoint">The LRS endpoint</param>
        /// <param name="authentification">Authentification object</param>
        /// <param name="tcapiCallback">Asynchornous callback object</param>
        /// <param name="offlineStorage">Offline Storage object</param>
        /// <param name="statementPostInterval">Interval for asynchronous operations to take place, in milliseconds</param>
        public TCAPI(string endpoint, IAuthenticationConfiguration authentification, ITCAPICallback tcapiCallback, IOfflineStorage offlineStorage, int statementPostInterval, int maxBatchSize)
        {
            this.endpoint = endpoint;
            this.authentification = authentification;
            this.tcapiCallback = tcapiCallback;
            this.offlineStorage = offlineStorage;
            this.statementPostInterval = statementPostInterval;
            this.maxBatchSize = maxBatchSize;

            this.asyncPostCallback = new AsyncPostCallback(this.PostSuccess, this.PostFailed, this.PostConnectionFailed);
            this.isAsyncFlushing = false;

            asyncPostTimer = new Timer();
            asyncPostTimer.Elapsed += new ElapsedEventHandler(AsyncPostTimerElapsed);
            asyncPostTimer.Interval = this.statementPostInterval;
            asyncPostTimer.Enabled = this.statementPostInterval > 0;
            asyncPostTimer.AutoReset = true;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Stores a single statement
        /// </summary>
        /// <param name="statement">The statement to store</param>
        public void StoreStatement(Statement statement)
        {
            StoreStatements(new Statement[] { statement });
        }
        /// <summary>
        /// Stores a single statement asynchronously
        /// </summary>
        /// <param name="statement">The statement to store</param>
        public void StoreStatement(Statement statement, bool synchronous)
        {
            StoreStatements(new Statement[] { statement }, synchronous);
        }

        /// <summary>
        /// Stores multiple statements
        /// </summary>
        /// <param name="statements">An array of statements to store</param>
        public void StoreStatements(Statement[] statements)
        {
            StoreStatements(statements, null);
        }

        /// <summary>
        /// Stores statements synchronously, must be called from a separate thread
        /// </summary>
        /// <param name="statements">Statements to store</param>
        /// <param name="callback">Callback to signal upon completion</param>
        private void StoreStatements(Statement[] statements, AsyncPostCallback callback)
        {
            // Break the statement into a 2D array.  First index is the number of batches, second is the number of
            // statements in that batch, which is either maxBatchSize or statements.Length % maxBatchSize 
            Statement[][] batches = new Statement[(statements.Length - 1) / maxBatchSize + 1][];
            for (int i = 0; i < batches.Length; i++)
            {
                batches[i] = new Statement[statements.Length - (maxBatchSize * (i + 1)) >= 0 ? maxBatchSize : statements.Length % maxBatchSize];
                Array.Copy(statements, i * maxBatchSize, batches[i], 0, batches[i].Length);
            }
            for (int round = 0; round < batches.Length; round++)
            {
                foreach (Statement s in batches[round])
                    s.Validate();
                TinCanJsonConverter converter = new TinCanJsonConverter();
                string postData = converter.SerializeToJSON(batches[round]);
                HttpMethods.PostRequest(postData, endpoint + STATEMENTS, authentification, callback);
            }
        }

        /// <summary>
        /// Stores multiple statements asynchronously
        /// </summary>
        /// <param name="statements">An array of statements to store</param>
        /// <param name="synchronous">When true, stores statements synchronously.  When false, adds the statements to the queue.</param>
        /// <remarks></remarks>
        public void StoreStatements(Statement[] statements, bool synchronous)
        {
            if (synchronous)
            {
                StoreStatements(statements);
                return;
            }
            foreach (Statement s in statements)
            {
                s.Validate();
            }
            offlineStorage.AddToStatementQueue(statements);
        }

        /// <summary>
        /// Voids a series of statements using the administrator actor
        /// </summary>
        /// <param name="statementIdsToVoid">A list of statement IDs</param>
        /// <param name="synchronous"></param>
        public void VoidStatements(string[] statementIdsToVoid)
        {
            Statement[] statementsToVoid = new Statement[statementIdsToVoid.Length];
            for (int i = 0; i < statementIdsToVoid.Length; i++)
            {
                Statement voided = new Statement();
                voided.Object = new TargetedStatement(statementIdsToVoid[i]);
                voided.Verb = StatementVerb.Voided.ToString();
                voided.Actor = adminActor;
                statementsToVoid[i] = voided;
            }
            StoreStatements(statementsToVoid);
        }

        /// <summary>
        /// Voids a series of statements using the administrator actor
        /// </summary>
        /// <param name="statementIdsToVoid">A list of statement IDs</param>
        /// <param name="synchronous"></param>
        public void VoidStatements(string[] statementIdsToVoid, bool synchronous)
        {
            if (synchronous)
            {
                VoidStatements(statementIdsToVoid);
                return;
            }
            TinCanJsonConverter converter = new TinCanJsonConverter();
            Statement[] statements = new Statement[statementIdsToVoid.Length];
            for (int i = 0; i < statementIdsToVoid.Length; i++)
            {
                Statement voided = new Statement();
                voided.Object = new TargetedStatement(statementIdsToVoid[i]);
                voided.Verb = StatementVerb.Voided.ToString();
                voided.Actor = adminActor;
                statements[i] = voided;
            }
            offlineStorage.AddToStatementQueue(statements);
        }

        /// <summary>
        /// Retreives a statement with a given ID
        /// </summary>
        /// <param name="statementId">Statement to retreive</param>
        /// <returns>The statement with this ID</returns>
        public Statement GetStatement(string statementId)
        {
            Statement statement = new Statement();
            statement.Id = statementId;
            TinCanJsonConverter converter = new TinCanJsonConverter();
            string statementToGet = converter.SerializeToJSON(statement);
            NameValueCollection nvc = new NameValueCollection();
            nvc["statementId"] = statementId;
            string result = HttpMethods.GetRequest(nvc, endpoint + STATEMENTS, authentification);
            Statement statementResult = (Statement)converter.DeserializeJSON(result, typeof(Statement));
            return statementResult;
        }

        /// <summary>
        /// Retreives all statements based matched by the query
        /// </summary>
        /// <param name="queryObject">Object to create a statement query with</param>
        /// <returns>A StatementResult with all the statements</returns>
        public StatementResult GetStatements(StatementQueryObject queryObject)
        {
            NameValueCollection nvc = queryObject.ToNameValueCollection();
            string resultAsJSON = HttpMethods.GetRequest(nvc, endpoint + STATEMENTS, authentification);
            TinCanJsonConverter converter = new TinCanJsonConverter();
            StatementResult result = (StatementResult)converter.DeserializeJSON(resultAsJSON, typeof(StatementResult));
            return result;
        }

        /// <summary>
        /// Retreives the statements from a continue URL
        /// </summary>
        /// <param name="moreUrl"></param>
        /// <returns></returns>
        public StatementResult GetStatements(string moreUrl)
        {
            string getResult = HttpMethods.GetRequest(null, endpoint + moreUrl, authentification);
            TinCanJsonConverter converter = new TinCanJsonConverter();
            StatementResult result = (StatementResult)converter.DeserializeJSON(getResult, typeof(StatementResult));
            return result;
        }

        /// <summary>
        /// Retreives a complete actor given a partial actor
        /// </summary>
        /// <param name="partialActor">An actor containing at least one inverse functional property</param>
        /// <returns></returns>
        public Actor GetActor(Actor partialActor)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            string getResult;
            Actor result;
                
            nvc["actor"] = converter.SerializeToJSON(partialActor);
            getResult = HttpMethods.GetRequest(nvc, endpoint + ACTORS, authentification);
            result = (Actor)converter.DeserializeJSON(getResult, typeof(Actor));

            return result;
        }

        /// <summary>
        /// Retrieves an Actor profile
        /// </summary>
        /// <param name="actor">The actor that owns the profile</param>
        /// <param name="profileId">The profile document key</param>
        /// <returns></returns>
        public ActorProfile GetActorProfile(Actor actor, string profileId)
        {
            ActorProfile result = new ActorProfile();
            string getResult;
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            nvc["actor"] = converter.SerializeToJSON(actor);
            nvc["profileId"] = profileId;
            WebHeaderCollection whc;
            getResult = HttpMethods.GetRequest(nvc, endpoint + ACTOR_PROFILE, authentification, out whc);
            if (whc != null)
                result.ContentType = whc["Content-Type"];
            result.ProfileId = profileId;
            result.Actor = actor;
            result.Body = getResult;
            return result;
        }

        /// <summary>
        /// Saves an actor profile
        /// </summary>
        /// <param name="actorProfile">The actor profile instance</param>
        /// <param name="previousProfile">The last instance of the actor profile</param>
        /// <param name="overwrite">Flag to force overwrite of the previous profile</param>
        public void SaveActorProfile(ActorProfile actorProfile, ActorProfile previousProfile)
        {
            SaveActorProfile(actorProfile, previousProfile, false);
        }

        /// <summary>
        /// Saves an actor profile
        /// </summary>
        /// <param name="actorProfile">The actor profile instance</param>
        /// <param name="previousProfile">The last instance of the actor profile</param>
        /// <param name="overwrite">Flag to force overwrite of the previous profile</param>
        public void SaveActorProfile(ActorProfile actorProfile, ActorProfile previousProfile, bool overwrite)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            string putData = actorProfile.Body;
            nvc["profileId"] = actorProfile.ProfileId;
            nvc["actor"] = converter.SerializeToJSON(previousProfile.Actor);
            nvc["overwrite"] = overwrite.ToString();
            string previousSha1 = string.Empty;
            if (previousProfile != null)
                previousSha1 = BitConverter.ToString(Encryption.GetSha1Hash(Encoding.UTF8.GetBytes(previousProfile.Body))).Replace("-", "");
            string contentType = actorProfile.ContentType;
            HttpMethods.PutRequest(putData, nvc, endpoint + ACTOR_PROFILE, authentification, contentType, previousSha1);
        }

        /// <summary>
        /// Deletes an actor profile
        /// </summary>
        /// <param name="actor">The actor that owns the profile</param>
        /// <param name="profileId">The profile document key</param>
        public void DeleteActorProfile(Actor actor, string profileId)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            nvc["actor"] = converter.SerializeToJSON(actor);
            nvc["profileId"] = profileId;
            HttpMethods.DeleteRequest(nvc, endpoint + ACTOR_PROFILE, authentification);
        }

        /// <summary>
        /// Deletes all the actor profiles for a given actor
        /// </summary>
        /// <param name="actor">The actor to delete profiles from</param>
        public void DeleteAllActorProfile(Actor actor)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            nvc["actor"] = converter.SerializeToJSON(actor);
            HttpMethods.DeleteRequest(nvc, endpoint + ACTOR_PROFILE, authentification);
        }

        /// <summary>
        /// Retrieves a list of all actor profile document keys
        /// </summary>
        /// <param name="actor">The actor that owns the document keys</param>
        /// <returns>An array of profile document keys</returns>
        public string[] GetActorProfileIds(Actor actor)
        {
            return GetActorProfileIds(actor, null);
        }

        /// <summary>
        /// Retrieves a list of all actor profile document keys
        /// </summary>
        /// <param name="actor">The actor that owns the document keys</param>
        /// <param name="since">Optional start date</param>
        /// <returns>An array of profile document keys</returns>
        public string[] GetActorProfileIds(Actor actor, NullableDateTime since)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            string[] getResult;
            nvc["actor"] = converter.SerializeToJSON(actor);
            if (since != null)
                nvc["since"] = since.Value.ToString(Constants.ISO8601_DATE_FORMAT);
            getResult = (string[])converter.DeserializeJSON(HttpMethods.GetRequest(nvc, endpoint + ACTOR_PROFILE, authentification), typeof(string[]));
            return getResult;
        }

        /// <summary>
        /// Gets all the activity states for an activity
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <param name="actor">The actor</param>
        /// <returns>An array of activity state document keys</returns>
        public string[] GetActivityStateIds(string activityId, Actor actor)
        {
            return GetActivityStateIds(activityId, actor, null, null);
        }

        /// <summary>
        /// Gets all the activity states for an activity
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <param name="actor">The actor</param>
        /// <param name="since">Exclusive start date</param>
        /// <returns>An array of activity state document keys</returns>
        public string[] GetActivityStateIds(string activityId, Actor actor, NullableDateTime since)
        {
            return GetActivityStateIds(activityId, actor, null, since);
        }

        /// <summary>
        /// Gets all the activity states for an activity
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <param name="actor">The actor</param>
        /// <param name="registrationId">The registration ID</param>
        /// <param name="since">Exclusive start date</param>
        /// <returns>An array of activity state document keys</returns>
        public string[] GetActivityStateIds(string activityId, Actor actor, string registrationId, NullableDateTime since)
        {
            string[] stateIds;
            string getResult;
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            nvc["activityId"] = activityId;
            nvc["actor"] = converter.SerializeToJSON(actor);
            if (!string.IsNullOrEmpty(registrationId))
                nvc["registrationId"] = registrationId;
            if (since != null)
                nvc["since"] = since.Value.ToString(Constants.ISO8601_DATE_FORMAT);
            getResult = HttpMethods.GetRequest(nvc, endpoint + ACTIVITY_STATE, authentification);
            stateIds = (string[])converter.DeserializeJSON(getResult, typeof(string[]));
            return stateIds;
        }

        /// <summary>
        /// Retrieves a specific activity state
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <param name="actor">The actor associated with the state</param>
        /// <param name="stateId">The state document id</param>
        /// <returns>The activity state</returns>
        public ActivityState GetActivityState(string activityId, Actor actor, string stateId)
        {
            return GetActivityState(activityId, actor, stateId, null);
        }
        
        /// <summary>
        /// Retrieves a specific activity state
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <param name="actor">The actor associated with the state</param>
        /// <param name="stateId">The state document id</param>
        /// <param name="registrationId">Optional registration ID</param>
        /// <returns>The activity state</returns>
        public ActivityState GetActivityState(string activityId, Actor actor, string stateId, string registrationId)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            string getResult;
            ActivityState result = new ActivityState();
            nvc["activityId"] = activityId;
            nvc["actor"] = converter.SerializeToJSON(actor);
            nvc["stateId"] = stateId;
            if (!string.IsNullOrEmpty(registrationId))
                nvc["registrationId"] = registrationId;
            WebHeaderCollection whc;
            getResult = HttpMethods.GetRequest(nvc, endpoint + ACTIVITY_STATE, authentification, out whc);
            if (whc != null)
                result.ContentType = whc["Content-Type"];
            result.Body = getResult;
            result.ActivityId = activityId;
            result.Actor = actor;
            result.RegistrationId = registrationId;
            result.StateId = stateId;
            return result;
        }

        /// <summary>
        /// Saves the activity state
        /// </summary>
        /// <param name="activityState">The activity state to save</param>
        public void SaveActivityState(ActivityState activityState)
        {
            SaveActivityState(activityState, true, null);
        }

        /// <summary>
        /// Saves the activity state
        /// </summary>
        /// <param name="activityState">The activity state to save</param>
        /// <param name="overwrite">Optional parameter to force overwrite</param>
        public void SaveActivityState(ActivityState activityState, bool overwrite)
        {
            SaveActivityState(activityState, overwrite, null);
        }

        /// <summary>
        /// Saves the activity state
        /// </summary>
        /// <param name="activityState">The activity state to save</param>
        /// <param name="previousState">Optional parameter for the last known state of the activity</param>
        public void SaveActivityState(ActivityState activityState, ActivityState previousState)
        {
            SaveActivityState(activityState, true, previousState);
        }
                
        /// <summary>
        /// Saves the activity state
        /// </summary>
        /// <param name="activityState">The activity state to save</param>
        /// <param name="overwrite">Optional parameter to force overwrite</param>
        /// <param name="previousState">Optional parameter for the last known state of the activity</param>
        public void SaveActivityState(ActivityState activityState, bool overwrite, ActivityState previousState)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            string putData;
            string previousSha1 = string.Empty;
            putData = activityState.Body;
            nvc["overwrite"] = overwrite.ToString();
            nvc["activityId"] = activityState.ActivityId;
            nvc["stateId"] = activityState.StateId;
            nvc["actor"] = converter.SerializeToJSON(activityState.Actor);
            if (activityState.RegistrationId != null)
                nvc["registrationId"] = activityState.RegistrationId;
            if (previousState != null)
                previousSha1 = BitConverter.ToString(Encryption.GetSha1Hash(Encoding.UTF8.GetBytes(previousState.Body))).Replace("-", "");
            string contentType = activityState.ContentType;
            HttpMethods.PutRequest(putData, nvc, endpoint + ACTIVITY_STATE, authentification, contentType, previousSha1);
        }

        /// <summary>
        /// Deletes the activity state
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <param name="actor">The associated actor</param>
        /// <param name="stateId">The state document key</param>
        /// <param name="registrationId">Optional registration ID</param>
        public void DeleteActivityState(string activityId, Actor actor, string stateId)
        {
            DeleteActivityState(activityId, actor, stateId, null);
        }

        /// <summary>
        /// Deletes the activity state
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <param name="actor">The associated actor</param>
        /// <param name="stateId">The state document key</param>
        /// <param name="registrationId">Optional registration ID</param>
        public void DeleteActivityState(string activityId, Actor actor, string stateId, string registrationId)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            nvc["activityId"] = activityId;
            nvc["actor"] = converter.SerializeToJSON(actor);
            nvc["stateId"] = stateId;
            if (!string.IsNullOrEmpty(registrationId))
                nvc["registrationId"] = registrationId;
            HttpMethods.DeleteRequest(nvc, endpoint + ACTIVITY_STATE, authentification);
        }

        /// <summary>
        /// Retrieves the ActivityProfile
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <param name="profileId">The profile document key</param>
        /// <returns></returns>
        public ActivityProfile GetActivityProfile(string activityId, string profileId)
        {
            ActivityProfile result = new ActivityProfile();
            string getResult;
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            nvc["profileId"] = profileId;
            nvc["activityId"] = activityId;
            WebHeaderCollection whc;
            getResult = HttpMethods.GetRequest(nvc, endpoint + ACTIVITY_PROFILE, authentification, out whc);
            if (whc != null)
                result.ContentType = whc["Content-Type"];
            result.ProfileId = profileId;
            result.Body = getResult;
            result.ActivityId = activityId;
            return result;
        }

        /// <summary>
        /// Saves the activity profile
        /// </summary>
        /// <param name="profile">The activity profile to save</param>
        /// <param name="overwrite">Optional parameter to force overwrite</param>
        public void SaveActivityProfile(ActivityProfile profile)
        {
            SaveActivityProfile(profile, false, null);
        }

        /// <summary>
        /// Saves the activity profile
        /// </summary>
        /// <param name="profile">The activity profile to save</param>
        public void SaveActivityProfile(ActivityProfile profile, bool overwrite)
        {
            SaveActivityProfile(profile, overwrite, null);
        }

        /// <summary>
        /// Saves the activity profile
        /// </summary>
        /// <param name="profile">The activity profile to save</param>
        /// <param name="previous">The last representation of the activity profile</param>
        public void SaveActivityProfile(ActivityProfile profile, ActivityProfile previous)
        {
            SaveActivityProfile(profile, false, previous);
        }

        /// <summary>
        /// Saves the activity profile
        /// </summary>
        /// <param name="profile">The activity profile to save</param>
        /// <param name="overwrite">Optional parameter to force overwrite</param>
        /// <param name="previous">The last representation of the activity profile</param>
        public void SaveActivityProfile(ActivityProfile profile, bool overwrite, ActivityProfile previous)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            string putData, previousSha1 = string.Empty;
            putData = profile.Body;
            nvc["overwrite"] = overwrite.ToString();
            nvc["activityId"] = profile.ActivityId;
            nvc["profileId"] = profile.ProfileId;
            if (previous != null)
                previousSha1 = BitConverter.ToString(Encryption.GetSha1Hash(Encoding.UTF8.GetBytes(previous.Body))).Replace("-", "");
            string contentType = profile.ContentType;
            HttpMethods.PutRequest(putData, nvc, endpoint + ACTIVITY_PROFILE, authentification, contentType, previousSha1);
        }

        /// <summary>
        /// Deletes the profile for a given activity
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <param name="profileId">The profile document key</param>
        public void DeleteActivityProfile(string activityId, string profileId)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            nvc["activityId"] = activityId;
            nvc["profileId"] = profileId;
            HttpMethods.DeleteRequest(nvc, endpoint + ACTIVITY_PROFILE, authentification);
        }

        /// <summary>
        /// Deletes all of the profiles associated with the activity
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        public void DeleteAllActivityProfile(string activityId)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            nvc["activityId"] = activityId;
            HttpMethods.DeleteRequest(nvc, endpoint + ACTIVITY_PROFILE, authentification);
        }

        /// <summary>
        /// Gets all of the profile document keys for an activity
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <returns></returns>
        public string[] GetActivityProfileIds(string activityId)
        {
            return GetActivityProfileIds(activityId, null);
        }

        /// <summary>
        /// Gets all of the profile document keys for an activity
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <param name="since">Optional start date parameter</param>
        /// <returns></returns>
        public string[] GetActivityProfileIds(string activityId, NullableDateTime since)
        {
            string[] profileIds;
            string getResult;
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            nvc["activityId"] = activityId;
            if (since != null)
                nvc["since"] = since.Value.ToString(Constants.ISO8601_DATE_FORMAT);
            getResult = HttpMethods.GetRequest(nvc, endpoint + ACTIVITY_PROFILE, authentification);
            profileIds = (string[])converter.DeserializeJSON(getResult, typeof(string[]));
            return profileIds;
        }

        /// <summary>
        /// Gets an Activity
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <returns></returns>
        public Activity GetActivity(string activityId)
        {
            Activity result = new Activity();
            string getResult;
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            nvc["activityId"] = activityId;
            getResult = HttpMethods.GetRequest(nvc, endpoint + ACTIVITY, authentification);
            result = (Activity)converter.DeserializeJSON(getResult, typeof(Activity));
            return result;
        }

        /// <summary>
        /// Synchronously flushes the async queue, emptying the buffer completely.
        /// Any failed statements get pushed back into the buffer by default (and then throw their exception).
        /// </summary>
        public void Flush()
        {
            this.asyncPostTimer.Stop();
            Statement[] statements = offlineStorage.GetQueuedStatements(maxBatchSize);
            if (statements != null && statements.Length > 0)
                lock (flushLock) // Incase the Async Flush is running concurrently, await a lock on flushLock to prevent InvalidOperationException
                {
                    while ((statements = offlineStorage.GetQueuedStatements(maxBatchSize)) != null && statements.Length > 0)
                    {
                        StoreStatements(statements);
                        offlineStorage.RemoveStatementsFromQueue(statements.Length);
                    }
                }
            asyncPostTimer.Start();
        }

        #region OAuth
        public string GetOAuthAuthorizationUrl(string redirectUrl)
        {
            throw new NotImplementedException();
        }

        public OAuthAuthentication UpdateOAuthTokenCredentials(string temporaryCredentialsId, string verifierCode)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Disposes the resources held by the TCAPI
        /// </summary>
        public void Dispose()
        {
            if (asyncPostTimer != null)
            {
                asyncPostTimer.Stop();
                asyncPostTimer.Dispose();
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// This method is fired by the AsyncPostCallback every time FlushAsync succeeds.
        /// </summary>
        private void PostSuccess(Statement[] statements)
        {
            offlineStorage.RemoveStatementsFromQueue(statements.Length);
        }

        private void PostFailed(Statement[] statements, Exception e)
        {
            offlineStorage.RemoveStatementsFromQueue(statements.Length);
        }

        private void PostConnectionFailed(Exception e)
        {

        }
        #endregion

        #region Private Methods
        private void AsyncPostTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                FlushAsync();
            }
            catch (Exception ex)
            {
                asyncPostTimer.Start();
                throw ex;
            }
        }

        /// <summary>
        /// Runs multiple HTTPPostRequests serially and asynchronously.
        /// </summary>
        private void FlushAsync()
        {
            Statement[] statements = offlineStorage.GetQueuedStatements(maxBatchSize);
            if (statements == null || statements.Length == 0)
                return;
            this.isAsyncFlushing = true;
            if (asyncPostTimer.Enabled) // Freeze the post timer while flushing occurs.
                asyncPostTimer.Stop();
            // Place a mutex lock on flushLock while the Async Method is pushing the statements.  Once that's done the lock will release and this should prevent clashes between Flush and FlushAsync
            lock (flushLock)
            {
                while ((statements = offlineStorage.GetQueuedStatements(maxBatchSize)) != null && statements.Length > 0)
                {
                    asyncPostCallback.Statements = statements;
                    StoreStatements(statements, asyncPostCallback);
                    offlineStorage.RemoveStatementsFromQueue(statements.Length);
                }
                asyncPostTimer.Start();
                this.isAsyncFlushing = false;
            }
        }
        #endregion

        #region AsyncPostCallback
        public class AsyncPostCallback
        {
            public delegate void AsyncPostSuccess(Statement[] statements);
            public delegate void AsyncPostFailed(Statement[] statements, Exception e);
            public delegate void AsyncPostConnectionFailure(Exception e);
            private event AsyncPostSuccess eventPostSuccess;
            private event AsyncPostFailed eventPostFailed;
            private event AsyncPostConnectionFailure eventConnectionFailed;
            private Statement[] statements;
            
            public Statement[] Statements
            {
                get { return statements; }
                set { statements = value; }
            }

            public AsyncPostCallback(AsyncPostSuccess handler, AsyncPostFailed failHandler, AsyncPostConnectionFailure connectHandler)
            {
                eventPostSuccess += handler;
                eventPostFailed += failHandler;
                eventConnectionFailed += connectHandler;
            }

            public void AsyncPostComplete()
            {
                eventPostSuccess(statements);
            }

            public void AsyncPostFailure(Exception e)
            {
                eventPostFailed(statements, e);
            }

            public void AsyncPostConnectionFailed(Exception e)
            {
                eventConnectionFailed(e);
            }
        }
        #endregion
    }
}
*/