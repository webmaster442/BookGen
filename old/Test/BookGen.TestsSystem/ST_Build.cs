using BookGen.Domain;

using NUnit.Framework;

namespace BookGen.TestsSystem;

[TestFixture]
internal class ST_Build : SystemTestBase
{
    public ST_Build() : base("book")
    {
    }

    [Test, CancelAfter(3000)]
    public void Test_Build_Print()
    {
        EnsureRunWithoutException(Constants.Succes, "Build -a BuildPrint -n");
        Environment.AssertFileExistsAndHasContents("output", "print", "print.html");
    }

    [Test, CancelAfter(3000)]
    public void Test_Build_Website()
    {
        EnsureRunWithoutException(Constants.Succes, "Build -a BuildWeb -n");

        Assert.Multiple(() =>
        {
            Environment.AssertFileExistsAndHasContents("output", "web", "sitemap.xml");
            Environment.AssertFileExistsAndHasContents("output", "web", "search.html");
            Environment.AssertFileExistsAndHasContents("output", "web", "index.html");
            Environment.AssertFileExistsAndHasContents("output", "web", "TestPage.html");
            Environment.AssertFileExistsAndHasContents("output", "web", "pages.js");
            Environment.AssertFileExistsAndHasContents("output", "web", "Assets", "popper.min.js");
            Environment.AssertFileExistsAndHasContents("output", "web", "Assets", "bootstrap.min.js");
            Environment.AssertFileExistsAndHasContents("output", "web", "Assets", "jquery.min.js");
            Environment.AssertFileExistsAndHasContents("output", "web", "Assets", "bootstrap.min.css");
            Environment.AssertFileExistsAndHasContents("output", "web", "Assets", "prism.js");
            Environment.AssertFileExistsAndHasContents("output", "web", "Assets", "prism.css");
        });
    }

    protected override void CleanFiles()
    {
        Environment.DeleteDir("output");
    }
}
