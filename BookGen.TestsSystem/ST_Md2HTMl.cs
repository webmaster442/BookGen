using BookGen.Api;
using NUnit.Framework;

namespace BookGen.TestsSystem
{
    public class ST_Md2HTMl : SystemTestBase
    {
        public ST_Md2HTMl() : base("Book")
        {
        }

        [Test]
        public void Test_NoSyntax()
        {
            EnsureRunWithoutException(ExitCode.Succes, "md2html -i Testpage.md -ns -r -o nsr.html");
            Environment.AssertFileExistsAndHasContents("nsr.html");

        }

        public override void CleanFiles()
        {
            Environment.DeleteFile("nsr.html");
        }
    }
}
