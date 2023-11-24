//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Tests
{
    [TestFixture, SingleThreaded]
    public class UT_ShortCodeLoader
    {
        private ShortCodeLoader _sut;
        private ILog _logMock;
        private TimeProvider _timeProvider;

        [SetUp]
        public void Setup()
        {
            _timeProvider = Substitute.For<TimeProvider>();
            _logMock = Substitute.For<ILog>();
            _sut = new ShortCodeLoader(_logMock,
                                       TestEnvironment.GetMockedSettings(),
                                       TestEnvironment.GetMockedAppSettings(),
                                       _timeProvider);
            _sut.LoadAll();
        }

        [TearDown]
        public void TearDown()
        {
            _sut.Dispose();
            _sut = null;
        }

        [Test]
        public void EnsureThat_ShortCodeLoaderLoadsShortCodes()
        {
            Assert.That(_sut.Imports, Is.Not.Empty);
        }

        [Test]
        public void EnsureThat_ShortCodeLoader_SatisfiesLogImport()
        {
            ITemplateShortCode sri = _sut.Imports.FirstOrDefault(s => s.Tag == nameof(SriDependency));
            Assert.That(sri, Is.Not.Null);
        }
    }
}
