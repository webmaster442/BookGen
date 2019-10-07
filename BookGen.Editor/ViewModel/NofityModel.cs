//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using GalaSoft.MvvmLight;
using System;
using System.IO;

namespace BookGen.Editor.ViewModel
{
    internal sealed class NofityModel : ViewModelBase, IDisposable
    {
        private string _currentdirectory;
        private string _rootDir;
        private FileSystemWatcher _watcher;

        public event EventHandler RefreshDirectoryTree;
        public event EventHandler RefreshCurrentDirFiles;

        public string RootDir
        {
            get { return _rootDir; }
            set
            {
                if (Set(ref _rootDir, value))
                {
                    CreateWatcher();
                }
            }
        }

        public string CurrentDirectory
        {
            get { return _currentdirectory; }
            set { Set(ref _currentdirectory, value); }
        }

        private void DeleteWatcher()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Created -= _watcher_CreeteDeleteChange;
                _watcher.Renamed -= _watcher_Renamed;
                _watcher.Deleted -= _watcher_CreeteDeleteChange;
                _watcher.Changed -= _watcher_CreeteDeleteChange;
                _watcher.Dispose();
                _watcher = null;
            }
        }

        private void CreateWatcher()
        {
            if (_watcher == null)
                _watcher = new FileSystemWatcher();

            _watcher.Path = _rootDir;
            _watcher.NotifyFilter = NotifyFilters.FileName
                                    | NotifyFilters.DirectoryName
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.Size;

            _watcher.Created += _watcher_CreeteDeleteChange;
            _watcher.Renamed += _watcher_Renamed;
            _watcher.Deleted += _watcher_CreeteDeleteChange;
            _watcher.Changed += _watcher_CreeteDeleteChange;
            _watcher.EnableRaisingEvents = true;
        }

        private void _watcher_CreeteDeleteChange(object sender, FileSystemEventArgs e)
        {
            bool dir = Directory.Exists(e.FullPath);
            if (dir)
            {
                RefreshDirectoryTree?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                var folder = Path.GetDirectoryName(e.FullPath);
                if (_currentdirectory == folder)
                {
                    RefreshCurrentDirFiles?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void _watcher_Renamed(object sender, RenamedEventArgs e)
        {
            bool dir = Directory.Exists(e.FullPath);
            if (dir)
            {
                RefreshDirectoryTree?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                var folder = Path.GetDirectoryName(e.FullPath);
                if (_currentdirectory == folder)
                {
                    RefreshCurrentDirFiles?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void Dispose()
        {
            DeleteWatcher();
        }
    }
}
