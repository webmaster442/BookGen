//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Framework;
using NUnit.Framework;
using System.Collections.Generic;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_ShortCodeParser
    {
        private ShortCodeParser _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new ShortCodeParser(new List<ITemplateShortCode>
            {
                new Stubs.DumyShortCode(),
                new Stubs.ArgumentedShortCode()
            });
        }

        public void TearDown()
        {
            _sut = null;
        }

        [Test]
        public void EnshureThat_ShortCodeParser_Parse_Works_ForSimple()
        {
            var result = _sut.Parse("[Dumy]");
            Assert.AreEqual("Genrated", result);
        }


        [Test]
        public void EnshureThat_ShortCodeParser_Parse_Works_ForArgumented()
        {
            var result = _sut.Parse("[Arguments parameter=\"success\"]");
            Assert.AreEqual("success", result);
        }
    }
}
