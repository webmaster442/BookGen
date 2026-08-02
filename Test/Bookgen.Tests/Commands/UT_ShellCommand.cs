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
            "BookGen book", 
            "BookGen book validate",
            "BookGen shell", 
            "BookGen gui",
            "BookGen folder addfrontmatter",
            "BookGen convert",
            "BookGen convert diagram2svg",
            "BookGen convert html2openxml",
            "BookGen convert html2pdf",
            "BookGen convert html2png",
            "BookGen convert images",
            "BookGen convert math2svg",
            "BookGen convert md2html",
            "BookGen convert md2terminal",
            "BookGen convert qrcode",
            "BookGen check"
        ]);
    }

    [TestCase("BookGen convert md", 0, "BookGen convert md2html")]
    [TestCase("BookGen b", 0, "BookGen book")]
    [TestCase("BookGen book", 0, "BookGen book validate")]
    [TestCase("BookGen convert m", 0, "BookGen convert md2html")]
    public async Task EnsureThat_Autocomplete_ReturnsExpected(string input, int index, string expected)
    {
        AnsiConsole.Record();
        var result = await Command.ExecuteAsync(ArgumentsBase.Empty, [index.ToString(), input], CancellationToken.None);
        string output= AnsiConsole.ExportText();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Zero);
            Assert.That(output, Does.Contain(expected));
        }
    }

    [Test]
    public async Task Test_Execute_NoArgs()
    {
        AnsiConsole.Record();
        var result = await Command.ExecuteAsync(ArgumentsBase.Empty, ["c"], CancellationToken.None);
        string output = AnsiConsole.ExportText();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Zero);
            Assert.That(output, Is.Empty);
        }
    }
}
