//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_InlineFile
    {
        [Test]
        public void EnsureThat_InlineFile_ReturnsCorrectString()
        {
            var sut = new InlineFile(TestEnvironment.GetMockedLog());
            var arguments = new ShortCodeArguments(new Dictionary<string, string>
            {
                { "File", TestEnvironment.GetFile("TestFile.txt") }
            });
            string result = sut.Generate(arguments);
            Assert.That(result, Is.EqualTo("Test"));
        }

        [Test]
        public void EnsureThat_InlineFile_Tag_MatchesClassName()
        {
            var sut = new InlineFile(TestEnvironment.GetMockedLog());

            const string expected = nameof(InlineFile);

            Assert.That(sut.Tag, Is.EqualTo(expected));
        }
    }
}
