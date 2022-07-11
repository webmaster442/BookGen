//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_MarkdownTableConverter
    {
        [Test]
        public void MarkdownTableConverter_TryConvertToMarkdownTable_ReturnsGood()
        {
            string input = "Test\tTable\r\n" +
                           "String\tInput";

            string expected = "| Test   | Table |\r\n" +
                              "| :----- | :---- |\r\n" +
                              "| String | Input |\r\n";

            bool result = MarkdownTableConverter.TryConvertToMarkdownTable(input, '\t', out string resultTable);

            Assert.IsTrue(result);
            Assert.AreEqual(expected, resultTable);

        }
    }
}
