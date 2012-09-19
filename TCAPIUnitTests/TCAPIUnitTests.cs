using RusticiSoftware.TinCanAPILibrary.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using RusticiSoftware.TinCanAPILibrary;
using System.Net;
using System.IO;
using RusticiSoftware.TinCanAPILibrary.Helper;
using System.Text;
using System.Threading;

namespace UnitTests
{
    /// <summary>
    ///This is a test class for TCAPITest and is intended
    ///to contain all TCAPITest Unit Tests
    ///</summary>
    [TestClass()]
    public class TCAPITest
    {
        private const string INCONCLUSIVE = "The results of this test should be viewed on the LRS itself.";
        private const string INCONCLUSIVE_CONSOLE = "The expected results of this test should be compared to the console output.";

        private TestContext testContextInstance;
        public static TCAPITest instance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        public TCAPITest()
        {
            instance = this;
        }

        #region Individual Unit Tests (No meaningful Asserts)
        /// <summary>
        ///A test for StoreStatement
        ///</summary>
        ///
        //[TestMethod()]
        public void StoreStatementTest()
        {
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            Statement[] statements = new Statement[3];
            statements[0] = new Statement(new Actor("Jaffer", "mailto:akintundex@gmail.com"), StatementVerb.Experienced, new TinCanActivity("test activity"));
            statements[1] = new Statement(new Actor("Abraham", "mailto:abraham@example.co.uk"), StatementVerb.Experienced, new TinCanActivity("TinCanClientLibrary"));
            statements[2] = new Statement(new Actor("DaBoss", "mailto:wutwut@notarealbanana.sup"), StatementVerb.Experienced, new TinCanActivity("test activity"));
            target.StoreStatements(statements);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for VoidStatements
        ///</summary>
        // [TestMethod()]
        public void VoidStatementsTest()
        {
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            target.AdminActor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            string[] statementIdsToVoid = { "a84cc4d6-69ee-4eb5-ac4c-d3d6a5077070" };
            target.VoidStatements(statementIdsToVoid);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for GetStatement
        ///</summary>
        //[TestMethod()]
        public void GetStatementTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            Statement actual;
            actual = target.GetStatement("b6cc80c6-6fb3-47d0-a311-d032a86435e2");
            Console.Write(converter.SerializeToJSON(actual));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for GetStatements
        ///</summary>
        //[TestMethod()]
        public void GetStatementsTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            StatementQueryObject queryObject = new StatementQueryObject();
            queryObject.Actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            StatementResult actual;
            actual = target.GetStatements(queryObject);
            Console.Write(converter.SerializeToJSON(actual));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for SaveActorProfile
        ///</summary>
        //[TestMethod()]
        public void SaveActorProfileTest()
        {
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            ActorProfile actorProfile = new ActorProfile();
            actorProfile.Actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            actorProfile.ProfileId = "Jaffer";
            actorProfile.Contents = "This is some test";
            ActorProfile previousProfile = new ActorProfile();
            previousProfile.Actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            previousProfile.ProfileId = "Jaffer";
            bool overwrite = true;
            target.SaveActorProfile(actorProfile, previousProfile, overwrite);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for GetActorProfile
        ///</summary>
        //[TestMethod()]
        public void GetActorProfileTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            string profileId = "Jaffer";
            ActorProfile actual;
            actual = target.GetActorProfile(actor, profileId);
            Console.Write(converter.SerializeToJSON(actual));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for GetActor
        ///</summary>
        //[TestMethod()]
        public void GetActorTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            Actor partialActor = new Actor();
            partialActor.Mbox = new String[] { "mailto:akintundex@gmail.com" };
            Actor fullActor = target.GetActor(partialActor);
            Console.Write(converter.SerializeToJSON(fullActor));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for GetActorProfileIds
        ///</summary>
        //[TestMethod()]
        public void GetActorProfileIdsTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            NullableDateTime since = null;
            string[] actual;
            actual = target.GetActorProfileIds(actor, since);
            Console.Write(converter.SerializeToJSON(actual));
            Assert.Inconclusive(INCONCLUSIVE_CONSOLE);
        }

        /// <summary>
        ///A test for DeleteActorProfile
        ///</summary>
        //[TestMethod()]
        public void DeleteActorProfileTest()
        {
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            string profileId = "Jaffer";
            target.DeleteActorProfile(actor, profileId);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for DeleteAllActorProfile
        ///</summary>
        //[TestMethod()]
        public void DeleteAllActorProfileTest()
        {
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com"); // TODO: Initialize to an appropriate value
            target.DeleteAllActorProfile(actor);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for GetActivityStateIds
        ///</summary>
        //[TestMethod()]
        public void GetActivityStateIdsTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
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
        //[TestMethod()]
        public void SaveActivityStateTest()
        {
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            ActivityState activityState = new ActivityState();
            activityState.ActivityId = "example.com";
            activityState.Actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            activityState.Contents = "This is a test input.";
            activityState.StateId = "Bananas";
            bool overwrite = false;
            ActivityState previousState = null;
            target.SaveActivityState(activityState, overwrite, previousState);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for GetActivityState
        ///</summary>
        //[TestMethod()]
        public void GetActivityStateTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
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
        //[TestMethod()]
        public void DeleteActivityStateTest()
        {
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            string activityId = "example.com";
            string registrationId = null;
            string stateId = "Bananas";
            target.DeleteActivityState(activityId, actor, stateId, registrationId);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for SaveActivityProfile
        ///</summary>
        //[TestMethod()]
        public void SaveActivityProfileTest()
        {
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            ActivityProfile profile = new ActivityProfile();
            profile.ProfileId = "Bananas";
            profile.ActivityId = "example.com";
            profile.Contents = "These are contents";
            bool overwrite = false;
            ActivityProfile previous = null;
            target.SaveActivityProfile(profile, overwrite, previous);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for GetActivityProfile
        ///</summary>
        //[TestMethod()]
        public void GetActivityProfileTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
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
        //[TestMethod()]
        public void GetActivityProfileIdsTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
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
        //[TestMethod()]
        public void DeleteActivityProfileTest()
        {
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            string activityId = "example.com";
            string profileId = "Bananas";
            target.DeleteActivityProfile(activityId, profileId);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for DeleteAllActivityProfile
        ///</summary>
        //[TestMethod()]
        public void DeleteAllActivityProfileTest()
        {
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            string activityId = "example.com";
            target.DeleteAllActivityProfile(activityId);
            Assert.Inconclusive(INCONCLUSIVE);
        }

        /// <summary>
        ///A test for GetActivity
        ///</summary>
        //[TestMethod()]
        public void GetActivityTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI("http://cloud.scorm.com/ScormEngineInterface/TCAPI/public", new BasicHTTPAuth("test", "password"));
            string activityId = "http://scorm.com/pong/beatTj"; // TODO: Initialize to an appropriate value
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
        //[TestMethod()]
        public void StoreStatementsAsyncTest()
        {
            TCAPI target = new TCAPI("https://cloud.scorm.com/ScormEngineInterface/TCAPI/CZSWMUZPSE", new BasicHTTPAuth("CZSWMUZPSE", "vwiuflgsY22FDXpHA4lwwe5hrnUXvcyJjW3fDrpH"), new TCAPICallback(), new OfflineStorage(), 1000, 2);
            Statement[] statements = new Statement[6];
            for (int i = 0; i < statements.Length; i++)
            {
                int j = i % 3;
                switch (j)
                {
                    case 0:
                        statements[i] = new Statement(new Actor("Mufasa", "mailto:Mufasa@gmail.com"), StatementVerb.Experienced, new TinCanActivity("test activity"));
                        break;
                    case 1:
                        statements[i] = new Statement(new Actor("Carl", "mailto:carl@example.co.uk"), StatementVerb.Experienced, new TinCanActivity("TinCanClientLibrary"));
                        break;
                    case 2:
                        statements[i] = new Statement(new Actor("DiBiase", "mailto:DiBiase@notarealbanana.sup"), StatementVerb.Experienced, new TinCanActivity("test activity"));
                        break;
                }
            }
            target.StoreStatements(statements, false);
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
        [TestMethod()]
        public void ActorProfileTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI("https://cloud.scorm.com/ScormEngineInterface/TCAPI/CZSWMUZPSE", new BasicHTTPAuth("CZSWMUZPSE", "vwiuflgsY22FDXpHA4lwwe5hrnUXvcyJjW3fDrpH"));
            Actor actor = new Actor("Mufasa", "mailto:mufasa@gmail.com");

            String[] profileIds = { "The Lion King", "The Fallen King", "The New King" };
            String[] profileContents = { 
                "Mufasa rules his country as a proud and fair king of lions, celebrating his recently newborn son Simba.",
                "Scar kills Mufasa, simply muttering the words 'Long Live the King'", 
                "Simba finally realizes he must follow in his fathers footsteps to save the kingdom from the evil Scar." };

            // Clear all existing profiles.
            target.DeleteAllActorProfile(actor);

            NullableDateTime since = null;
            string[] actual;
            actual = target.GetActorProfileIds(actor, since);
            Assert.AreEqual(0, actual.Length); // Again, this test should be run on a fake actor.
            /* Save a new actor profile */
            ActorProfile p1 = new ActorProfile();
            p1.Actor = actor;
            p1.ProfileId = profileIds[0];
            p1.Contents = profileContents[0];
            ActorProfile pp = new ActorProfile();
            pp.ProfileId = profileIds[0];
            pp.Actor = actor;
            target.SaveActorProfile(p1, pp, true);
            actual = target.GetActorProfileIds(actor, since);
            Assert.AreEqual(1, actual.Length); // There should be just one profile for now.
            p1.ProfileId = profileIds[1];
            p1.Contents = profileContents[1];
            pp.ProfileId = profileIds[1];
            target.SaveActorProfile(p1, pp, true);
            actual = target.GetActorProfileIds(actor, since);
            Assert.AreEqual(2, actual.Length);
            p1.ProfileId = profileIds[2];
            p1.Contents = profileContents[2];
            pp.ProfileId = profileIds[2];
            target.SaveActorProfile(p1, pp, true);
            actual = target.GetActorProfileIds(actor);
            Assert.AreEqual(3, actual.Length);

            // Ensure all the posted data matches

            ActorProfile pResult = target.GetActorProfile(actor, profileIds[0]);
            Assert.AreEqual(profileContents[0], pResult.Contents);

            pResult = target.GetActorProfile(actor, profileIds[1]);
            Assert.AreEqual(profileContents[1], pResult.Contents);

            pResult = target.GetActorProfile(actor, profileIds[2]);
            Assert.AreEqual(profileContents[2], pResult.Contents);

            target.DeleteActorProfile(actor, profileIds[0]);
            actual = target.GetActorProfileIds(actor);
            Assert.AreEqual(2, actual.Length);
            target.DeleteAllActorProfile(actor);
            actual = target.GetActorProfileIds(actor);
            Assert.AreEqual(0, actual.Length);
        }
        #endregion
    }
}