//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using NUnit.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_FluentHtmlWriter
    {
        private FluentHtmlWriter _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new FluentHtmlWriter();
        }

        [TearDown]
        public void Teardown()
        {
            _sut = null;
        }

        [Test]
        public void EnsureThat_FluentHtmlWriter_WriteJavaScript_ReturnsCorrectOutput()
        {
            const string expected = "<script type=\"text/javascript\">alert(\"hello\");</script>\r\n";
            Assert.AreEqual(expected, _sut.WriteJavaScript("alert(\"hello\");").ToString());
        }

        [Test]
        public void EnsureThat_FluentHtmlWriter_WriteStyle_ReturnsCorrectOutput()
        {
            const string expected = "<style type=\"text/css\">foo</style>\r\n";
            Assert.AreEqual(expected, _sut.WriteStyle("foo").ToString());
        }

        [Test]
        public void EnsureThat_FluentHtmlWriter_WriteElement_Without_ContentAndTags_ReturnsCorrectOutput()
        {
            const string expected = "<br/>\r\n";
            Assert.AreEqual(expected, _sut.WriteElement(FluentHtmlWriter.Tags.Br).ToString());
        }


        [Test]
        public void EnsureThat_FluentHtmlWriter_WriteElement_WithoutTags_ReturnsCorrectOutput()
        {
            const string expected = "<p>test</p>\r\n";
            var result = _sut.WriteElement(FluentHtmlWriter.Tags.Paragraph, "test").ToString();
            Assert.AreEqual(expected, result);
        }
    }
}
