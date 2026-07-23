//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli.Annotations;
using BookGen.Infrastructure;

namespace BookGen.Commands.Docs;

[CommandName("document schemas")]
[Description("Outputs the JSON schemas used by Bookgen on the terminal. Output can be redirected to a file.")]
internal class SchemasCommand : DocumentCommandBase
{
    private readonly IDynamicDocumentGenerator _dynamicDocumentGenerator;

    public SchemasCommand(IDynamicDocumentGenerator dynamicDocumentGenerator)
    {
        _dynamicDocumentGenerator = dynamicDocumentGenerator;
    }

    protected override string GetDocumentContent()
        => _dynamicDocumentGenerator.GenerateSchemasDocument();
}
