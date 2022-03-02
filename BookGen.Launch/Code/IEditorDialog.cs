//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using ICSharpCode.AvalonEdit.Document;

namespace BookGen.Launch.Code
{
    internal interface IEditorDialog
    {
        IDocument Document { get; }
    }
}
