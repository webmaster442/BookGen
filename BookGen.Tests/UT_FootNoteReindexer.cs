//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Markdown;
using NUnit.Framework;

namespace BookGen.Tests
{
    public class UT_FootNoteReindexer
    {
        private FootNoteReindexer _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new FootNoteReindexer();
        }

        [Test]
        public void EnsureThat_FootNoteReindexer_ReindexingWorks()
        {
            string s1 =
                  "first[^1]\r\n"
                + "second[^2]\r\n"
                + "\r\n"
                + "[1]: first\r\n"
                + "[2]: second\r\n";

            string s2 =
                "third[^1]\r\n"
                + "fourth[^2]\r\n"
                + "\r\n"
                + "[1]: third\r\n"
                + "[2]: forth\r\n";

            string expected =
                "first[^1]\r\n"
                + "second[^2]\r\n"
                + "\r\n"
                + "third[^3]\r\n"
                + "fourth[^4]\r\n"
                + "\r\n"
                + "[1]: first\r\n"
                + "[2]: second\r\n"
                + "[3]: third\r\n"
                + "[4]: forth\r\n";
            _sut.AddMarkdown(s1);
            _sut.AddMarkdown(s2);

            string result = _sut.ToString();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EnsureThat_FootNoteReindexer_ReindexingWorksComplex()
        {
            string s1 =
                "first[^1]\r\n"
                + "second[^2]\r\n"
                + "\r\n"
                + "[1]: first\r\n"
                + "[2]: second\r\n";

            string s2 =
                "third[^1]\r\n"
                + "fourth[^2]\r\n"
                + "\r\n"
                + "[1]: third\r\n"
                + "[2]: forth\r\n";

            string expected =
                "first[^1]\r\n"
                + "second[^2]\r\n"
                + "\r\n"
                + "foo\r\n"
                + "third[^3]\r\n"
                + "fourth[^4]\r\n"
                + "\r\n"
                + "[1]: first\r\n"
                + "[2]: second\r\n"
                + "[3]: third\r\n"
                + "[4]: forth\r\n";
            _sut.AddMarkdown(s1);
            _sut.AddMarkdown("foo\r\n");
            _sut.AddMarkdown(s2);

            string result = _sut.ToString();

            Assert.AreEqual(expected, result);
        }

    }
}
