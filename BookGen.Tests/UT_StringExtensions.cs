using BookGen.Utilities;
using NUnit.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    internal class UT_StringExtensions
    {
        [TestCase("foo", "Foo")]
        [TestCase("BaR", "Bar")]
        [TestCase("USD", "USD")]
        [TestCase("this is fine", "This Is Fine")]
        public void EnsureThat_ToTitleCase_ReturnsExpected(string input, string expected)
        {
            var actual = input.ToTitleCase(new System.Globalization.CultureInfo("en-US"));

            Assert.AreEqual(expected, actual);
        }

        [TestCase("this is fine", 3)]
        public void EnsureThat_GetWordCount_ReturnsExpected(string input, int expected)
        {
            var actual = input.GetWordCount();
            Assert.AreEqual(expected, actual);
        }
    }
}
