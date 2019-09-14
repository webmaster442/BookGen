//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Editor.Infrastructure;
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
        private readonly IFileSystemServices _fileSystemServices;

        public DialogService(IFileSystemServices fileSystemServices)
        {
            _fileSystemServices = fileSystemServices;
        }

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
            var dir = new FsPath(EditorSessionManager.CurrentSession.WorkDirectory);
            var dialog = new InsertPictureDialog(dir, _fileSystemServices);
            if (dialog.ShowDialog() == true)
            {
                isFigure = dialog.IsFigure;
                url = dialog.Url;
                alt = dialog.Alt;
                return true;
            }

            isFigure = false;
            url = string.Empty;
            alt = string.Empty;
            return false;
        }

        public void ShowSpellSettingsConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
