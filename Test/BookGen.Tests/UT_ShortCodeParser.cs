//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Framework.Scripts;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_ShortCodeParser
    {
        private ShortCodeParser _sut;
        private ILog _log;

        [SetUp]
        public void Setup()
        {
            _log = Substitute.For<ILog>();
            var tranlate = new Translations();
            var handler = new CsharpScriptHandler(_log);

            _sut = new ShortCodeParser(new List<ITemplateShortCode>
            {
                new Stubs.DumyShortCode(),
                new Stubs.ArgumentedShortCode(),
                new Stubs.ArgumentNameYielderShortCode()
            }, handler, tranlate, _log);
        }

        [TearDown]
        public void TearDown()
        {
            _sut = null;
        }

        [Test]
        public void EnshureThat_ShortCodeParser_Parse_Works_ForSimple()
        {
            string result = _sut.Parse("<!--{Dumy}-->");
            Assert.That(result, Is.EqualTo("Genrated"));
        }

        [Test]
        public void EnshureThat_ShortCodeParser_Parse_Works_ForComplexStrings()
        {
            const string input = "Lorem ipsum dolor sit <!--{Dumy}-->, consectetur adipiscing elit. Nam non porttitor ligula. Proin eget pulvinar nisi. Suspendisse urna quam, vehicula nec felis eget, faucibus vestibulum diam. Etiam ultrices dignissim laoreet. Cras porttitor, nisi sit amet commodo porttitor, enim felis tempus eros, ac mattis sapien ante sed nunc. Praesent sodales porttitor nisi in dictum. Proin ut sapien turpis. Mauris mattis aliquet condimentum. Nullam imperdiet libero sit amet risus placerat, in eleifend arcu vestibulum. Morbi aliquam rutrum turpis, sit amet ultrices lorem tempor sit amet. Curabitur mollis placerat mi, ut auctor dui bibendum in. Aenean quis placerat lorem, <!--{Dumy}--> mollis elit. Proin.";
            const string expected = "Lorem ipsum dolor sit Genrated, consectetur adipiscing elit. Nam non porttitor ligula. Proin eget pulvinar nisi. Suspendisse urna quam, vehicula nec felis eget, faucibus vestibulum diam. Etiam ultrices dignissim laoreet. Cras porttitor, nisi sit amet commodo porttitor, enim felis tempus eros, ac mattis sapien ante sed nunc. Praesent sodales porttitor nisi in dictum. Proin ut sapien turpis. Mauris mattis aliquet condimentum. Nullam imperdiet libero sit amet risus placerat, in eleifend arcu vestibulum. Morbi aliquam rutrum turpis, sit amet ultrices lorem tempor sit amet. Curabitur mollis placerat mi, ut auctor dui bibendum in. Aenean quis placerat lorem, Genrated mollis elit. Proin.";
            string result = _sut.Parse(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void EnsureThat_ShortCodeParser_Parse_Works_WhenNoMatches()
        {
            string result = _sut.Parse("test");
            Assert.That(result, Is.EqualTo("test"));
        }

        [Test]
        public void EnshureThat_ShortCodeParser_Parse_Works_ForArgumented()
        {
            string result = _sut.Parse("<!--{Arguments parameter=\"success\"}-->");
            Assert.That(result, Is.EqualTo("success"));
        }

        [Test]
        public void EnshureThat_ShortCodeParser_Parse_Works_ForFilePath()
        {
            string result = _sut.Parse("<!--{Arguments parameter=\"c:\\asd folder\foo.txt\"}-->");
            Assert.That(result, Is.EqualTo("c:\\asd folder\foo.txt"));
        }

        [TestCase("asd")]
        [TestCase("0123")]
        [TestCase("foo")]
        public void EnshureThat_ShortCodeParser_Parse_HandlesArgumentsWithoutValue(string input)
        {
            string result = _sut.Parse($"<!--{{yield {input}}}-->");
            Assert.That(result, Is.EqualTo(input));
        }
    }
}
