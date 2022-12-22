﻿//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.IO;

namespace BookGen.Tests
{
    [TestFixture]
    internal class UT_HtmlTidy
    {
        private HtmlTidy _sut;

        [SetUp]
        public void Setup()
        {
            _sut= new HtmlTidy();
        }

        [TestCase("<figure>foo</figure>", "<div>foo</div>")]
        [TestCase("<article>foo</article>", "<div>foo</div>")]
        [TestCase("<details>foo</details>", "<div>foo</div>")]
        [TestCase("<footer>foo</footer>", "<div>foo</div>")]
        [TestCase("<header>foo</header>", "<div>foo</div>")]
        [TestCase("<section>foo</section>", "<div>foo</div>")]
        [TestCase("<nav>foo</nav>", "<div>foo</div>")]
        [TestCase("<figcaption>foo</figcaption>", "<p>foo</p>")]
        public void EnsureThat_HtmlTidy_ConvertHtml5TagsToXhtmlCompatible_Correct(string input, string expected)
        {
            string result = _sut.ConvertHtml5TagsToXhtmlCompatible(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [Timeout(3000)]
        public void EnsureThat_HtmlTidy_HtmlToXhtml_ReturnsText()
        {
            var file = TestEnvironment.GetFile("full.html");
            var contents = File.ReadAllText(file);

            var xhtml = _sut.HtmlToXhtml(contents);

            Assert.Multiple(() =>
            {
                Assert.That(xhtml, Is.Not.Null);
                Assert.That(xhtml, Is.Not.Empty);
            });
        }
    }
}
