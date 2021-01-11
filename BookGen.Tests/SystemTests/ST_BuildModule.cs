//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using NUnit.Framework;

namespace BookGen.Tests.SystemTests
{
    [TestFixture]
    public class ST_BuildModule : SystemTestBase
    {
        private readonly string[] htmlExpectedFiles = new string[]
        {
            "TestPage.html",
            "sitemap.xml",
            "search.html",
            "pages.js",
            "index.html",
            "Assets\\turbolinks.js",
            "Assets\\prism.js",
            "Assets\\prism.css",
            "Assets\\popper.min.js",
            "Assets\\jquery.min.js",
            "Assets\\bootstrap.min.js",
            "Assets\\bootstrap.min.js",

        };

        [Test]
        public void TestBuildFailsNoconfig()
        {
            //Arrange
            CopyDemoProject();

            //Act
            RunProgram("Build", "-n", "-d", Workdir, "-a", "BuildWeb");
            
            //Assert
            Assert.IsTrue(Program.ErrorHappened);
        }

        [Test]
        public void TestHtmlBuild()
        {
            //Arrange
            CopyDemoProject();
            CreateConfigFile();
            
            //act
            RunProgramAndAssertSuccess("Build", "-n", "-d", Workdir, "-a", "BuildWeb");

            //assert
            foreach (var file in htmlExpectedFiles)
            {
                SystemAsserts.FileExists(Combine(BuildDir, file));
                SystemAsserts.FileHasContent(Combine(BuildDir, file));
            }
        }

        [Test]
        public void TestWordpressBuild()
        {
            //Arrange
            CopyDemoProject();
            CreateConfigFile();

            //act
            RunProgramAndAssertSuccess("Build", "-n", "-d", Workdir, "-a", "BuildWordpress");

            //assert
            SystemAsserts.FileExists(Combine(BuildDir, "wordpressExport.xml"));
            SystemAsserts.FileHasContent(Combine(BuildDir, "wordpressExport.xml"));
        }

        [Test]
        public void TestEpubBuild()
        {
            //Arrange
            CopyDemoProject();
            CreateConfigFile();

            //act
            RunProgramAndAssertSuccess("Build", "-n", "-d", Workdir, "-a", "BuildEpub");

            //assert
            SystemAsserts.FileExists(Combine(BuildDir, "book.epub"));
            SystemAsserts.FileHasContent(Combine(BuildDir, "book.epub"));
        }

        [Test]
        public void TestPrintBuild()
        {
            //Arrange
            CopyDemoProject();
            CreateConfigFile();

            //act
            RunProgramAndAssertSuccess("Build", "-n", "-d", Workdir, "-a", "BuildPrint");

            //assert
            SystemAsserts.FileExists(Combine(BuildDir, "print.html"));
            SystemAsserts.FileHasContent(Combine(BuildDir, "print.html"));
            SystemAsserts.FileExists(Combine(BuildDir, "Img\\Test.png"));
            SystemAsserts.FileHasContent(Combine(BuildDir, "Img\\Test.png"));
        }
    }
}
