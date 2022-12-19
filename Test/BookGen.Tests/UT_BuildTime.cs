//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_BuildTime
    {
        [Test]
        public void EnsureThat_BuildTime_ReturnsCorrectString()
        {
            var sut = new BuildTime();

            string expected = DateTime.Now.ToString("yy-MM-dd hh:mm:ss");

            string result = sut.Generate(null);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void EnsureThat_BuildTime_Tag_MatchesClassName()
        {
            var sut = new BuildTime();

            const string expected = nameof(BuildTime);

            Assert.That(sut.Tag, Is.EqualTo(expected));
        }
    }
}
