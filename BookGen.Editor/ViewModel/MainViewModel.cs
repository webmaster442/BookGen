//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Editor.EditorControl;
using BookGen.Editor.Infrastructure;
using BookGen.Editor.Models;
using BookGen.Editor.ServiceContracts;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;

namespace BookGen.Editor.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private IMarkdownEditor _editor;
        private readonly IExceptionHandler _exceptionHandler;
        private FsPath _editedFile;
        private string _editedFileHash;
        private bool _editEnabled;

        public FileBrowserViewModel FileExplorer { get; }
        public BuildViewModel BuildModel { get; set; }

        public IMarkdownEditor Editor
        {
            get { return _editor; }
            set
            {
                Set(ref _editor, value);
                FormatTableCommand = new ReformatTableCommand(_editor, _exceptionHandler);
            }
        }

        public FsPath EditedFile
        {
            get { return _editedFile; }
            set
            {
                _editedFile = value;
                Editor.Text = _editedFile?.ReadFile();
                _editedFileHash = HashUtils.GetSHA1(Editor.Text);
                RaisePropertyChanged(nameof(EditedFile));
                EditorEnabled = value?.IsExisting == true;
            }
        }

        public bool EditorEnabled
        {
            get { return _editEnabled; }
            set { Set(ref _editEnabled, value); }
        }

        public ICommand SaveCommand { get; }
        public ICommand DialogInsertPictureCommand { get; }
        public ICommand DialogInsertLinkCommand { get; }
        public ICommand DialogFindReplaceCommand { get; }
        public ICommand DialogGotoLineCommand { get; }
        public ICommand FormatTableCommand { get; set; }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IFileSystemServices fileSystemServices,
                             IExceptionHandler exceptionHandler,
                             IDialogService dialogService)
        {
            _exceptionHandler = exceptionHandler;
            FileExplorer = new FileBrowserViewModel(fileSystemServices, exceptionHandler, dialogService);
            FileExplorer.RootDir = EditorSessionManager.CurrentSession.WorkDirectory;
            BuildModel = new BuildViewModel(_exceptionHandler, dialogService);

            SaveCommand = new RelayCommand(OnSave, OnCanSave);
            DialogInsertPictureCommand = new RelayCommand(OnInsertPicture);
            DialogInsertLinkCommand = new RelayCommand(OnInsertLink);
            DialogFindReplaceCommand = new RelayCommand<string>(OnFindReplace);
            DialogGotoLineCommand = new RelayCommand(OnGotoLine);
            MessengerInstance.Register<OpenFileMessage>(this, OnOpenFile);

        }

        private void OnOpenFile(OpenFileMessage obj)
        {
            EditedFile = obj.File;
        }

        private bool OnCanSave()
        {
            return _editedFile!= null
                && _editedFile.IsExisting
                && _editedFileHash != HashUtils.GetSHA1(Editor.Text);
        }

        private void OnSave()
        {
            _editedFile.WriteFile(Editor.Text);
            _editedFileHash = HashUtils.GetSHA1(Editor.Text);
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
            bool figure = false;
            string url = null;
            string alt = null;
            var result = Editor?.DialogService.ShowInsertPictureDialog(out figure, out url, out alt);
            if (result == true)
            {
                string md = "";
                if (figure)
                    md = $"^^^\r\n![{alt}]({url})\r\n^^^{alt}\r\n";
                else
                    md = $"![{alt}]({url})";

                Editor.InsertstringAtCaretPos(md);
            }
        }
    }
}