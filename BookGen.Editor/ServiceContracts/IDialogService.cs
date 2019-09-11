//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using ICSharpCode.AvalonEdit.Document;

namespace BookGen.Editor.ServiceContracts
{
    internal interface IDialogService
    {
        void ShowSpellSettingsConfiguration();
        bool ShowGotoLineDialog(IDocument currentDocument, int carretOffset, out int line);
        bool ShowInsertLinkDialog(out string link, out string linkText);
        bool ShowInsertPictureDialog(out bool isFigure, out string url, out string alt);
        void CloseFlyouts();
        void OpenFileExplorer();
    }
}
