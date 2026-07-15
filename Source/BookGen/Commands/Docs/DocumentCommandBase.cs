//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Infrastructure;

using Spectre.Console;

namespace BookGen.Commands.Docs;

internal abstract class DocumentCommandBase : Cli.Command
{
    protected string ReadEmbeddedResource(string resourceName)
    {
        using var stream = GetType().Assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new InvalidOperationException($"Resource '{resourceName}' not found.");
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    protected abstract string GetDocumentContent();

    public override int Execute(IReadOnlyList<string> context)
    {

        if (Console.IsOutputRedirected)
        {
            AnsiConsole.WriteLine(GetDocumentContent());
            return ExitCodes.Success;
        }

        HelpRenderer renderer = new();
        renderer.RenderHelp(GetDocumentContent().Split('\n'));

        return ExitCodes.Success;
    }
}
