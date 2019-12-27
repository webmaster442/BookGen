//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Template.ShortCodeImplementations;
using BookGen.Core.Configuration;
using NUnit.Framework;
using System.Collections.Generic;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_Translate
    {
        private Translations _translations;
        private Translate _sut;

        [SetUp]
        public void Setup()
        {
            _translations = new Translations
            {
                { "asd", "dsa" },
                { "abc", "def" }
            };
            _sut = new Translate(_translations);
        }

        [TestCase("asd", "dsa")]
        [TestCase("abc", "def")]
        [TestCase("foo", "translation not found: 'foo'")]
        [TestCase("", "")]
        public void EnsureThat_Translate_ReturnsCorrectValues(string input, string expected)
        {
            var arg = new Dictionary<string, string>();
            arg.Add(input, "");
            var result = _sut.Generate(arg);
            Assert.AreEqual(expected, result);
        }

        [TestCase("test?")]
        [TestCase("key-lookup")]
        [TestCase("???")]
        [TestCase("#&")]
        [TestCase("foo#")]
        public void EnsureThat_Translate_ReturnsError_WhenKeyFormatInvalid(string input)
        {
            var arg = new Dictionary<string, string>();
            arg.Add(input, "");
            var result = _sut.Generate(arg);
            Assert.AreEqual($"Invalid tranlation key: {input}", result);
        }
    }
}