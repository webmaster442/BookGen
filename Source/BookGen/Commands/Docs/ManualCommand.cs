//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli.Annotations;
using BookGen.Infrastructure;

namespace BookGen.Commands.Docs;

[CommandName("document manual")]
[Description("Displays the manual on the terminal. Output can be redirected to a file.")]
internal sealed class ManualCommand : DocumentCommandBase
{
    protected override string GetDocumentContent()
        => Embedded.ReadEmbeddedResource("BookGen.Resources.manual.md");
}
