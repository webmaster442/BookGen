using ICSharpCode.AvalonEdit.Document;

namespace BookGen.Launch.Code
{
    internal interface IEditorDialog
    {
        IDocument Document { get; }
    }
}
