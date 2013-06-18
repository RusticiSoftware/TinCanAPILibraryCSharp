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
using RusticiSoftware.TinCanAPILibrary.Model;
using System;
using System.Collections.Specialized;
using RusticiSoftware.TinCanAPILibrary;
using System.Net;
using System.IO;
using RusticiSoftware.TinCanAPILibrary.Helper;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace UnitTests
{
    /// <summary>
    ///This is a test class for TCAPITest and is intended
    ///to contain all TCAPITest Unit Tests
    ///</summary>
    [TestFixture]
    public class TCAPIFixture
    {
        private const string INCONCLUSIVE = "The results of this test should be viewed on the LRS itself.";
        private const string INCONCLUSIVE_CONSOLE = "The expected results of this test should be compared to the console output.";

        #region Individual Unit Tests (No meaningful Asserts)
        /// <summary>
        ///A test for StoreStatement
        ///</summary>
        ///
        [Test]
        public void StoreStatementTest()
        {
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            target.MaxBatchSize = 1;
            Statement[] statements = new Statement[1];
            TinCanActivity activity = new TinCanActivity("http://www.example.com");
            activity.Definition = new ActivityDefinition();
            activity.Definition.Name = new LanguageMap();
            activity.Definition.Name.Add("en-US", "TCAPI C# 0.95 Library.");
            statements[0] = new Statement(new Actor("Example", "mailto:test@example.com"), new StatementVerb(PredefinedVerbs.Experienced), activity);
            target.StoreStatements(statements);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for VoidStatements
        ///</summary>
        [Test]
        [Ignore]
        public void VoidStatementsTest()
        {
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            target.AdminActor = new Actor("Example", "mailto:test@example.com");
            string[] statementIdsToVoid = { "c17c9b10-95d4-4579-90d2-d2d4683fb88b" };
            target.VoidStatements(statementIdsToVoid);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for GetStatement
        ///</summary>
        [Test]
        [Ignore]
        public void GetStatementTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            Statement actual;
            actual = target.GetStatement("c17c9b10-95d4-4579-90d2-d2d4683fb88b");
            Console.Write(converter.SerializeToJSON(actual));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for GetStatements
        ///</summary>
        [Test]
        public void GetStatementsTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"), TCAPIVersion.TinCan095);
            StatementQueryObject queryObject = new StatementQueryObject();
            queryObject.Actor = new Actor("Example", "mailto:test@example.com");
            queryObject.Since = new DateTime(2013, 6, 1);
            queryObject.Limit = 50;
            int limit = 0;
            StatementResult actual;
            actual = target.GetStatements(queryObject);
            limit = actual.Statements.Length;
            Console.Write(converter.SerializeToJSON(actual));
            while (limit <= 50 && !string.IsNullOrEmpty(actual.More))
            {
                actual = target.GetStatements(actual.More);
                Console.Write(converter.SerializeToJSON(actual));
                limit += actual.Statements.Length;
                //break;
            }
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for SaveActorProfile
        ///</summary>
        [Test]
        [Ignore]
        public void SaveActorProfileTest()
        {
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            ActorProfile actorProfile = new ActorProfile();
            actorProfile.Actor = new Actor("Example", "mailto:test@example.com");
            actorProfile.ProfileId = "Example";
            actorProfile.Body = "This is some test";
            ActorProfile previousProfile = new ActorProfile();
            previousProfile.Actor = new Actor("Example", "mailto:test@example.com");
            previousProfile.ProfileId = "Example";
            previousProfile.Body = "Hello";
            bool overwrite = true;
            target.SaveActorProfile(actorProfile, previousProfile, overwrite);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for GetActorProfile
        ///</summary>
        [Test]
        [Ignore]
        public void GetActorProfileTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Example", "mailto:test@example.com");
            string profileId = "Example";
            ActorProfile actual;
            actual = target.GetActorProfile(actor, profileId);
            Console.Write(converter.SerializeToJSON(actual));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for GetActor
        ///</summary>
        [Test]
        [Ignore]
        public void GetActorTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            Actor partialActor = new Actor();
            partialActor.Mbox = "mailto:test@example.com";
            Actor fullActor = target.GetActor(partialActor);
            Console.Write(converter.SerializeToJSON(fullActor));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for GetActorProfileIds
        ///</summary>
        [Test]
        [Ignore]
        public void GetActorProfileIdsTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Example", "mailto:test@example.com");
            NullableDateTime since = null;
            string[] actual;
            actual = target.GetActorProfileIds(actor, since);
            Console.Write(converter.SerializeToJSON(actual));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for DeleteActorProfile
        ///</summary>
        [Test]
        [Ignore]
        public void DeleteActorProfileTest()
        {
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Example", "mailto:test@example.com");
            string profileId = "Example";
            target.DeleteActorProfile(actor, profileId);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for DeleteAllActorProfile
        ///</summary>
        [Test]
        [Ignore]
        public void DeleteAllActorProfileTest()
        {
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Example", "mailto:test@example.com"); // TODO: Initialize to an appropriate value
            target.DeleteAllActorProfile(actor);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for GetActivityStateIds
        ///</summary>
        [Test]
        [Ignore]
        public void GetActivityStateIdsTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Example", "mailto:test@example.com");
            string activityId = "example.com";
            string registrationId = null;
            NullableDateTime since = null;
            string[] actual;
            actual = target.GetActivityStateIds(activityId, actor, registrationId, since);
            Console.Write(converter.SerializeToJSON(actual));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for SaveActivityState
        ///</summary>
        [Test]
        [Ignore]
        public void SaveActivityStateTest()
        {
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            ActivityState activityState = new ActivityState();
            activityState.ActivityId = "example.com";
            activityState.Actor = new Actor("Example", "mailto:test@example.com");
            activityState.Body = "This is a test input.";
            activityState.StateId = "Bananas";
            bool overwrite = false;
            ActivityState previousState = null;
            target.SaveActivityState(activityState, overwrite, previousState);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for GetActivityState
        ///</summary>
        [Test]
        [Ignore]
        public void GetActivityStateTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Example", "mailto:test@example.com");
            string activityId = "example.com";
            string registrationId = null;
            string stateId = "Bananas";
            ActivityState actual;
            actual = target.GetActivityState(activityId, actor, stateId, registrationId);
            Console.Write(converter.SerializeToJSON(actual));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for DeleteActivityState
        ///</summary>
        [Test]
        [Ignore]
        public void DeleteActivityStateTest()
        {
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Example", "mailto:test@example.com");
            string activityId = "example.com";
            string registrationId = null;
            string stateId = "Bananas";
            target.DeleteActivityState(activityId, actor, stateId, registrationId);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for SaveActivityProfile
        ///</summary>
        [Test]
        [Ignore]
        public void SaveActivityProfileTest()
        {
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            ActivityProfile profile = new ActivityProfile();
            profile.ProfileId = "Bananas";
            profile.ActivityId = "example.com";
            profile.Body = "These are contents";
            bool overwrite = false;
            ActivityProfile previous = null;
            target.SaveActivityProfile(profile, overwrite, previous);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for GetActivityProfile
        ///</summary>
        [Test]
        [Ignore]
        public void GetActivityProfileTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            string activityId = "example.com";
            string profileId = "Bananas";
            ActivityProfile actual;
            actual = target.GetActivityProfile(activityId, profileId);
            Console.Write(converter.SerializeToJSON(actual));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for GetActivityProfileIds
        ///</summary>
        [Test]
        [Ignore]
        public void GetActivityProfileIdsTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            string activityId = "example.com";
            NullableDateTime since = null;
            string[] actual;
            actual = target.GetActivityProfileIds(activityId, since);
            Console.Write(converter.SerializeToJSON(actual));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for DeleteActivityProfile
        ///</summary>
        [Test]
        [Ignore]
        public void DeleteActivityProfileTest()
        {
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            string activityId = "example.com";
            string profileId = "Bananas";
            target.DeleteActivityProfile(activityId, profileId);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for DeleteAllActivityProfile
        ///</summary>
        [Test]
        [Ignore]
        public void DeleteAllActivityProfileTest()
        {
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            string activityId = "example.com";
            target.DeleteAllActivityProfile(activityId);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for GetActivity
        ///</summary>
        [Test]
        [Ignore]
        public void GetActivityTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("test", "password"));
            string activityId = "example.com"; // TODO: Initialize to an appropriate value
            Activity actual;
            actual = target.GetActivity(activityId);
            Console.Write(converter.SerializeToJSON(actual));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }
        #endregion

        #region Joint Unit Tests (with meaningful Asserts!)
        /// <summary>
        ///A test for StoreStatements
        ///</summary>
        [Test]
        [Ignore]
        public void StoreStatementsAsyncTest()
        {
            TCAPI target = new TCAPI(new Uri("https://cloud.scorm.com/tc/CZSWMUZPSE"), new BasicHTTPAuth("CZSWMUZPSE", "vwiuflgsY22FDXpHA4lwwe5hrnUXvcyJjW3fDrpH"), new TCAPICallback(), new OfflineStorage(), 750, 2);
            Statement[] statements = new Statement[6];
            for (int i = 0; i < statements.Length; i++)
            {
                int j = i % 3;
                switch (j)
                {
                    case 0:
                        statements[i] = new Statement(new Actor("Mufasa", "mailto:Mufasa@gmail.com"), new StatementVerb(PredefinedVerbs.Experienced), new TinCanActivity("test activity"));
                        break;
                    case 1:
                        statements[i] = new Statement(new Actor("Carl", "mailto:carl@example.co.uk"), new StatementVerb(PredefinedVerbs.Experienced), new TinCanActivity("TinCanClientLibrary"));
                        break;
                    case 2:
                        statements[i] = new Statement(new Actor("DiBiase", "mailto:DiBiase@notarealbanana.sup"), new StatementVerb(PredefinedVerbs.Experienced), new TinCanActivity("test activity"));
                        break;
                }
            }   
            target.StoreStatements(statements, false);
            //target.Flush();
            Statement[] statementSize;
            while ((statementSize = target.OfflineStorage.GetQueuedStatements(1)) != null
                && (statementSize.Length > 0))
            {
                Thread.Sleep(500);
                Console.WriteLine("Waiting");
            }
            target.Dispose(); // Releases the thread timer handle
            // If all statements successfully flush out of the buffer AND no exceptions are thrown (which should repopulate the buffer)
            // then the test was successful and the final statementSize should be null, indicating an empty queue.
            // The default implementation returns a null object.  However you may also return a size-0 array, all checks ensure both.
            Assert.IsTrue(statementSize == null || statementSize.Length == 0);
        }

        /// <summary>
        /// A test for pushing and storing Actor Profiles, then deleting them.
        /// </summary>
        /// <remarks>This test should use a dummy actor, not a real one!</remarks>
        [Test]
        [Ignore]
        public void ActorProfileTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("CZSWMUZPSE", "vwiuflgsY22FDXpHA4lwwe5hrnUXvcyJjW3fDrpH"));
            Actor actor = new Actor("Mufasa", "mailto:mufasa@gmail.com");

            string[] profileIds = { "The Lion King", "The Fallen King", "The New King" };
            string[] profileContents = { 
                "Mufasa rules his country as a proud and fair king of lions, celebrating his recently newborn son Simba.",
                "Scar kills Mufasa, simply muttering the words 'Long Live the King'", 
                "Simba finally realizes he must follow in his fathers footsteps to save the kingdom from the evil Scar." };

            // Clear all existing profiles.
            target.DeleteAllActorProfile(actor);

            NullableDateTime since = null;
            string[] actual;
            actual = target.GetActorProfileIds(actor, since);

            Assert.AreEqual(0, actual.Length);

            /* Save a new actor profile */
            ActorProfile p1 = new ActorProfile();
            p1.Actor = actor;
            p1.ProfileId = profileIds[0];
            p1.Body = profileContents[0];
            ActorProfile pp = new ActorProfile();
            pp.ProfileId = profileIds[0];
            pp.Actor = actor;
            pp.Body = profileContents[0];
            target.SaveActorProfile(p1, pp, true);
            actual = target.GetActorProfileIds(actor, since);

            Assert.AreEqual(1, actual.Length);

            p1.ProfileId = profileIds[1];
            p1.Body = profileContents[1];
            pp.ProfileId = profileIds[1];
            target.SaveActorProfile(p1, pp, true);
            actual = target.GetActorProfileIds(actor, since);

            Assert.AreEqual(2, actual.Length);

            p1.ProfileId = profileIds[2];
            p1.Body = profileContents[2];
            pp.ProfileId = profileIds[2];
            target.SaveActorProfile(p1, pp, true);
            actual = target.GetActorProfileIds(actor);

            Assert.AreEqual(3, actual.Length);

            // Ensure all the posted data matches

            ActorProfile pResult = target.GetActorProfile(actor, profileIds[0]);
            Assert.AreEqual(profileContents[0], pResult.Body);

            pResult = target.GetActorProfile(actor, profileIds[1]);
            Assert.AreEqual(profileContents[1], pResult.Body);

            pResult = target.GetActorProfile(actor, profileIds[2]);
            Assert.AreEqual(profileContents[2], pResult.Body);

            target.DeleteActorProfile(actor, profileIds[0]);
            actual = target.GetActorProfileIds(actor);

            Assert.AreEqual(2, actual.Length);

            target.DeleteAllActorProfile(actor);
            actual = target.GetActorProfileIds(actor);

            Assert.AreEqual(0, actual.Length);
        }

        /// <summary>
        /// Tests all the methods associated with Activity State
        /// </summary>
        /// <remarks>Again, use a dummy activity, not a real one.</remarks>
        [Test]
        [Ignore]
        public void ActivityStateTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("CZSWMUZPSE", "vwiuflgsY22FDXpHA4lwwe5hrnUXvcyJjW3fDrpH"));
            Actor actor = new Actor("Mufasa", "mailto:mufasa@gmail.com");
            string[] stateIds = { "The Lion King", "The Fallen King", "The New King" };
            string[] stateContents = {
                "Mufasa rules his country as a proud and fair king of lions, celebrating his recently newborn son Simba.",
                "Scar kills Mufasa, simply muttering the words 'Long Live the King'", 
                "Simba finally realizes he must follow in his fathers footsteps to save the kingdom from the evil Scar." };

            string activityId = "example.com/TheLionKing";
            string[] results = target.GetActivityStateIds(activityId, actor);

            //Assert.AreEqual(0, results.Length);

            ActivityState state = new ActivityState();
            state.ActivityId = activityId;
            state.Actor = actor;
            state.StateId = stateIds[0];
            state.Body = stateContents[0];

            ActivityState previous = new ActivityState();
            previous.ActivityId = activityId;
            previous.Actor = actor;
            previous.StateId = stateIds[0];
            previous.Body = stateContents[0];

            target.SaveActivityState(state, false, previous);

            //target.SaveActivityState(state);

            state.StateId = stateIds[1];
            state.Body = stateContents[1];
            target.SaveActivityState(state);

            state.StateId = stateIds[2];
            state.Body = stateContents[2];
            target.SaveActivityState(state);

            results = target.GetActivityStateIds(activityId, actor);

            Assert.AreEqual(3, results.Length);

            ActivityState asResult = target.GetActivityState(activityId, actor, stateIds[0]);
            Assert.AreEqual(stateContents[0], asResult.Body);
            asResult = target.GetActivityState(activityId, actor, stateIds[0]);

            asResult = target.GetActivityState(activityId, actor, stateIds[1]);
            Assert.AreEqual(stateContents[1], asResult.Body);

            asResult = target.GetActivityState(activityId, actor, stateIds[2]);
            Assert.AreEqual(stateContents[2], asResult.Body);

            /*
            target.DeleteActivityState(activityId, actor, stateIds[0]);
            results = target.GetActivityStateIds(activityId, actor);

            Assert.AreEqual(2, results.Length);
            target.DeleteActivityState(activityId, actor, stateIds[1]);
            target.DeleteActivityState(activityId, actor, stateIds[2]);
            */
            results = target.GetActivityStateIds(activityId, actor);
        }

        /// <summary>
        /// Tests all the methods associated with Activity Profile
        /// </summary>
        [Test]
        public void ActivityProfileTest()
        {
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("CZSWMUZPSE", "vwiuflgsY22FDXpHA4lwwe5hrnUXvcyJjW3fDrpH"));
            Actor actor = new Actor("Mufasa", "mailto:mufasa@gmail.com");
            string[] profileIds = { "The Lion King", "The Fallen King", "The New King" };
            string[] profileContents = {
                "Mufasa rules his country as a proud and fair king of lions, celebrating his recently newborn son Simba.",
                "Scar kills Mufasa, simply muttering the words 'Long Live the King'", 
                "Simba finally realizes he must follow in his fathers footsteps to save the kingdom from the evil Scar." };
            string activityId = "\"example.com/TheLionKing\"";
            string[] actual;
            actual = target.GetActivityProfileIds(activityId);

            //Assert.AreEqual(0, actual.Length);

            ActivityProfile profile = new ActivityProfile();
            profile.ActivityId = activityId;
            profile.ProfileId = profileIds[0];
            profile.Body = profileContents[0];
            ActivityProfile pp = new ActivityProfile();
            pp.ActivityId = activityId;
            pp.ProfileId = profileIds[0];
            pp.Body = profileContents[0];

            target.SaveActivityProfile(profile, true, pp);

            profile.ProfileId = profileIds[1];
            profile.Body = profileContents[1];

            target.SaveActivityProfile(profile);

            profile.ProfileId = profileIds[2];
            profile.Body = profileContents[2];

            target.SaveActivityProfile(profile);

            /*
            ActivityProfile previous = new ActivityProfile();
            previous.ProfileId = profileIds[2];
            previous.Body = profileContents[1];

            target.SaveActivityProfile(profile, false, previous);
            */

            actual = target.GetActivityProfileIds(activityId);
            Assert.AreEqual(3, actual.Length);

            ActivityProfile apResult = target.GetActivityProfile(activityId, profileIds[0]);
            Assert.AreEqual(profileContents[0], apResult.Body);

            apResult = target.GetActivityProfile(activityId, profileIds[1]);
            Assert.AreEqual(profileContents[1], apResult.Body);

            apResult = target.GetActivityProfile(activityId, profileIds[2]);
            Assert.AreEqual(profileContents[2], apResult.Body);

            target.DeleteActivityProfile(activityId, profileIds[0]);
            actual = target.GetActivityProfileIds(activityId);
            Assert.AreEqual(2, actual.Length);

            target.DeleteAllActivityProfile(activityId);
            actual = target.GetActivityProfileIds(activityId);
            Assert.AreEqual(0, actual.Length);
        }

        /// <summary>
        /// Test to ensure ETag collisions are not ignored.
        /// </summary>
        [Test]
        [Ignore]
        public void CollisionTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI(new Uri("http://cloud.scorm.com/tc/public"), new BasicHTTPAuth("CZSWMUZPSE", "vwiuflgsY22FDXpHA4lwwe5hrnUXvcyJjW3fDrpH"));
            Actor actor = new Actor("Mufasa", "mailto:mufasa@gmail.com");
            string[] stateIds = { "The Lion King", "The Fallen King", "The New King" };
            string[] stateContents = {
                "Mufasa rules his country as a proud and fair king of lions, celebrating his recently newborn son Simba.",
                "Scar kills Mufasa, simply muttering the words 'Long Live the King'", 
                "Simba finally realizes he must follow in his fathers footsteps to save the kingdom from the evil Scar." };

            string activityId = "example.com/TheLionKing";
            string[] results = target.GetActivityStateIds(activityId, actor);

            ActivityState state = new ActivityState(activityId, stateIds[0], actor, stateContents[1], "text/plain");
            ActivityState previous = new ActivityState(activityId, stateIds[0], actor, stateContents[0], "text/plain");
            target.SaveActivityState(state);
            state.Body = stateContents[2];
            target.SaveActivityState(state, false, previous);
        }
        #endregion
    }
}