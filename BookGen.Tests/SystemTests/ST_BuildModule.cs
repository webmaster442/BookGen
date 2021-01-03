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
        public void TestHtmlBuild()
        {
            //Arrange
            CopyDemoProject();
            CreateConfigFile();
            
            //act
            RunProgram("Build", "-n", "-d", Workdir, "-a", "BuildWeb");

            //assert
            foreach (var file in htmlExpectedFiles)
            {
                SystemAsserts.FileExists(BuildDir, file);
                SystemAsserts.FileHasContent(BuildDir, file);
            }
        }

        [Test]
        public void TestWordpressBuild()
        {
            //Arrange
            CopyDemoProject();
            CreateConfigFile();

            //act
            RunProgram("Build", "-n", "-d", Workdir, "-a", "BuildWordpress");

            //assert
            SystemAsserts.FileExists(BuildDir, "wordpressExport.xml");
            SystemAsserts.FileHasContent(BuildDir, "wordpressExport.xml");
        }

        [Test]
        public void TestEpubBuild()
        {
            //Arrange
            CopyDemoProject();
            CreateConfigFile();

            //act
            RunProgram("Build", "-n", "-d", Workdir, "-a", "BuildEpub");

            //assert
            SystemAsserts.FileExists(BuildDir, "book.epub");
            SystemAsserts.FileHasContent(BuildDir, "book.epub");
        }

        [Test]
        public void TestPrintBuild()
        {
            //Arrange
            CopyDemoProject();
            CreateConfigFile();

            //act
            RunProgram("Build", "-n", "-d", Workdir, "-a", "BuildPrint");

            //assert
            SystemAsserts.FileExists(BuildDir, "print.html");
            SystemAsserts.FileHasContent(BuildDir, "print.html");
            SystemAsserts.FileExists(BuildDir, "Img\\Test.png");
            SystemAsserts.FileHasContent(BuildDir, "Img\\Test.png");
        }
    }
}
