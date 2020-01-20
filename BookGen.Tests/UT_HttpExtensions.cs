//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework.Server;
using NUnit.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_HttpExtensions
    {
        [Test]
        public void EnsureThat_ParseQueryParameters_ParsesCorrectly()
        {
            var result = "?foo=bar&foo1=bar1&foo2=bar2".ParseQueryParameters();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("bar", result["foo"]);
            Assert.AreEqual("bar1", result["foo1"]);
            Assert.AreEqual("bar2", result["foo2"]);
        }

        [Test]
        public void EnsureThat_ParseQueryParameters_HandlesBase64()
        {
            var result = "?foo=YmFyZQ==".ParseQueryParameters();
            Assert.AreEqual("YmFyZQ==", result["foo"]);
        }

    }
}
