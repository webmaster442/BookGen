﻿//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Tests
{
    [TestFixture, SingleThreaded]
    public class UT_ShortCodeLoader
    {
        private ShortCodeLoader _sut;
        private Mock<ILog> _logMock;

        [SetUp]
        public void Setup()
        {
            _logMock = new Mock<ILog>();
            _sut = new ShortCodeLoader(_logMock.Object, TestEnvironment.GetMockedSettings(), TestEnvironment.GetMockedAppSettings());
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
            Assert.IsTrue(_sut.Imports.Count > 0);
        }

        [Test]
        public void EnsureThat_ShortCodeLoader_SatisfiesLogImport()
        {
            ITemplateShortCode sri = _sut.Imports.FirstOrDefault(s => s.Tag == nameof(SriDependency));
            Assert.IsNotNull(sri);
        }
    }
}