//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib;

using BookGen.Commands;
using BookGen.Vfs;

using Moq;

namespace Bookgen.Tests.Commands;

[TestFixture]
internal class UT_Md2HtmlCommand : CommandTestBase<Md2HtmlCommand>
{
    private readonly Mock<IReadOnlyFileSystem> MultiReadScopeMock = new Mock<IReadOnlyFileSystem>(MockBehavior.Strict);

    protected override void SetupMocks()
    {
        MultiReadScopeMock.Setup(fs => fs.ReadAllText("test.md")).Returns("test");
        MultiReadScopeMock.Setup(fs => fs.GetLastModifiedUtc("test.md")).Returns(new DateTime(2024, 1, 1));
        AssetSourceMock.Setup(a => a.GetAsset(BundledAssets.TemplateSinglePage)).Returns("<h1>{{Title}}</h1>{{Content}}");
        AssetSourceMock.Setup(a => a.GetAsset(BundledAssets.PrismJs)).Returns("");
        FileSystemMock.Setup(fs => fs.WriteAllText("out.html", It.IsAny<string>()));
        FilesystemFactoryMock.Setup(f => f.CreateMultiReadScopeFileSystem(It.IsAny<IEnumerable<string>>())).Returns(MultiReadScopeMock.Object);
        FilesystemFactoryMock.Setup(f => f.CreateWritableFileSystem(It.IsAny<string>())).Returns(FileSystemMock.Object);
    }

    protected override Md2HtmlCommand CreateSut()
        => new Md2HtmlCommand(LoggerMock.Object, FilesystemFactoryMock.Object, ProgramPathResolverMock.Object, AssetSourceMock.Object);

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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(exitCode, Is.EqualTo(0));
            MultiReadScopeMock.Verify(fs => fs.ReadAllText("test.md"), Times.AtLeastOnce());
            FileSystemMock.Verify(fs => fs.WriteAllText("out.html", expectedContent), Times.Once);
        }
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(exitCode, Is.EqualTo(0));
            AssetSourceMock.Verify(a => a.GetAsset(BundledAssets.TemplateSinglePage), Times.Once);
            MultiReadScopeMock.Verify(fs => fs.ReadAllText("test.md"), Times.Once);
            FileSystemMock.Verify(fs => fs.WriteAllText("out.html", It.IsAny<string>()), Times.Once);
        }
    }
}
