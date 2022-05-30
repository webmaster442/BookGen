//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Utilities;

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
            var result = MarkdownUtils.GetDocumentTitle(input, TestEnvironment.GetMockedLog());
            Assert.AreEqual(expected, result);
        }

    }
}
