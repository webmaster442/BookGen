//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.Annotations;
using BookGen.Infrastructure;

using Spectre.Console;

namespace BookGen.Commands.Docs;

[ExitCode(ExitCodes.Success, "The command executed successfully.")]
internal abstract class DocumentCommandBase : Cli.Command
{
    protected abstract string GetDocumentContent();

    public override int Execute(IReadOnlyList<string> context)
    {

        if (Console.IsOutputRedirected)
        {
            AnsiConsole.WriteLine(GetDocumentContent());
            return ExitCodes.Success;
        }

        HelpRenderer.RenderHelp(GetDocumentContent());

        return ExitCodes.Success;
    }
}
