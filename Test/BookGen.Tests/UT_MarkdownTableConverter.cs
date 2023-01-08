//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
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
            const string input = "Test\tTable\r\n" +
                           "String\tInput";

            const string expected = "| Test   | Table |\r\n" +
                                    "| :----- | :---- |\r\n" +
                                    "| String | Input |\r\n";

            bool result = MarkdownTableConverter.TryConvertToMarkdownTable(input, '\t', out string resultTable);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(resultTable, Is.EqualTo(expected));
            });
        }
    }
}
