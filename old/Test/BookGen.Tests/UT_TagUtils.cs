//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Tests
{
    [TestFixture]
    internal class UT_TagUtils
    {
        private TagUtils _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new TagUtils();
        }

        [TestCase("test", "test")]
        [TestCase("", "n-a")]
        [TestCase("c#", "csharp")]
        [TestCase("??", "questionquestion")]
        [TestCase("Árvíztűrő tükörfúrógép", "arvizturo-tukorfurogep")]
        public void EnshureThat_TagUtils(string input, string expected)
        {
            string actual = _sut.GetUrlNiceName(input);
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
