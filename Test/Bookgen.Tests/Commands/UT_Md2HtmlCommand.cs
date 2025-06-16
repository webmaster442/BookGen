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
        AssetSourceMock.Setup(a => a.GetAsset(BundledAssets.PrismJs)).Returns("");
        FileSystemMock.As<IReadOnlyFileSystem>().Setup(fs => fs.ReadAllText("test.md")).Returns("test");
        FileSystemMock.As<IReadOnlyFileSystem>().Setup(fs => fs.GetLastModifiedUtc("test.md")).Returns(new DateTime(2024, 1, 1));
        FileSystemMock.Setup(fs => fs.WriteAllText("out.html", It.IsAny<string>()));

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

        int exitCode = await Command.ExecuteAsync(arguments, Array.Empty<string>());

        const string expectedContent = "<p>test</p>\n";

        Assert.Multiple(() =>
        {
            Assert.That(exitCode, Is.EqualTo(0));
            FileSystemMock.Verify(fs => fs.ReadAllText("test.md"), Times.AtLeastOnce());
            FileSystemMock.Verify(fs => fs.WriteAllText("out.html", expectedContent), Times.Once);
        });
    }

    [Test]
    public async Task EnsureThat_GenerateHtml_Works()
    {
        var arguments = new Md2HtmlCommand.Md2HtmlArguments
        {
            InputFiles = ["test.md"],
            NoSyntax = true,
            OutputFile = "out.html",
            RawHtml = false,
            SvgPassthrough = true,
            Title = "Document title"
        };

        int exitCode = await Command.ExecuteAsync(arguments, Array.Empty<string>());

        const string expectedContent = "<h1>Document title</h1><p>test</p>\r\n";

        Assert.Multiple(() =>
        {
            Assert.That(exitCode, Is.EqualTo(0));
            AssetSourceMock.Verify(a => a.GetAsset(BundledAssets.TemplateSinglePage), Times.Once);
            FileSystemMock.Verify(fs => fs.ReadAllText("test.md"), Times.Once);
            FileSystemMock.Verify(fs => fs.WriteAllText("out.html", expectedContent), Times.Once);
        });
    }
}