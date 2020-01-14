using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Framework.Scripts;
using BookGen.Tests.Environment;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace BookGen.Tests
{
    [TestFixture, SingleThreaded]
    public class IT_NodeJsHost
    {
        private NodeJsHost _sut;
        private Mock<ILog> _log;
        private Mock<IReadonlyRuntimeSettings> _settings;

        [OneTimeSetUp]
        public void Setup()
        {
            _log = new Mock<ILog>();
            _settings = new Mock<IReadonlyRuntimeSettings>();
            _settings.Setup(x => x.SourceDirectory).Returns(FsPath.Empty);
            _sut = new NodeJsHost(_log.Object, _settings.Object);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _sut = null;
            _settings = null;
            _log = null;
        }

        [Test]
        public void EnsureThat_Generate_RunsSimpleJs()
        {
            var result = _sut.Generate(new Arguments(new Dictionary<string, string>
            {
                { "file", TestEnvironment.GetFile("SimpleNodeJs.js") }
            }));
            Assert.AreEqual("Hello from nodeJS\n", result);
        }

        [Test]
        public void EnsureThat_Generate_RunsComplexJs()
        {
            var result = _sut.Generate(new Arguments(new Dictionary<string, string>
            {
                { "file", TestEnvironment.GetFile("LongNodeJs.js") }
            }));
            Assert.AreEqual("0\n1\n2\n3\n4\n5\n6\n7\n8\n9\n", result);
        }
    }
}
