//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_BuildTime
    {
        private TimeProvider _timeProvider;
        private DateTimeOffset _expected;

        [SetUp]
        public void Setup()
        {
            _expected = new DateTimeOffset(new DateTime(1, 1, 1, 11, 11, 11));
            _timeProvider = Substitute.For<TimeProvider>();
            _timeProvider.LocalTimeZone.Returns(TimeZoneInfo.Utc);
            _timeProvider.GetUtcNow().Returns(_expected);

        }

        [Test]
        public void EnsureThat_BuildTime_ReturnsCorrectString()
        {
            var sut = new BuildTime(_timeProvider);

            string expected = _expected.ToString("yy-MM-dd hh:mm:ss");

            string result = sut.Generate(new ShortCodeArguments());

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
