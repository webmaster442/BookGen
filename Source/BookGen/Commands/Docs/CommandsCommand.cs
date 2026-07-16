//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli.Annotations;
using BookGen.Infrastructure;

namespace BookGen.Commands.Docs;

[CommandName("document commands")]
[Description("Displays commands reference on the terminal. Output can be redirected to a file.")]
internal sealed class CommandsCommand : DocumentCommandBase
{
    private readonly IDynamicDocumentGenerator _dynamicDocumentGenerator;

    public CommandsCommand(IDynamicDocumentGenerator dynamicDocumentGenerator)
    {
        _dynamicDocumentGenerator = dynamicDocumentGenerator;
    }

    protected override string GetDocumentContent()
        => _dynamicDocumentGenerator.GenerateCommandsDocument();
}
