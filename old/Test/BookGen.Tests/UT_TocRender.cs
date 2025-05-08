using System.IO;

using BookGen.DomainServices.Markdown.TableOfContents;

using Markdig;

namespace BookGen.Tests;

[TestFixture]
public class UT_TocRender
{
    private MarkdownPipeline _pipeline;

    [SetUp]
    public void Setup()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseTableOfContents()
            .UseGenericAttributes()
            .Build();
    }

    [TestCase("normal.md")]
    [TestCase("normal-with-title.md")]
    [TestCase("normal-limited.md")]
    public void TestTocRender(string inputFile)
    {
        string markDown = File.ReadAllText(TestEnvironment.GetFile(inputFile));
        string expectedHTML = File.ReadAllText(TestEnvironment.GetFile(Path.ChangeExtension(inputFile, ".html"))).Replace("\r\n", "\n");

        string result = Markdown.ToHtml(markDown, _pipeline);
        Assert.That(result, Is.EqualTo(expectedHTML));
    }
}
