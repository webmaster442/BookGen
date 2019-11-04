//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using NUnit.Framework;
using System.Linq;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_ArgumentParser
    {
        [Test]
        public void TestGetSwitchWithValue()
        {
            var arguments = new string[] { "-d", "directory", "-a", "Build" };
            var parsed = new ArgumentParser(arguments);

            Assert.AreEqual("build", parsed.GetSwitchWithValue("a", "action"));
            Assert.AreEqual("directory", parsed.GetSwitchWithValue("d", "dir"));
        }

        [Test]
        public void TestGetSwitch()
        {
            var arguments = new string[] { "-t", "--test" };
            var parsed = new ArgumentParser(arguments);

            Assert.IsTrue(parsed.GetSwitch("t", "--test"));
            Assert.IsFalse(parsed.GetSwitch("d", "dir"));
        }

        [Test]
        public void TestGetValues()
        {
            var arguments = new string[] { "foo", "bar" };
            var parsed = new ArgumentParser(arguments);

            var items = parsed.GetValues().ToList();

            Assert.AreEqual("foo", items[0]);
            Assert.AreEqual("bar", items[1]);
        }

    }
}
