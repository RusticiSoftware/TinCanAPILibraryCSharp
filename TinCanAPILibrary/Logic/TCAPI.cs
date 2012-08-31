using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using RusticiSoftware.TinCanAPILibrary.Exceptions;
using RusticiSoftware.TinCanAPILibrary.Helper;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace RusticiSoftware.TinCanAPILibrary.Logic
{
    /// <summary>
    /// The TCAPI Object which handles the bulk of the logic and communication
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
        private String endpoint;
        private int statementPostInterval;
        private IAuthenticationConfiguration authentification;
        private IOfflineStorage offlineStorage;
        private Actor adminActor;
        private ITCAPICallback tcapiCallback;
        private Queue<Statement> queue = new Queue<Statement>();
        private int queueSize = 50;
        #endregion

        #region Properties
        /// <summary>
        /// Delay between automatic posting of the statement Queue.  Use 0 to disable.
        /// </summary>
        public int StatementPostInterval
        {
            get { return statementPostInterval; }
            set { statementPostInterval = value; }
        }
        /// <summary>
        /// URL Endpoint to send statements to
        /// </summary>
        public String Endpoint
        {
            get { return endpoint; }
            set
            {
                if (value.EndsWith("/"))
                {
                    value = value.Substring(0, value.Length - 1);
                }
                endpoint = value; 
            }
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
            set { offlineStorage = value; }
        }

        /// <summary>
        /// TCAPI Callback to handle AsyncPost success/failure.
        /// </summary>
        public ITCAPICallback TCAPICallback
        {
            get { return tcapiCallback; }
            set { tcapiCallback = value; }
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
            foreach (Statement s in statements)
                s.Validate();
            TinCanJsonConverter converter = new TinCanJsonConverter();
            string postData = converter.SerializeToJSON(statements);
            HttpMethods.PostRequest(postData, endpoint + STATEMENTS, authentification);
        }

        private void StoreStatementsAsync(Statement[] statements)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            string postData = converter.SerializeToJSON(statements);
            HttpMethods.PostRequestAsync(postData, endpoint + STATEMENTS, authentification, tcapiCallback);
        }

        /// <summary>
        /// Stores multiple statements asynchronously
        /// </summary>
        /// <param name="statements">An array of statements to store</param>
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
                queue.Enqueue(s);
            }
            Flush();
        }

        /// <summary>
        /// Voids a series of statements using the administrator actor
        /// </summary>
        /// <param name="statementIdsToVoid">A list of statement IDs</param>
        /// <param name="synchronous"></param>
        public void VoidStatements(string[] statementIdsToVoid)
        {
            Statement[] statementsToVoid = new Statement[statementIdsToVoid.Length];
            TinCanJsonConverter converter = new TinCanJsonConverter();
            for (int i = 0; i < statementIdsToVoid.Length; i++)
            {
                Statement voided = new Statement();
                voided.Object = new TargetedStatement(statementIdsToVoid[i]);
                voided.Verb = StatementVerb.Voided.ToString();
                voided.Actor = adminActor;
                statementsToVoid[i] = voided;
            }
            string postData = converter.SerializeToJSON(statementsToVoid);
            HttpMethods.PostRequest(postData, endpoint + STATEMENTS, authentification);
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
            for (int i = 0; i < statementIdsToVoid.Length; i++)
            {
                Statement voided = new Statement();
                voided.Object = new TargetedStatement(statementIdsToVoid[i]);
                voided.Verb = StatementVerb.Voided.ToString();
                // voided.Actor = adminActor;
                queue.Enqueue(voided);
            }
            Flush();
            /*
            string postData = converter.SerializeToJSON(statementsToVoid);
            HttpMethods.PostRequestAsync(postData, endpoint + STATEMENTS, authentification, callback);
            */
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
            getResult = HttpMethods.GetRequest(nvc, endpoint + ACTOR_PROFILE, authentification);
            Console.Write(getResult);
            result.ProfileId = profileId;
            result.Actor = actor;
            result.Contents = getResult;
            return result;
        }

        /// <summary>
        /// Saves an actor profile
        /// </summary>
        /// <param name="actorProfile">The actor profile instance</param>
        /// <param name="previousProfile">The last instance of the actor profile</param>
        /// <param name="overwrite">Flag to force overwrite of the previous profile</param>
        public void SaveActorProfile(ActorProfile actorProfile, ActorProfile previousProfile, bool overwrite = false)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            string putData = converter.SerializeToJSON(actorProfile.Contents);
            nvc["profileId"] = actorProfile.ProfileId;
            nvc["actor"] = converter.SerializeToJSON(previousProfile.Actor);
            nvc["overwrite"] = overwrite.ToString();
            HttpMethods.PutRequest(putData, nvc, endpoint + ACTOR_PROFILE, authentification);
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
        /// <param name="since">Optional start date</param>
        /// <returns>An array of profile document keys</returns>
        public string[] GetActorProfileIds(Actor actor, NullableDateTime since = null)
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
        /// <param name="registrationId">The registration ID</param>
        /// <param name="since">Exclusive start date</param>
        /// <returns>An array of activity state document keys</returns>
        public string[] GetActivityStateIds(string activityId, Actor actor, string registrationId = null, NullableDateTime since = null)
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
        /// <param name="registrationId">Optional registration ID</param>
        /// <returns>The activity state</returns>
        public ActivityState GetActivityState(string activityId, Actor actor, string stateId, string registrationId = null)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            string getResult;
            ActivityState result = new ActivityState();
            nvc["activityId"] = activityId;
            nvc["actor"] = converter.SerializeToJSON(actor);
            nvc["stateId"] = stateId;
            if (!String.IsNullOrEmpty(registrationId))
                nvc["registrationId"] = registrationId;
            getResult = HttpMethods.GetRequest(nvc, endpoint + ACTIVITY_STATE, authentification);
            result.Contents = getResult;
            result.ActivityId = activityId;
            result.Actor = actor;
            result.Registration = registrationId;
            result.StateId = stateId;
            return result;
        }

        /// <summary>
        /// Saves the activity state
        /// </summary>
        /// <param name="activityState">The activity state to save</param>
        /// <param name="overwrite">Optional parameter to force overwrite</param>
        /// <param name="previousState">Optional parameter for the last known state of the activity</param>
        public void SaveActivityState(ActivityState activityState, bool overwrite = true, ActivityState previousState = null)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            string putData;
            putData = activityState.Contents;
            nvc["overwrite"] = overwrite.ToString();
            nvc["activityId"] = activityState.ActivityId;
            nvc["stateId"] = activityState.StateId;
            nvc["actor"] = converter.SerializeToJSON(activityState.Actor);
            if (activityState.Registration != null)
                nvc["registrationId"] = activityState.Registration;
            if (previousState != null)
                nvc["previousState"] = previousState.Contents;
            HttpMethods.PutRequest(putData, nvc, endpoint + ACTIVITY_STATE, authentification);
        }

        /// <summary>
        /// Deletes the activity state
        /// </summary>
        /// <param name="activityId">The activity ID</param>
        /// <param name="actor">The associated actor</param>
        /// <param name="stateId">The state document key</param>
        /// <param name="registrationId">Optional registration ID</param>
        public void DeleteActivityState(string activityId, Actor actor, string stateId, string registrationId = null)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            nvc["activityId"] = activityId;
            nvc["actor"] = converter.SerializeToJSON(actor);
            nvc["stateId"] = stateId;
            if (!String.IsNullOrEmpty(registrationId))
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
            getResult = HttpMethods.GetRequest(nvc, endpoint + ACTIVITY_PROFILE, authentification);
            Console.Write(getResult);
            result.ProfileId = profileId;
            result.Contents = getResult;
            result.ActivityId = activityId;
            return result;
        }

        /// <summary>
        /// Saves the activity profile
        /// </summary>
        /// <param name="profile">The activity profile to save</param>
        /// <param name="overwrite">Optional parameter to force overwrite</param>
        /// <param name="previous">The last representation of the activity profile</param>
        public void SaveActivityProfile(ActivityProfile profile, bool overwrite = false, ActivityProfile previous = null)
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            NameValueCollection nvc = new NameValueCollection();
            string putData;
            putData = profile.Contents;
            nvc["overwrite"] = overwrite.ToString();
            nvc["activityId"] = profile.ActivityId;
            nvc["profileId"] = profile.ProfileId;
            if (previous != null)
                nvc["previousState"] = previous.Contents;
            HttpMethods.PutRequest(putData, nvc, endpoint + ACTIVITY_PROFILE, authentification);
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
        /// <param name="since">Optional start date parameter</param>
        /// <returns></returns>
        public string[] GetActivityProfileIds(string activityId, NullableDateTime since = null)
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
            Console.Write(getResult);
            result = (Activity)converter.DeserializeJSON(getResult, typeof(Activity));
            return result;
        }

        /// <summary>
        /// Asynchronously pushes the statement queue whenever it fills up or overflows
        /// </summary>
        public void Flush()
        {
            if (queue.Count >= queueSize)
            {
                StoreStatementsAsync(queue.ToArray());
            }
        }

        public string GetOAuthAuthorizationUrl(string redirectUrl = null)
        {
            throw new NotImplementedException();
        }

        public OAuthAuthentication UpdateOAuthTokenCredentials(string temporaryCredentialsId, string verifierCode)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
