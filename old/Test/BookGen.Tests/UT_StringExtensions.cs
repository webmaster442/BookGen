//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
            string actual = input.ToTitleCase(new System.Globalization.CultureInfo("en-US"));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("this is fine", 3)]
        public void EnsureThat_GetWordCount_ReturnsExpected(string input, int expected)
        {
            int actual = input.GetWordCount();
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
