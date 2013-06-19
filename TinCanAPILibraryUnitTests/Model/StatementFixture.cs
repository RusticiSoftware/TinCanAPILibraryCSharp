using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RusticiSoftware.TinCanAPILibrary.Model;

namespace TinCanAPILibraryUnitTests.Model
{
    [TestFixture]
    public class StatementFixture
    {

        private Statement statement;

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
            Assert.Throws<ArgumentException>(() => statement.Id = "Not a proper UUID");
        }

        [Test]
        public void Validate_returns_non_null_enumerable_with_failure_results_when_invalid()
        {
            statement = new Statement();
            IEnumerable<ValidationFailure> failures = statement.Validate(earlyReturnOnFailure : false);
            Assert.NotNull(failures);
            Assert.GreaterOrEqual(new List<ValidationFailure>(failures).Count, 1, "Expect several errors due to lack of supplied statement information");
        }

        [Test]
        public void Validate_returns_non_null_empty_enumerable_when_valid()
        {
            var activity = new Activity("http://www.example.com");
            activity.Definition = new ActivityDefinition();
            activity.Definition.Name = new LanguageMap();
            activity.Definition.Name.Add("en-US", "TCAPI C# 0.95 Library.");
            statement = new Statement(new Actor("Example", "mailto:test@example.com"), new StatementVerb(PredefinedVerbs.Experienced), activity);
            IEnumerable<ValidationFailure> failures = statement.Validate(earlyReturnOnFailure: false);
            Assert.NotNull(failures);
            Assert.AreEqual(new List<ValidationFailure>(failures).Count, 0);
        }
    }
}
