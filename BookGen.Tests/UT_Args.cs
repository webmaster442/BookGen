//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using NUnit.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_ArgumentList
    {
        [Test]
        public void TestArguments()
        {
            var arguments = new string[] { "-d", "directory", "-a", "Build" };
            var parsed = ArgumentList.Parse(arguments);

            Assert.AreEqual("build", parsed.GetArgument("a", "action").Value);
            Assert.AreEqual("directory", parsed.GetArgument("d", "dir").Value);
        }
    }
}
