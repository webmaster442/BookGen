//-----------------------------------------------------------------------------
// (c) 2020-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework.Scripts;

namespace BookGen.Tests.Integration
{
    [TestFixture, SingleThreaded]
    public class IT_ScriptsHandler
    {
        private CsharpScriptHandler _sut;
        private ILog _log;
        private IReadonlyRuntimeSettings _settings;
        private int _loaded;

        [OneTimeSetUp]
        public void Setup()
        {
            _log = Substitute.For<ILog>();
            _settings = Substitute.For<IReadonlyRuntimeSettings>();
            _settings.SourceDirectory.Returns(FsPath.Empty);

            _sut = new CsharpScriptHandler(_log);
            _sut.SetHostFromRuntimeSettings(_settings);
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
            Assert.That(_loaded, Is.EqualTo(1));
        }

        [Test]
        public void EnsureThat_TestScriptIsKnownScript()
        {
            Assert.That(_sut.IsKnownScript("TestScript"), Is.True);
        }

        [Test]
        public void EnsureThat_CallingTestScript_ReturnsCorrectResult()
        {
            var args = new ShortCodeArguments();
            string result = _sut.ExecuteScript("TestScript", args);

            Assert.That(result, Is.EqualTo("Hello, from test script!"));
        }
    }
}
