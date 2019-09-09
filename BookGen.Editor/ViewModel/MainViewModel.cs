//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.EditorControl;
using BookGen.Editor.Infrastructure;
using BookGen.Editor.ServiceContracts;
using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using System;

namespace BookGen.Editor.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {

        private IMarkdownEditor _editor;
        public FileBrowserViewModel FileExplorer { get; }

        public IMarkdownEditor Editor
        {
            get { return _editor; }
            set
            {
                if (_editor != value)
                {
                    _editor = value;
                    RaisePropertyChanged(nameof(Editor));
                }
            }
        }

        public ICommand DialogInsertPictureCommand { get; }
        public ICommand DialogInsertLinkCommand { get; }
        public ICommand DialogFindReplaceCommand { get; }
        public ICommand DialogGotoLineCommand { get; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IFileSystemServices fileSystemServices,
                             IExceptionHandler exceptionHandler)
        {
            FileExplorer = new FileBrowserViewModel(fileSystemServices, exceptionHandler);
            FileExplorer.RootDir = EditorSessionManager.CurrentSession.WorkDirectory;

            DialogInsertPictureCommand = new RelayCommand(OnInsertPicture);
            DialogInsertLinkCommand = new RelayCommand(OnInsertLink);
            DialogFindReplaceCommand = new RelayCommand<string>(OnFindReplace);
            DialogGotoLineCommand = new RelayCommand(OnGotoLine);

        }

        private void OnGotoLine()
        {
            int line = -1;
            var result = Editor?.DialogService.ShowGotoLineDialog(Editor.Document, Editor.CaretOffset, out line);
            if (result == true)
            {
                Editor.ScrollToLine(line);
                Editor.CaretOffset = Editor.Document.GetLineByNumber(line).Offset;
            }
        }

        private void OnFindReplace(string param)
        {
            if (string.IsNullOrEmpty(param))
                Editor?.ShowFindDialog();
            else
                Editor?.ShowReplaceDialog();
        }

        private void OnInsertLink()
        {
            string link = null;
            string linktext = null;
            var result = Editor?.DialogService.ShowInsertLinkDialog(out link, out linktext);
            if (result == true)
            {
                string md;
                if (string.IsNullOrEmpty(linktext))
                    md = link;
                else
                    md = $"[{linktext}]({link})";

                Editor.InsertstringAtCaretPos(md);
            }

        }

        private void OnInsertPicture()
        {
            throw new NotImplementedException();
        }
    }
}