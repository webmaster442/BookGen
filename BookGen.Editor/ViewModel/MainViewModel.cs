//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Editor.Controls;
using BookGen.Editor.Infrastructure;
using BookGen.Editor.Models;
using BookGen.Editor.ServiceContracts;
using BookGen.Editor.Views.Dialogs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BookGen.Editor.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private IMarkdownEditor _editor;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IDialogService _dialogService;
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
                if (_editor != null)
                    _editor.PropertyChanged -= _editor_PropertyChanged;
                Set(ref _editor, value);
                FormatTableCommand = new ReformatTableCommand(_editor, _exceptionHandler);
                _editor.PropertyChanged += _editor_PropertyChanged;
            }
        }

        private void _editor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_editor.Text))
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public FsPath EditedFile
        {
            get { return _editedFile; }
            set
            {
                _editedFile = value;
                Editor.Text = _editedFile?.ReadFile(Locator.Resolve<ILog>());
                _editedFileHash = HashUtils.GetSHA1(Editor.Text);
                RaisePropertyChanged(nameof(EditedFile));
                EditorEnabled = value?.IsExisting == true;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public bool EditorEnabled
        {
            get { return _editEnabled; }
            set { Set(ref _editEnabled, value); }
        }

        public RelayCommand SaveCommand { get; }
        public ICommand DialogInsertPictureCommand { get; }
        public ICommand DialogInsertLinkCommand { get; }
        public ICommand DialogFindReplaceCommand { get; }
        public ICommand DialogGotoLineCommand { get; }
        public ICommand FormatTableCommand { get; set; }
        public ICommand OpenSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IFileSystemServices fileSystemServices,
                             IExceptionHandler exceptionHandler,
                             IDialogService dialogService)
        {
            _exceptionHandler = exceptionHandler;
            _dialogService = dialogService;
            FileExplorer = new FileBrowserViewModel(fileSystemServices, exceptionHandler, dialogService);
            FileExplorer.RootDir = EditorSessionManager.CurrentSession.WorkDirectory;
            BuildModel = new BuildViewModel(_exceptionHandler, dialogService);

            SaveCommand = new RelayCommand(OnSave, OnCanSave);
            DialogInsertPictureCommand = new RelayCommand(OnInsertPicture);
            DialogInsertLinkCommand = new RelayCommand(OnInsertLink);
            DialogFindReplaceCommand = new RelayCommand<string>(OnFindReplace);
            DialogGotoLineCommand = new RelayCommand(OnGotoLine);
            MessengerInstance.Register<OpenFileMessage>(this, OnOpenFile);
            OpenSettings = new RelayCommand(OnOpenSettings);
        }

        private void OnOpenSettings()
        {
            _dialogService.OpenSettings();
        }

#pragma warning disable S3168 // "async" methods should not return "void"
        private async void OnOpenFile(OpenFileMessage obj)
        {
            if (OnCanSave())
            {
                var result = await DialogCommons.ShowMessage("File Save", "File Modified since last save. Save changes?", true);
                if (result) OnSave();
            }
            EditedFile = obj.File;
        }
#pragma warning restore S3168 // "async" methods should not return "void"

        private bool OnCanSave()
        {
            return
                _editedFile != null
                && _editedFile.IsExisting
                && _editedFileHash != HashUtils.GetSHA1(Editor.Text);
        }

        private void OnSave()
        {
            _editedFile.WriteFile(Locator.Resolve<ILog>(), Editor.Text);
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
                string md;
                if (figure)
                    md = $"^^^\r\n![{alt}]({url})\r\n^^^{alt}\r\n";
                else
                    md = $"![{alt}]({url})";

                Editor.InsertstringAtCaretPos(md);
            }
        }
    }
}