//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Framework.Scripts;
using BookGen.Tests.Environment;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace BookGen.Tests.Integration
{
    [TestFixture, SingleThreaded]
    public class IT_ScriptsHandler
    {
        private ScriptHandler _sut;
        private Mock<ILog> _log;
        private Mock<IReadonlyRuntimeSettings> _settings;
        private int _loaded;

        [OneTimeSetUp]
        public void Setup()
        {
            _log = new Mock<ILog>();
            _settings = new Mock<IReadonlyRuntimeSettings>();
            _settings.Setup(x => x.SourceDirectory).Returns(FsPath.Empty);

            _sut = new ScriptHandler(_log.Object);
            _sut.SetHostFromRuntimeSettings(_settings.Object);
            _loaded = _sut.LoadScripts(new FsPath(TestEnvironment.GetTestFolder()));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _loaded = 0;
            _sut = null;
            _log = null;
            _settings = null;
        }

        [Test]
        public void EnsureThat_LoadScripts_RetrunedNumberOfLoadedScripts()
        {
            Assert.AreEqual(1, _loaded);
        }

        [Test]
        public void EnsureThat_TestScriptIsKnownScript()
        {
            Assert.IsTrue(_sut.IsKnownScript("TestScript"));
        }

        [Test]
        public void EnsureThat_CallingTestScript_ReturnsCorrectResult()
        {
            var args = new Dictionary<string, string>();
            string result = _sut.ExecuteScript("TestScript", args);

            Assert.AreEqual("Hello, from test script!", result);
        }
    }
}
