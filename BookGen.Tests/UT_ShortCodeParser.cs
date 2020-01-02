//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework;
using BookGen.Framework.Scripts;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_ShortCodeParser
    {
        private ShortCodeParser _sut;
        private Mock<ILog> _log;

        [SetUp]
        public void Setup()
        {
            _log = new Mock<ILog>();
            var tranlate = new Translations();
            var handler = new ScriptHandler(_log.Object);

            _sut = new ShortCodeParser(new List<ITemplateShortCode>
            {
                new Stubs.DumyShortCode(),
                new Stubs.ArgumentedShortCode(),
                new Stubs.ArgumentNameYielderShortCode()
            }, handler, tranlate, _log.Object);
        }

        public void TearDown()
        {
            _sut = null;
        }

        [Test]
        public void EnshureThat_ShortCodeParser_Parse_Works_ForSimple()
        {
            var result = _sut.Parse("<!--{Dumy}-->");
            Assert.AreEqual("Genrated", result);
        }

        [Test]
        public void EnshureThat_ShortCodeParser_Parse_Works_ForComplexStrings()
        {
            var input = "Lorem ipsum dolor sit <!--{Dumy}-->, consectetur adipiscing elit. Nam non porttitor ligula. Proin eget pulvinar nisi. Suspendisse urna quam, vehicula nec felis eget, faucibus vestibulum diam. Etiam ultrices dignissim laoreet. Cras porttitor, nisi sit amet commodo porttitor, enim felis tempus eros, ac mattis sapien ante sed nunc. Praesent sodales porttitor nisi in dictum. Proin ut sapien turpis. Mauris mattis aliquet condimentum. Nullam imperdiet libero sit amet risus placerat, in eleifend arcu vestibulum. Morbi aliquam rutrum turpis, sit amet ultrices lorem tempor sit amet. Curabitur mollis placerat mi, ut auctor dui bibendum in. Aenean quis placerat lorem, <!--{Dumy}--> mollis elit. Proin.";
            var expected = "Lorem ipsum dolor sit Genrated, consectetur adipiscing elit. Nam non porttitor ligula. Proin eget pulvinar nisi. Suspendisse urna quam, vehicula nec felis eget, faucibus vestibulum diam. Etiam ultrices dignissim laoreet. Cras porttitor, nisi sit amet commodo porttitor, enim felis tempus eros, ac mattis sapien ante sed nunc. Praesent sodales porttitor nisi in dictum. Proin ut sapien turpis. Mauris mattis aliquet condimentum. Nullam imperdiet libero sit amet risus placerat, in eleifend arcu vestibulum. Morbi aliquam rutrum turpis, sit amet ultrices lorem tempor sit amet. Curabitur mollis placerat mi, ut auctor dui bibendum in. Aenean quis placerat lorem, Genrated mollis elit. Proin.";
            var result = _sut.Parse(input);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EnsureThat_ShortCodeParser_Parse_Works_WhenNoMatches()
        {
            var result = _sut.Parse("test");
            Assert.AreEqual("test", result);
        }


        [Test]
        public void EnshureThat_ShortCodeParser_Parse_Works_ForArgumented()
        {
            var result = _sut.Parse("<!--{Arguments parameter=\"success\"}-->");
            Assert.AreEqual("success", result);
        }

        [Test]
        public void EnshureThat_ShortCodeParser_Parse_Works_ForFilePath()
        {
            var result = _sut.Parse("<!--{Arguments parameter=\"c:\\asd folder\foo.txt\"}-->");
            Assert.AreEqual("c:\\asd folder\foo.txt", result);
        }

        [TestCase("asd")]
        [TestCase("0123")]
        [TestCase("foo")]
        public void EnshureThat_ShortCodeParser_Parse_HandlesArgumentsWithoutValue(string input)
        {
            var result = _sut.Parse($"<!--{{yield {input}}}-->");
            Assert.AreEqual(input, result);
        }
    }
}
