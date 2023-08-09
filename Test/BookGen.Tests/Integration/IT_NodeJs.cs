//-----------------------------------------------------------------------------
// (c) 2020-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework.Scripts;

namespace BookGen.Tests.Integration
{
    [TestFixture, SingleThreaded]
    public class IT_NodeJs
    {
        private NodeJs _sut;
        private ILog _log;

        [OneTimeSetUp]
        public void Setup()
        {
            var _log = Substitute.For<ILog>();
            _sut = new NodeJs(_log, TestEnvironment.GetMockedSettings(), TestEnvironment.GetMockedAppSettings());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _sut = null;
            _log = null;
        }

        [Test]
        public void EnsureThat_Generate_RunsSimpleJs()
        {
            string result = _sut.Generate(new ShortCodeArguments(new Dictionary<string, string>
            {
                { "file", TestEnvironment.GetFile("SimpleNodeJs.js") }
            }));
            Assert.That(result, Is.EqualTo("Hello from nodeJS\n"));
        }

        [Test]
        public void EnsureThat_Generate_RunsComplexJs()
        {
            string result = _sut.Generate(new ShortCodeArguments(new Dictionary<string, string>
            {
                { "file", TestEnvironment.GetFile("LongNodeJs.js") }
            }));
            Assert.That(result, Is.EqualTo("0\n1\n2\n3\n4\n5\n6\n7\n8\n9\n"));
        }
    }
}
