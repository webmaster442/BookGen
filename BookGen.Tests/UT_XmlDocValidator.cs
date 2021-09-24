//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.AssemblyDocumenter;
using BookGen.Core;
using BookGen.Tests.Environment;
using Moq;
using NUnit.Framework;

namespace BookGen.Tests.Integration
{
    [TestFixture]
    public class UT_XmlDocValidator
    {
        private Mock<ILog> _logMock;

        [SetUp]
        public void Setup()
        {
            _logMock = new Mock<ILog>();
        }

        [Test]
        public void EnsureThatValidXmlResultsTrue()
        {
            bool result = XmlDocValidator.ValidateXml(new FsPath(TestEnvironment.GetFile("test.xml")), _logMock.Object);
            Assert.IsTrue(result);
        }

        [Test]
        public void EnsureThatInvalidXmlResultsFalse()
        {
            bool result = XmlDocValidator.ValidateXml(new FsPath(TestEnvironment.GetFile("test.js")), _logMock.Object);
            Assert.False(result);
        }
    }
}
