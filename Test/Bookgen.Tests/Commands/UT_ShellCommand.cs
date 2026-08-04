//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Commands;

using Spectre.Console;

namespace Bookgen.Tests.Commands;


[TestFixture]
internal class UT_ShellCommand : CommandTestBase<ShellCommand>
{
    protected override ShellCommand CreateSut()
    {
        return new ShellCommand(CommandRunnerProxyMock.Object);
    }

    protected override void SetupMocks()
    {
        CommandRunnerProxyMock.Setup(x => x.CommandNames).Returns(
        [
            "book", 
            "book validate",
            "shell", 
            "gui",
            "folder addfrontmatter",
            "convert",
            "convert diagram2svg",
            "convert html2openxml",
            "convert html2pdf",
            "convert html2png",
            "convert images",
            "convert math2svg",
            "convert md2html",
            "convert md2terminal",
            "convert qrcode",
            "check"
        ]);
        CommandRunnerProxyMock.Setup(x => x.GetAutoCompleteItems("convert md2html")).Returns(
        [
            "-i",
            "-o",
            "--input",
            "--output"
        ]);
    }

    [TestCase("BookGen convert md", 17, "md2html")]
    [TestCase("BookGen b", 8, "book")]
    [TestCase("BookGen book", 11, "book validate")]
    [TestCase("BookGen convert m", 16, "md2html")]
    [TestCase("BookGen convert md2html -i", 27, "")]
    public async Task EnsureThat_Autocomplete_ReturnsExpected(string input, int index, string expected)
    {
        using var writer = new StringWriter();
        Console.SetOut(writer);

        var result = await Command.ExecuteAsync(ArgumentsBase.Empty, [index.ToString(), input], CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Zero);
            Assert.That(writer.ToString(), Does.Contain(expected));
        }
    }

    [Test]
    public async Task Test_Execute_NoArgs()
    {
        using var writer = new StringWriter();
        Console.SetOut(writer);

        var result = await Command.ExecuteAsync(ArgumentsBase.Empty, ["c"], CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Zero);
            Assert.That(writer.ToString(), Is.Empty);
        }
    }
}
