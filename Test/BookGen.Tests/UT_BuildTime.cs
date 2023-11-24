//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_BuildTime
    {
        private TimeProvider _timeProvider;

        [SetUp]
        public void Setup()
        {
            _timeProvider = Substitute.For<TimeProvider>();
        }

        [Test]
        public void EnsureThat_BuildTime_ReturnsCorrectString()
        {
            var sut = new BuildTime(_timeProvider);

            string expected = DateTime.Now.ToString("yy-MM-dd hh:mm:ss");

            string result = sut.Generate(null);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void EnsureThat_BuildTime_Tag_MatchesClassName()
        {
            var sut = new BuildTime(_timeProvider);

            const string expected = nameof(BuildTime);

            Assert.That(sut.Tag, Is.EqualTo(expected));
        }
    }
}
