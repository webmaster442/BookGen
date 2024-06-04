//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain;

using NUnit.Framework;

namespace BookGen.TestsSystem
{
    public class ST_Md2HTML : SystemTestBase
    {
        public ST_Md2HTML() : base("Book")
        {
        }

        [Test, CancelAfter(3000)]
        public void Test_NoSyntax_Raw()
        {
            EnsureRunWithoutException(Constants.Succes, "md2html -i Testpage.md -ns -r -o nsr.html -nw");
            Environment.AssertFileExistsAndHasContents("nsr.html");
            string? contents = Environment.ReadFileContents("nsr.html");
            Assert.Multiple(() =>
            {
                Assert.That(contents, Does.Not.StartWith("<html>"));
                Assert.That(contents, Does.Not.EndWith("</html>"));
            });
        }

        [Test, CancelAfter(3000)]
        public void Test_Syntax_Raw()
        {
            EnsureRunWithoutException(Constants.Succes, "md2html -i Testpage.md -r -o r.html -nw");
            Environment.AssertFileExistsAndHasContents("r.html");
            string? contents = Environment.ReadFileContents("r.html");
            Assert.Multiple(() =>
            {
                Assert.That(contents, Does.Contain("<span class=\"token keyword\">"));
                Assert.That(contents, Does.Contain("<style"));
                Assert.That(contents, Does.Contain("</style>"));

                Assert.That(contents, Does.Not.StartWith("<html>"));
                Assert.That(contents, Does.Not.EndWith("</html>"));
            });
        }

        [Test, CancelAfter(3000)]
        public void Test_Full()
        {
            EnsureRunWithoutException(Constants.Succes, "md2html -i Testpage.md -o full.html -nw");

            Environment.AssertFileExistsAndHasContents("full.html");
            string? contents = Environment.ReadFileContents("full.html");
            Assert.Multiple(() =>
            {
                Assert.That(contents, Does.Contain("<span class=\"token keyword\">"));
                Assert.That(contents, Does.Contain("<style"));
                Assert.That(contents, Does.Contain("</style>"));

                Assert.That(contents, Does.Contain("<html>"));
                Assert.That(contents, Does.EndWith("</html>\r\n"));
            });
        }

        protected override void CleanFiles()
        {
            Environment.DeleteFile("nsr.html");
            Environment.DeleteFile("full.html");
            Environment.DeleteFile("r.html");
        }
    }
}
