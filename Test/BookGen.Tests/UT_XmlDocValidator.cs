//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.AssemblyDocumenter;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_XmlDocValidator
    {
        private ILogger _logMock;

        [SetUp]
        public void Setup()
        {
            _logMock = Substitute.For<ILog>();
        }

        [Test]
        public void EnsureThatValidXmlResultsTrue()
        {
            bool result = XmlDocValidator.ValidateXml(new FsPath(TestEnvironment.GetFile("test.xml")), _logMock);
            Assert.That(result, Is.True);
        }

        [Test]
        public void EnsureThatInvalidXmlResultsFalse()
        {
            bool result = XmlDocValidator.ValidateXml(new FsPath(TestEnvironment.GetFile("test.js")), _logMock);
            Assert.That(result, Is.False);
        }
    }
}
