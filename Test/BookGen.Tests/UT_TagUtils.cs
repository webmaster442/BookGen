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
            var actual = _sut.GetUrlNiceName(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
