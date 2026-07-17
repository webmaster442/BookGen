using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("default")]
[Description("The default command that displays the default help message.")]
[ExitCode(ExitCodes.Success, "The command executed successfully.")]
internal sealed class DefaultCommand : Command
{
    public override int Execute(IReadOnlyList<string> context)
    {
        string text = Embedded.ReadEmbeddedResource("BookGen.Resources.default.txt");
        AnsiConsole.MarkupLine(text);
        return ExitCodes.Success;
    }
}
