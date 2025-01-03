﻿//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown;

using Microsoft.Extensions.Logging;

namespace BookGen.Tests
{
    public class UT_FootNoteReindexer
    {
        private FootNoteReindexer _sut;
        private ILogger _logMock;

        [SetUp]
        public void Setup()
        {
            _logMock = TestEnvironment.GetMockedLog();
            _sut = new FootNoteReindexer(_logMock);
        }

        [Test]
        public void EnsureThat_FootNoteReindexer_ReindexingWorks()
        {
            const string s1 =
                  "first[^1]\r\n"
                + "second[^2]\r\n"
                + "\r\n"
                + "[^1]: first\r\n"
                + "[^2]: second\r\n";

            const string s2 =
                "third[^1]\r\n"
                + "fourth[^2]\r\n"
                + "\r\n"
                + "[^1]: third\r\n"
                + "[^2]: forth\r\n";

            const string expected =
                "first[^1]\r\n"
                + "second[^2]\r\n"
                + "\r\n"
                + "\r\n"
                + "third[^3]\r\n"
                + "fourth[^4]\r\n"
                + "\r\n"
                + "\r\n"
                + "[^1]: first\r\n"
                + "[^2]: second\r\n"
                + "\r\n"
                + "[^3]: third\r\n"
                + "[^4]: forth\r\n"
                + "\r\n";
            _sut.AddMarkdown(s1);
            _sut.AddMarkdown(s2);

            string result = _sut.ToString();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void EnsureThat_FootNoteReindexer_ReindexingWorksComplex1()
        {
            const string s1 =
                "first[^1]\r\n"
                + "second[^2]\r\n"
                + "\r\n"
                + "[^1]: first\r\n"
                + "[^2]: second\r\n";

            const string s2 =
                "third[^1]\r\n"
                + "fourth[^2]\r\n"
                + "\r\n"
                + "[^1]: third\r\n"
                + "[^2]: forth\r\n";

            const string expected =
                "first[^1]\r\n"
                + "second[^2]\r\n"
                + "\r\n"
                + "\r\n"
                + "foo\r\n"
                + "third[^3]\r\n"
                + "fourth[^4]\r\n"
                + "\r\n"
                + "\r\n"
                + "[^1]: first\r\n"
                + "[^2]: second\r\n"
                + "\r\n"
                + "[^3]: third\r\n"
                + "[^4]: forth\r\n"
                + "\r\n";
            _sut.AddMarkdown(s1);
            _sut.AddMarkdown("foo\r\n");
            _sut.AddMarkdown(s2);

            string result = _sut.ToString();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void EnsureThat_FootNoteReindexer_ReindexingWorksComplex2()
        {
            const string s1 =
                "first[^1]\r\n"
                + "second[^2]\r\n"
                + "\r\n"
                + "[^1]: first\r\n"
                + "[^2]: second\r\n";

            const string s2 =
                "third[^1]\r\n"
                + "fourth[^2]\r\n"
                + "asd[^3]\r\n"
                + "bsd[^4]\r\n"
                + "\r\n"
                + "[^1]: third\r\n"
                + "[^2]: forth\r\n"
                + "[^3]: foo\r\n"
                + "[^4]: bar\r\n";

            const string expected =
                "first[^1]\r\n"
                + "second[^2]\r\n"
                + "\r\n"
                + "\r\n"
                + "foo\r\n"
                + "third[^3]\r\n"
                + "fourth[^4]\r\n"
                + "asd[^5]\r\n"
                + "bsd[^6]\r\n"
                + "\r\n"
                + "\r\n"
                + "[^1]: first\r\n"
                + "[^2]: second\r\n"
                + "\r\n"
                + "[^3]: third\r\n"
                + "[^4]: forth\r\n"
                + "[^5]: foo\r\n"
                + "[^6]: bar\r\n"
                + "\r\n";
            _sut.AddMarkdown(s1);
            _sut.AddMarkdown("foo\r\n");
            _sut.AddMarkdown(s2);

            string result = _sut.ToString();

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
