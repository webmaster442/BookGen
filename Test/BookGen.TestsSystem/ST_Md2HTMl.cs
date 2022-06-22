using BookGen.Api;
using NUnit.Framework;

namespace BookGen.TestsSystem
{
    public class ST_Md2HTML : SystemTestBase
    {
        public ST_Md2HTML() : base("Book")
        {
        }

        [Test]
        public void Test_NoSyntax_Raw()
        {
            EnsureRunWithoutException(ExitCode.Succes, "md2html -i Testpage.md -ns -r -o nsr.html");
            Environment.AssertFileExistsAndHasContents("nsr.html");
            var contents = Environment.ReadFileContents("nsr.html");
            Assert.IsFalse(contents.StartsWith("<html>"));
            Assert.IsFalse(contents.EndsWith("</html>"));
        }

        [Test]
        public void Test_Syntax_Raw()
        {
            EnsureRunWithoutException(ExitCode.Succes, "md2html -i Testpage.md -r -o r.html");
            Environment.AssertFileExistsAndHasContents("r.html");
            var contents = Environment.ReadFileContents("r.html");

            Assert.IsTrue(contents.Contains("<span class=\"token keyword\">"));
            Assert.IsTrue(contents.Contains("<style"));
            Assert.IsTrue(contents.Contains("</style>"));

            Assert.IsFalse(contents.StartsWith("<html>"));
            Assert.IsFalse(contents.EndsWith("</html>"));
        }

        [Test]
        public void Test_Full()
        {
            EnsureRunWithoutException(ExitCode.Succes, "md2html -i Testpage.md -o full.html");

            Environment.AssertFileExistsAndHasContents("full.html");
            var contents = Environment.ReadFileContents("full.html");

            Assert.IsTrue(contents.Contains("<span class=\"token keyword\">"));
            Assert.IsTrue(contents.Contains("<style"));
            Assert.IsTrue(contents.Contains("</style>"));

            Assert.IsTrue(contents.Contains("<html>"));
            Assert.IsTrue(contents.EndsWith("</html>"));
        }

        public override void CleanFiles()
        {
            Environment.DeleteFile("nsr.html");
            Environment.DeleteFile("full.html");
            Environment.DeleteFile("r.html");
        }
    }
}
