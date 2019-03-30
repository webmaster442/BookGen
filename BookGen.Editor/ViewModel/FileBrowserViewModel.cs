//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Framework;
using BookGen.Editor.Model;
using BookGen.Editor.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BookGen.Editor.ViewModel
{
    internal sealed class FileBrowserViewModel: ViewModelBase, IDisposable
    {
        private ObservableCollection<FileItem> _files;
        private ObservableCollection<DirectoryItem> _directories;
        private string _currentdirectory;
        private string _rootDir;
        private NofityModel _nofityModel;
        private FileItem _selectedFile;

        public FileBrowserViewModel()
        {
            ChangeDirectory = new DelegateCommand<DirectoryItem>(OnChangeDir, CanChangeDir);
            _nofityModel = new NofityModel();
            _nofityModel.RefreshCurrentDirFiles += _nofityModel_RefreshCurrentDirFiles;
            _nofityModel.RefreshDirectoryTree += _nofityModel_RefreshDirectoryTree;

            CreateFile = new DelegateCommand<string>(OnCreateFile, CanCreate);
            CreateDirectory = new DelegateCommand<string>(OnCreateDirectory, CanCreate);
            DeleteFile = DelegateCommand.CreateCommand(OnDeleteFile, IsFileSelected);
            RenameFile = DelegateCommand.CreateCommand(OnRenameFile, IsFileSelected);
            OpenFile = DelegateCommand.CreateCommand(OnOpenFile, IsFileSelected);
            EditFile = DelegateCommand.CreateCommand(OnEditFile, CanEditFile);
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
            get { return _directories;  }
            set { SetValue(ref _directories, value); }
        }

        public ObservableCollection<FileItem> Files
        {
            get { return _files; }
            set { SetValue(ref _files, value); }
        }

        public FileItem SelectedFile
        {
            get { return _selectedFile; }
            set { SetValue(ref _selectedFile, value); }
        }

        public string RootDir
        {
            get { return _rootDir; }
            set
            {
                if (SetValue(ref _rootDir, value))
                {
                    CurrentDirectory = _rootDir;
                    Directories = new ObservableCollection<DirectoryItem>(FileSystemServices.GetDirectories(_rootDir));
                    _nofityModel.RootDir = _rootDir;
                }
            }
        }

        public string CurrentDirectory
        {
            get { return _currentdirectory; }
            set
            {
                if (SetValue(ref _currentdirectory, value))
                {
                    Files = new ObservableCollection<FileItem>(FileSystemServices.ListDirectory(_currentdirectory));
                    _nofityModel.CurrentDirectory = _currentdirectory;
                }
            }
        }

        private void _nofityModel_RefreshDirectoryTree(object sender, EventArgs e)
        {
            Directories = new ObservableCollection<DirectoryItem>(FileSystemServices.GetDirectories(_rootDir));
        }

        private void _nofityModel_RefreshCurrentDirFiles(object sender, EventArgs e)
        {
            Files = new ObservableCollection<FileItem>(FileSystemServices.ListDirectory(_currentdirectory));
        }

        private void OnChangeDir(DirectoryItem obj)
        {
            CurrentDirectory = obj.FullPath;
        }

        private bool CanChangeDir(DirectoryItem obj)
        {
            return obj != null;
        }

        private bool IsFileSelected(object obj)
        {
            return SelectedFile != null;
        }

        private void OnRenameFile(object obj)
        { 
            if (SelectedFile == null) return;
            ExceptionHandler.SafeRun(() => FileSystemServices.RenameFile(SelectedFile.FullPath));
        }

        private void OnDeleteFile(object obj)
        {
            if (SelectedFile == null) return;
            ExceptionHandler.SafeRun(() => FileSystemServices.DeleteFile(SelectedFile.FullPath));
        }

        private bool CanCreate(string obj)
        {
            return !string.IsNullOrEmpty(CurrentDirectory);
        }

        private void OnCreateDirectory(string obj)
        {
            ExceptionHandler.SafeRun(() => FileSystemServices.CreateFolder(CurrentDirectory));
        }

        private void OnCreateFile(string obj)
        {
            ExceptionHandler.SafeRun(() => FileSystemServices.CreateFile(CurrentDirectory));
        }

        private void OnOpenFile(object obj)
        {
            if (SelectedFile == null) return;
            ExceptionHandler.SafeRun(() => System.Diagnostics.Process.Start(SelectedFile.FullPath));
        }

        private bool CanEditFile(object obj)
        {
            return
                SelectedFile != null
                && string.Equals(SelectedFile.FileType, ".md", StringComparison.OrdinalIgnoreCase);
        }

        private void OnEditFile(object obj)
        {
            if (SelectedFile == null) return;
            ExceptionHandler.SafeRun(() => EditorServices.LaunchEditorFor(SelectedFile.FullPath));
        }
    }
}
