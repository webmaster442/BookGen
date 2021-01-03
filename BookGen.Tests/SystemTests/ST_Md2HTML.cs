//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using NUnit.Framework;

namespace BookGen.Tests.SystemTests
{
    [TestFixture]
    public class ST_Md2HTML: SystemTestBase
    {
        [Test]
        public void Test_Raw_DoesntCreateHtmlDocTags()
        {
            //arrange
            CopyDemoProject();

            //Act
            RunProgramAndAssertSuccess("Md2HTML", "-i", $"{Workdir}\\Testpage.md", "-o", $"{Workdir}\\test.html", "-r");

            //Assert
            SystemAsserts.FileExists(Combine(Workdir, "test.html"));
            SystemAsserts.FileHasContent(Combine(Workdir, "test.html"));
            SystemAsserts.FileNotConainsStrings(Combine(Workdir, "test.html"),
                new string[]
                {
                    "<html>",
                    "</html>"
                });
        }
    }
}
