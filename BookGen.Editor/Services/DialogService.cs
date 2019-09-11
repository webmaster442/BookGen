//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.ServiceContracts;
using BookGen.Editor.Views.Dialogs;
using ICSharpCode.AvalonEdit.Document;
using MahApps.Metro.Controls;
using System;
using System.Linq;

namespace BookGen.Editor.Services
{
    internal class DialogService : IDialogService
    {
        public void CloseFlyouts()
        {
            if (App.Current.MainWindow is MetroWindow mw)
            {
                foreach (Flyout flyout in mw.Flyouts.Items)
                {
                    flyout.IsOpen = false;
                }
            }
        }

        public void OpenFileExplorer()
        {
            if (App.Current.MainWindow is MetroWindow mw)
            {
                foreach (Flyout flyout in mw.Flyouts.Items)
                {
                    if (flyout.Name == "FileExplorer")
                    {
                        flyout.IsOpen = true;
                        break;
                    }
                }
            }
        }

        public bool ShowGotoLineDialog(IDocument currentDocument, int carretOffset, out int line)
        {
            var currentline = currentDocument.GetLineByOffset(carretOffset).LineNumber;
            var dialog = new GotoLineDialog(currentDocument.LineCount, currentline);
            if (dialog.ShowDialog() == true)
            {
                line = dialog.Line;
                return true;
            }
            else
            {
                line = -1;
                return false;
            }
        }

        public bool ShowInsertLinkDialog(out string link, out string linkText)
        {
            var dialog = new InsertLinkDialog();
            if (dialog.ShowDialog() == true)
            {
                linkText = dialog.LinkText;
                link = dialog.Link;
                return true;
            }
            else
            {
                link = string.Empty;
                linkText = string.Empty;
                return false;
            }
        }

        public bool ShowInsertPictureDialog(out bool isFigure, out string url, out string alt)
        {
            throw new NotImplementedException();
        }

        public void ShowSpellSettingsConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
