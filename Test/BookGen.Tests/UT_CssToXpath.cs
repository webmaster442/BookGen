//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Css;

namespace BookGen.Tests
{
    [TestFixture]
    internal class UT_CssToXpath
    {
        private CssToXpath _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new CssToXpath();
        }

        [TestCase("*", "//*")]
        [TestCase("p", "//p")]
        [TestCase("p > *", "//p/*")]
        [TestCase("#foo", "//*[@id='foo']")]
        [TestCase("*[title]", "//*[@title]")]
        [TestCase(".bar", "//*[contains(concat(' ',normalize-space(@class),' '),' bar ')]")]
        [TestCase("div#test .note span:first-child", "//div[@id='test']//*[contains(concat(' ',normalize-space(@class),' '),' note ')]//*[1]/self::span")]
        public void TransformTest(string input, string expected)
        {
            Assert.That(_sut.Transform(input), Is.EqualTo(expected));
        }
    }
}
