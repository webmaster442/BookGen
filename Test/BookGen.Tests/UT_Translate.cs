﻿//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;

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
            var arg = new ShortCodeArguments(new Dictionary<string, string>
            {
                { input, "" }
            });
            string result = _sut.Generate(arg);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("test?")]
        [TestCase("key-lookup")]
        [TestCase("???")]
        [TestCase("#&")]
        [TestCase("foo#")]
        public void EnsureThat_Translate_ReturnsError_WhenKeyFormatInvalid(string input)
        {
            var arg = new ShortCodeArguments(new Dictionary<string, string>
            {
                { input, "" }
            });
            string result = _sut.Generate(arg);
            Assert.That(result, Is.EqualTo($"Invalid tranlation key: {input}"));
        }
    }
}