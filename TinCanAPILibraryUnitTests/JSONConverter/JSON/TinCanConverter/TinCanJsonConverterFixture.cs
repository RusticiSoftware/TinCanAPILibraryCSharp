using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.JSONConverter.JSON.TinCanConverter
{
    [TestFixture]
    public class TinCanJsonConverterFixture
    {
        private TinCanJsonConverter converter;

        [SetUp]
        public void SetUp()
        {
            converter = new TinCanJsonConverter();
        }

        [TearDown]
        public void TearDown()
        {
            converter = null;
        }

        [Test]
        public void Simple_statement_deserialized_fully()
        {
            var source = @"{
                'id': '12345678-1234-5678-1234-567812345678',
                'actor':{
                    'mbox':'mailto:xapi@adlnet.gov'
                },
                'verb':{
                    'id':'http://adlnet.gov/expapi/verbs/created',
                    'display':{
                        'en-US':'created'
                    }
                },
                'object':{
                    'id':'http://example.adlnet.gov/xapi/example/activity'
                }
            }";
            Statement statement = converter.DeserializeJSON(source, typeof(Statement)) as Statement;
            Assert.NotNull(statement);
            Assert.AreEqual("12345678-1234-5678-1234-567812345678", statement.Id);
            Assert.NotNull(statement.Actor);
            Assert.AreEqual("mailto:xapi@adlnet.gov", statement.Actor.Mbox);
            Assert.NotNull(statement.Verb);
            Assert.AreEqual("http://adlnet.gov/expapi/verbs/created", statement.Verb.Id);
            Assert.NotNull(statement.Verb.Display);
            Assert.AreEqual(1, statement.Verb.Display.Count);
            Assert.AreEqual("created", statement.Verb.Display["en-US"]);
            Assert.NotNull(statement.Object);
            Assert.IsInstanceOf<Activity>(statement.Object);
            Assert.AreEqual("http://example.adlnet.gov/xapi/example/activity", (statement.Object as Activity).Id);
        }
    }
}
