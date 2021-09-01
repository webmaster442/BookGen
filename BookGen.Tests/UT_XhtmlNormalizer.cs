//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Resources;
using NUnit.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    class UT_XhtmlNormalizer
    {
        [TestCase("<figure>foo</figure>", "<div>foo</div>")]
        [TestCase("<article>foo</article>", "<div>foo</div>")]
        [TestCase("<details>foo</details>", "<div>foo</div>")]
        [TestCase("<footer>foo</footer>", "<div>foo</div>")]
        [TestCase("<header>foo</header>", "<div>foo</div>")]
        [TestCase("<section>foo</section>", "<div>foo</div>")]
        [TestCase("<nav>foo</nav>", "<div>foo</div>")]
        [TestCase("<figcaption>foo</figcaption>", "<p>foo</p>")]
        [TestCase("<script>foo;</script>", "<script><![CDATA[foo;]]></script>")]
        [TestCase("<script id=\"asd\">foo;</script>", "<script id=\"asd\"><![CDATA[foo;]]></script>")]
        [TestCase("<style>foo;</style>", "<style><![CDATA[foo;]]></style>")]
        public void EnsureThat_XhtmlNormalizer_NormalizeToXHTML_Correct(string input, string expected)
        {
            var result = XhtmlNormalizer.NormalizeToXHTML(input);
            Assert.AreEqual(expected, result);
        }
    }
}
