using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;
using RusticiSoftware.TinCanAPILibrary.Exceptions;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class StatementFixture
    {

        private Statement statement;

        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void TearDown()
        {
            statement = null;
        }

        [Test]
        public void Empty_constructor_does_not_throw_exceptions()
        {
            Assert.DoesNotThrow(() => new Statement());
        }

        [Test]
        public void Parameterized_constructor_does_not_throw_exceptions()
        {
            Assert.DoesNotThrow(() => new Statement(new Actor(), new StatementVerb(), new StatementTarget()));
        }

        [Test]
        public void Predefined_verb_constructor_does_not_throw_exceptions()
        {
            Assert.DoesNotThrow(() => new Statement(new Actor(), PredefinedVerbs.Completed, new StatementTarget()));
        }

        [Test]
        public void Id_setter_throws_exception_for_non_uuid_string()
        {
            statement = new Statement();
            Assert.Throws<InvalidArgumentException>(() => statement.Id = "Not a proper UUID");
        }
    }
}
