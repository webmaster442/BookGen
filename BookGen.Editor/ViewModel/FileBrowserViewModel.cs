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
    internal class FileBrowserViewModel: ViewModelBase
    {
        private ObservableCollection<FileItem> _files;
        private ObservableCollection<DirectoryItem> _directories;
        private string _currentdirectory;
        private string _rootDir;

        public FileBrowserViewModel()
        {
            ChangeDirectory = new DelegateCommand<DirectoryItem>(OnChangeDir, CanChangeDir);
        }

        private void OnChangeDir(DirectoryItem obj)
        {
            CurrentDirectory = obj.FullPath;
        }

        private bool CanChangeDir(DirectoryItem obj)
        {
            return obj != null;
        }

        public ICommand ChangeDirectory { get; }

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

        public string RootDir
        {
            get { return _rootDir; }
            set
            {
                if (SetValue(ref _rootDir, value))
                {
                    CurrentDirectory = _rootDir;
                    Directories = new ObservableCollection<DirectoryItem>(FileSystemServices.GetDirectories(_rootDir));
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
                }
            }
        }
    }
}
