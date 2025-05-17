using Bookgen.Lib;

using BookGen.Commands;
using BookGen.Vfs;

using Moq;

namespace Bookgen.Tests.Commands;

[TestFixture]
internal class UT_Md2HtmlCommand : CommandTestBase<Md2HtmlCommand>
{
    protected override Md2HtmlCommand CreateSut()
        => new Md2HtmlCommand(LoggerMock.Object, FileSystemMock.Object, AssetSourceMock.Object);

    protected override void SetupMocks()
    {
        AssetSourceMock.Setup(a => a.GetAsset(BundledAssets.TemplateSinglePage)).Returns("<h1>{{Title}}</h1>{{Content}}");
        FileSystemMock.Setup(fs => fs.ReadAllText("test.md")).Returns("test");
        FileSystemMock.As<IReadOnlyFileSystem>().Setup(fs => fs.ReadAllText("test.md")).Returns("test");
        FileSystemMock.Setup(fs => fs.WriteAllText("out.html", It.IsAny<string>()));
    }

    private static string Normalize(string input)
    {
        return input.Replace("\r\n", "\n").Replace("\r", "\n");
    }

    [Test]
    public async Task EnsureThat_GenerateRawWorks()
    {
        var arguments = new Md2HtmlCommand.Md2HtmlArguments
        {
            InputFiles = ["test.md"],
            NoSyntax = true,
            OutputFile = "out.html",
            RawHtml = true,
            SvgPassthrough = true,
            Title = "Document title"
        };

        int exitCode = await Command.Execute(arguments, Array.Empty<string>());

        string expectedContent = "<p>test</p>\n";

        Assert.Multiple(() =>
        {
            Assert.That(exitCode, Is.EqualTo(0));
            FileSystemMock.Verify(fs => fs.ReadAllText("test.md"), Times.Once);
            FileSystemMock.Verify(fs => fs.WriteAllText(
                                            "out.html",
                                            expectedContent),
                                  Times.Once);
        });
    }

    [Test]
    public async Task EnsureThat_GenerateHtml_Works()
    {
        var arguments = new Md2HtmlCommand.Md2HtmlArguments
        {
            InputFiles = ["test.md"],
            NoSyntax = false,
            OutputFile = "out.html",
            RawHtml = false,
            SvgPassthrough = true,
            Title = "Document title"
        };

        int exitCode = await Command.Execute(arguments, Array.Empty<string>());

        string expectedContent = "<h1>Document title</h1><p>test</p>\n";

        Assert.Multiple(() =>
        {
            Assert.That(exitCode, Is.EqualTo(0));
            FileSystemMock.Verify(fs => fs.ReadAllText("test.md"), Times.Once);
            FileSystemMock.Verify(fs => fs.WriteAllText(
                                            "out.html",
                                            expectedContent),
                                  Times.Once);
        });
    }
}