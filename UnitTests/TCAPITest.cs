using RusticiSoftware.TinCanAPILibrary.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using RusticiSoftware.TinCanAPILibrary;
using System.Net;
using System.IO;
using RusticiSoftware.TinCanAPILibrary.Helper;
using System.Text;

namespace UnitTests
{
    /// <summary>
    ///This is a test class for TCAPITest and is intended
    ///to contain all TCAPITest Unit Tests
    ///</summary>
    [TestClass()]
    public class TCAPITest
    {


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

        /// <summary>
        ///A test for StoreStatement
        ///</summary>
        ///
        //[TestMethod()]
        public void StoreStatementTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            Statement[] statements = new Statement[3];
            statements[0] = new Statement(new Actor("Jaffer", "mailto:akintundex@gmail.com"), StatementVerb.Experienced, new TinCanActivity("test activity"));
            statements[1] = new Statement(new Actor("Abraham", "mailto:abraham@example.co.uk"), StatementVerb.Experienced, new TinCanActivity("TinCanClientLibrary"));
            statements[2] = new Statement(new Actor("DaBoss", "mailto:wutwut@notarealbanana.sup"), StatementVerb.Experienced, new TinCanActivity("test activity"));
            target.StoreStatements(statements);
            Console.WriteLine();
        }

