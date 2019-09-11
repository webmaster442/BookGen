//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Models;
using BookGen.Editor.ServiceContracts;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using BookGen.Editor.Views.Dialogs;

namespace BookGen.Editor.ViewModel
{
    internal sealed class FileBrowserViewModel: ViewModelBase, IDisposable
    {
        private readonly IFileSystemServices _fileSystemServices;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IDialogService _dialogService;

        private ObservableCollection<FileItem> _files;
        private ObservableCollection<DirectoryItem> _directories;
        private string _currentdirectory;
        private string _rootDir;
        private string _filter;
        private NofityModel _nofityModel;
        private FileItem _selectedFile;

        public FileBrowserViewModel(IFileSystemServices fileSystemServices,
                                    IExceptionHandler exceptionHandler,
                                    IDialogService dialogService)
        {
            _fileSystemServices = fileSystemServices;
            _exceptionHandler = exceptionHandler;
            _dialogService = dialogService;

            ChangeDirectory = new RelayCommand<DirectoryItem>(OnChangeDir, CanChangeDir);
            _nofityModel = new NofityModel();
            _nofityModel.RefreshCurrentDirFiles += _nofityModel_RefreshCurrentDirFiles;
            _nofityModel.RefreshDirectoryTree += _nofityModel_RefreshDirectoryTree;

            CreateFile = new RelayCommand<string>(OnCreateFile, CanCreate);
            CreateDirectory = new RelayCommand<string>(OnCreateDirectory, CanCreate);
            DeleteFile = new RelayCommand(OnDeleteFile, IsFileSelected);
            RenameFile = new RelayCommand(OnRenameFile, IsFileSelected);
            OpenFile = new RelayCommand(OnOpenFile, IsFileSelected);
            EditFile = new RelayCommand(OnEditFile, CanEditFile);
        }

        public void Dispose()
        {
            if (_nofityModel != null)
            {
                _nofityModel.Dispose();
                _nofityModel = null;
            }
        }

        public ICommand ChangeDirectory { get; }
        public ICommand CreateFile { get; }
        public ICommand CreateDirectory { get; }
        public ICommand DeleteFile { get; }
        public ICommand RenameFile { get; }
        public ICommand OpenFile { get; }
        public ICommand EditFile { get; }

        public ObservableCollection<DirectoryItem> Directories
        {
            get { return _directories; }
            set { Set(ref _directories, value); }
        }

        public ObservableCollection<FileItem> Files
        {
            get { return _files; }
            set { Set(ref _files, value); }
        }

        public FileItem SelectedFile
        {
            get { return _selectedFile; }
            set { Set(ref _selectedFile, value); }
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                Set(ref _filter, value);
                Files = new ObservableCollection<FileItem>(_fileSystemServices.ListDirectory(_currentdirectory, _filter));
            }
        }

        public string RootDir
        {
            get { return _rootDir; }
            set
            {
                if (Set(ref _rootDir, value))
                {
                    CurrentDirectory = _rootDir;
                    Directories = new ObservableCollection<DirectoryItem>(_fileSystemServices.GetDirectories(_rootDir));
                    _nofityModel.RootDir = _rootDir;
                }
            }
        }

        public string CurrentDirectory
        {
            get { return _currentdirectory; }
            set
            {
                if (Set(ref _currentdirectory, value))
                {
                    _filter = string.Empty;
                    Files = new ObservableCollection<FileItem>(_fileSystemServices.ListDirectory(_currentdirectory));
                    _nofityModel.CurrentDirectory = _currentdirectory;
                    RaisePropertyChanged(nameof(Filter));
                }
            }
        }

        private void _nofityModel_RefreshDirectoryTree(object sender, EventArgs e)
        {
            Directories = new ObservableCollection<DirectoryItem>(_fileSystemServices.GetDirectories(_rootDir));
        }

        private void _nofityModel_RefreshCurrentDirFiles(object sender, EventArgs e)
        {
            Files = new ObservableCollection<FileItem>(_fileSystemServices.ListDirectory(_currentdirectory));
            _filter = string.Empty;
            RaisePropertyChanged(nameof(Filter));
        }

        private void OnChangeDir(DirectoryItem obj)
        {
            CurrentDirectory = obj.FullPath;
        }

        private bool CanChangeDir(DirectoryItem obj)
        {
            return obj != null;
        }

        private bool IsFileSelected()
        {
            return SelectedFile != null;
        }

        private void OnRenameFile()
        {
            if (SelectedFile == null) return;
            _exceptionHandler.SafeRun(() => _fileSystemServices.RenameFile(SelectedFile.FullPath));
        }

        private void OnDeleteFile()
        {
            if (SelectedFile == null) return;
            _exceptionHandler.SafeRun(() => _fileSystemServices.DeleteFile(SelectedFile.FullPath));
        }

        private bool CanCreate(string s)
        {
            return !string.IsNullOrEmpty(CurrentDirectory);
        }

        private void OnCreateDirectory(string s)
        {
            _exceptionHandler.SafeRun(() => _fileSystemServices.CreateFolder(CurrentDirectory));
        }

        private void OnCreateFile(string s)
        {
            _exceptionHandler.SafeRun(() => _fileSystemServices.CreateFile(CurrentDirectory));
        }

        private void OnOpenFile()
        {
            if (SelectedFile == null) return;
            _exceptionHandler.SafeRun(() => System.Diagnostics.Process.Start(SelectedFile.FullPath));
        }

        private bool CanEditFile()
        {
            return
                SelectedFile != null
                && string.Equals(SelectedFile.FileType, ".md", StringComparison.OrdinalIgnoreCase);
        }

        private void OnEditFile()
        {
            if (SelectedFile == null) return;
            MessengerInstance.Send(new OpenFileMessage
            {
                File = new Core.FsPath(SelectedFile.FullPath)
            });
            _dialogService.CloseFlyouts();
        }
    }
}
