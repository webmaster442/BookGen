//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Framework;
using NUnit.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_TranslationApplier
    {
        private Translations _translations;

        [SetUp]
        public void Setup()
        {
            _translations = new Translations
            {
                { "asd", "dsa" },
                { "abc", "def" }
            };
        }

        [Test]
        public void EnsureThat_TranslationApplier_ApplyTranslations_ReplacesAllKnown()
        {
            var result = TranslationApplier.ApplyTranslations("This is a {asd} words {abc} test string", _translations);
            Assert.AreEqual("This is a dsa words def test string", result);
        }

        [Test]
        public void EnsureThat_TranslationApplier_ApplyTranslations_IndicatesMissings()
        {
            var result = TranslationApplier.ApplyTranslations("This is a {missing} test", _translations);
            Assert.AreEqual("This is a translation not found: {missing} test", result);
        }
    }
}
