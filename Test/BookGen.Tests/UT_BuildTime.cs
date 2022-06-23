//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework.Shortcodes;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_BuildTime
    {
        [Test]
        public void EnsureThat_BuildTime_ReturnsCorrectString()
        {
            var sut = new BuildTime();

            var expected = DateTime.Now.ToString("yy-MM-dd hh:mm:ss");

            var result = sut.Generate(null);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EnsureThat_BuildTime_Tag_MatchesClassName()
        {
            var sut = new BuildTime();

            const string expected = nameof(BuildTime);

            Assert.AreEqual(expected, sut.Tag);
        }
    }
}
