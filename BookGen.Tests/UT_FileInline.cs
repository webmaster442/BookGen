//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Template.ShortCodeImplementations;
using BookGen.Tests.Environment;
using NUnit.Framework;
using System.Collections.Generic;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_InlineFile
    {

        [Test]
        public void EnsureThat_InlineFile_ReturnsCorrectString()
        {
            var sut = new InlineFile(TestEnvironment.GetMockedLog());
            var arguments = new Dictionary<string, string>
            {
                { "File", TestEnvironment.GetFile("TestFile.txt") }
            };
            var result = sut.Generate(arguments);
            Assert.AreEqual("Test", result);
        }

        [Test]
        public void EnsureThat_InlineFile_Tag_MatchesClassName()
        {
            var sut = new InlineFile(TestEnvironment.GetMockedLog());

            const string expected = nameof(InlineFile);

            Assert.AreEqual(expected, sut.Tag);
        }
    }
}
