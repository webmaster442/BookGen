//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_MarkdownUtils
    {
        [TestCase("# C# Test", "C# Test")]
        [TestCase("## C# Test", "C# Test")]
        [TestCase("### C# Test", "C# Test")]
        public void EnsureThat_MarkdownUtils_GetTitleWorksCorrectly(string input, string expected)
        {
            string result = MarkdownUtils.GetDocumentTitle(input, TestEnvironment.GetMockedLog());
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
