using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TinCanAPIUnitTest
{
    [TestFixture]
    public class TestFixture1
    {
        [Test]
        public void TestTrue()
        {
            Assert.IsTrue(true);
        }

        // This test fail for example, replace result or delete this test to see all tests pass
        [Test]
        public void TestFault()
        {
            Assert.IsTrue(false);
        }
    }
}
