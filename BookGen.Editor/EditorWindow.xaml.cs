//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Editor.Dialogs;
using BookGen.Editor.Framework;
using BookGen.Editor.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace BookGen.Editor
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow
    {
        private readonly FsPath _file;
        private string _fileHash;

        public ICommand SaveCommand { get; }
        public ICommand InsertPictureCommand { get; }
        public ICommand InsertLinkCommand { get; }

        public EditorWindow(FsPath file)
        {
            InitializeComponent();
            DataContext = this;
            _file = file;
            Editor.Text = _file.ReadFile();
            _fileHash = HashUtils.GetSHA1(Editor.Text);
            SaveCommand = DelegateCommand.CreateCommand(OnSave, OnCanSave);
            InsertPictureCommand = DelegateCommand.CreateCommand(OnInsertPicture);
            InsertLinkCommand = DelegateCommand.CreateCommand(OnInsertLink);
        }

        private void OnInsertLink(object obj)
        {
            var lnk = new InsertLinkDialog();
            if (lnk.ShowDialog() == true)
            {
                string md = null;
                if (string.IsNullOrEmpty(lnk.LinkText))
                    md = lnk.Link;
                else
                    md = $"[{lnk.LinkText}]({lnk.Link})";

                Editor.InsertstringAtCaretPos(md);
            }
        }

        private void OnInsertPicture(object obj)
        {
            var pict = new InsertPictureDialog(_file);
            if (pict.ShowDialog() == true)
            {
                string md = $"![{pict.Alt}]({pict.Url})";
                Editor.InsertstringAtCaretPos(md);
            }
        }

        private bool OnCanSave(object obj)
        {
            return _fileHash != HashUtils.GetSHA1(Editor.Text);
        }

        private void OnSave(object obj)
        {
            _file.WriteFile(Editor.Text);
            _fileHash = HashUtils.GetSHA1(Editor.Text);
        }

        private void Backstage_IsOpenChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ExceptionHandler.SafeRun(() =>
            {
                string mdHtml = EditorServices.RenderPreview(Editor.Text, _file);
                HtmlView.RenderPartialHtml(mdHtml);
            });
        }

        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (HashUtils.GetSHA1(Editor.Text) != _fileHash)
            {
                var q = MessageBox.Show("Do you want to save changes?", "Save changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (q == MessageBoxResult.Cancel)
                    e.Cancel = true;
                else if (q == MessageBoxResult.Yes)
                    OnSave(null);
            }
        }
    }
}