        /// <summary>
        ///A test for VoidStatements
        ///</summary>
        // [TestMethod()]
        public void VoidStatementsTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            target.AdminActor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            string[] statementIdsToVoid = { "a84cc4d6-69ee-4eb5-ac4c-d3d6a5077070" }; // TODO: Initialize to an appropriate value
            target.VoidStatements(statementIdsToVoid);
        }

        /// <summary>
        ///A test for GetStatement
        ///</summary>
        //[TestMethod()]
        public void GetStatementTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            Statement actual;
            actual = target.GetStatement("b6cc80c6-6fb3-47d0-a311-d032a86435e2");
            Console.Write(actual.ToString());
        }

        /// <summary>
        ///A test for GetStatements
        ///</summary>
        //[TestMethod()]
        public void GetStatementsTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            StatementQueryObject queryObject = new StatementQueryObject(); // TODO: Initialize to an appropriate value
            StatementResult actual;
            actual = target.GetStatements(queryObject);
            Console.Write(actual);
            /*
            while (!String.IsNullOrEmpty(actual.More))
            {
                actual = target.GetStatements(actual.More);
                Console.Write(actual);
            }
             * */
        }

        /// <summary>
        ///A test for SaveActorProfile
        ///</summary>
        //[TestMethod()]
        public void SaveActorProfileTest()
        {
            TinCanJsonConverter converter = new TinCanJsonConverter();
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            ActorProfile actorProfile = new ActorProfile();
            actorProfile.Actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            actorProfile.ProfileId = "Jaffer";
            actorProfile.Contents = "This is some test";
            ActorProfile previousProfile = new ActorProfile();
            previousProfile.Actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            previousProfile.ProfileId = "Jaffer";
            bool overwrite = true; // TODO: Initialize to an appropriate value
            target.SaveActorProfile(actorProfile, previousProfile, overwrite);
        }

        /// <summary>
        ///A test for GetActorProfile
        ///</summary>
        //[TestMethod()]
        public void GetActorProfileTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com"); // TODO: Initialize to an appropriate value
            string profileId = "Jaffer"; // TODO: Initialize to an appropriate value
            ActorProfile actual;
            actual = target.GetActorProfile(actor, profileId);
            Console.Write(actual);
        }

        /// <summary>
        ///A test for GetActor
        ///</summary>
        //[TestMethod()]
        public void GetActorTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            Actor partialActor = new Actor();
            partialActor.Mbox = new String[]{"mailto:akintundex@gmail.com"};
            Actor fullActor = target.GetActor(partialActor);
            Console.Write(fullActor);
        }

        /// <summary>
        ///A test for GetActorProfileIds
        ///</summary>
        //[TestMethod()]
        public void GetActorProfileIdsTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com"); // TODO: Initialize to an appropriate value
            NullableDateTime since = null; // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.GetActorProfileIds(actor, since);
            Console.Write(actual);
        }

        /// <summary>
        ///A test for DeleteActorProfile
        ///</summary>
        //[TestMethod()]
        public void DeleteActorProfileTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com"); // TODO: Initialize to an appropriate value
            string profileId = "Jaffer"; // TODO: Initialize to an appropriate value
            target.DeleteActorProfile(actor, profileId);
        }

        /// <summary>
        ///A test for DeleteAllActorProfile
        ///</summary>
        //[TestMethod()]
        public void DeleteAllActorProfileTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com"); // TODO: Initialize to an appropriate value
            target.DeleteAllActorProfile(actor);
        }

        /// <summary>
        ///A test for GetActivityStateIds
        ///</summary>
        //[TestMethod()]
        public void GetActivityStateIdsTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com"); // TODO: Initialize to an appropriate value
            string activityId = "example.com";
            string registrationId = null; // TODO: Initialize to an appropriate value
            NullableDateTime since = null; // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.GetActivityStateIds(activityId, actor, registrationId, since);
            Console.Write(actual);
        }
        
        /// <summary>
        ///A test for SaveActivityState
        ///</summary>
        //[TestMethod()]
        public void SaveActivityStateTest()
        {   
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            ActivityState activityState = new ActivityState();
            activityState.ActivityId = "example.com";
            activityState.Actor = new Actor("Jaffer", "mailto:akintundex@gmail.com");
            activityState.Contents = "This is a test input.";
            activityState.StateId = "Bananas";
            bool overwrite = false; // TODO: Initialize to an appropriate value
            ActivityState previousState = null; // TODO: Initialize to an appropriate value
            target.SaveActivityState(activityState, overwrite, previousState);
        }

        /// <summary>
        ///A test for GetActivityState
        ///</summary>
        //[TestMethod()]
        public void GetActivityStateTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com"); // TODO: Initialize to an appropriate value
            string activityId = "example.com";
            string registrationId = null; // TODO: Initialize to an appropriate value
            string stateId = "Bananas";
            ActivityState actual;
            actual = target.GetActivityState(activityId, actor, stateId, registrationId);
            Console.Write(actual);
        }

        /// <summary>
        ///A test for DeleteActivityState
        ///</summary>
        //[TestMethod()]
        public void DeleteActivityStateTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            Actor actor = new Actor("Jaffer", "mailto:akintundex@gmail.com"); // TODO: Initialize to an appropriate value
            string activityId = "example.com";
            string registrationId = null; // TODO: Initialize to an appropriate value
            string stateId = "Bananas";
            target.DeleteActivityState(activityId, actor, stateId, registrationId);
        }

        /// <summary>
        ///A test for StoreStatements
        ///</summary>
        //[TestMethod()]
        public void StoreStatementsAsyncTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            target.TCAPICallback = new TCAPICallback();
            Statement[] statements = new Statement[60];
            for (int i = 0; i < statements.Length; i++)
            {
                int j = i % 3;
                switch (j)
                {
                    case 0:
                        statements[i] = new Statement(new Actor("Jaffer", "mailto:akintundex@gmail.com"), StatementVerb.Experienced, new TinCanActivity("test activity"));
                        break;
                    case 1:
                        statements[i] = new Statement(new Actor("Abraham", "mailto:abraham@example.co.uk"), StatementVerb.Experienced, new TinCanActivity("TinCanClientLibrary"));
                        break;
                    case 2:
                        statements[i] = new Statement(new Actor("DaBoss", "mailto:wutwut@notarealbanana.sup"), StatementVerb.Experienced, new TinCanActivity("test activity"));
                        break;
                }
            }
            target.StoreStatements(statements, false);
            Console.Write("Hi");
        }

        /// <summary>
        /// A test for StoreStatements
        /// This method takes a long time to run.
        ///</summary>
        //[TestMethod()]
        public void StoreStatementsConnectionCloseTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            target.TCAPICallback = new TCAPICallback();
            Statement statement = null;
            for (int i = 0; i < 300; i++)
            {
                int j = i % 3;
                switch (j)
                {
                    case 0:
                        statement = new Statement(new Actor("Jaffer", "mailto:akintundex@gmail.com"), StatementVerb.Experienced, new TinCanActivity("test activity"));
                        break;
                    case 1:
                        statement = new Statement(new Actor("Abraham", "mailto:abraham@example.co.uk"), StatementVerb.Experienced, new TinCanActivity("TinCanClientLibrary"));
                        break;
                    case 2:
                        statement = new Statement(new Actor("DaBoss", "mailto:wutwut@notarealbanana.sup"), StatementVerb.Experienced, new TinCanActivity("test activity"));
                        break;
                }
                target.StoreStatement(statement, false);
            }
            Console.Write("Hi");
        }

        /// <summary>
        ///A test for SaveActivityProfile
        ///</summary>
        //[TestMethod()]
        public void SaveActivityProfileTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            ActivityProfile profile = new ActivityProfile(); // TODO: Initialize to an appropriate value
            profile.ProfileId = "Bananas";
            profile.ActivityId = "example.com";
            profile.Contents = "These are contents";
            bool overwrite = false; // TODO: Initialize to an appropriate value
            ActivityProfile previous = null; // TODO: Initialize to an appropriate value
            target.SaveActivityProfile(profile, overwrite, previous);
        }

        /// <summary>
        ///A test for GetActivityProfile
        ///</summary>
        //[TestMethod()]
        public void GetActivityProfileTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            string activityId = "example.com";
            string profileId = "Bananas";
            ActivityProfile actual;
            actual = target.GetActivityProfile(activityId, profileId);
            Console.Write(actual);
        }

        /// <summary>
        ///A test for GetActivityProfileIds
        ///</summary>
        //[TestMethod()]
        public void GetActivityProfileIdsTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            string activityId = "example.com";
            NullableDateTime since = null;
            string[] actual;
            actual = target.GetActivityProfileIds(activityId, since);
            Console.Write(actual);
        }

        /// <summary>
        ///A test for DeleteActivityProfile
        ///</summary>
        //[TestMethod()]
        public void DeleteActivityProfileTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            string activityId = "example.com"; // TODO: Initialize to an appropriate value
            string profileId = "Bananas"; // TODO: Initialize to an appropriate value
            target.DeleteActivityProfile(activityId, profileId);
        }

        /// <summary>
        ///A test for DeleteAllActivityProfile
        ///</summary>
        //[TestMethod()]
        public void DeleteAllActivityProfileTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            string activityId = "example.com";
            target.DeleteAllActivityProfile(activityId);
        }

        /// <summary>
        ///A test for GetActivity
        ///</summary>
        //[TestMethod()]
        public void GetActivityTest()
        {
            TCAPI target = new TCAPI();
            target.Authentification = new BasicHTTPAuth("test", "password");
            target.Endpoint = "http://cloud.scorm.com/ScormEngineInterface/TCAPI/public";
            string activityId = "http://scorm.com/pong/beatTj"; // TODO: Initialize to an appropriate value
            Activity actual;
            actual = target.GetActivity(activityId);
            Console.Write(actual);
        }
    }
}
    