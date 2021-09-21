//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Wpf;
using BookGen.Launch.Code;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BookGen.Launch
{
    internal class MainViewModel : ViewModelBase
    {
        public DelegateCommand OpenFolderCommand { get; }
        public DelegateCommand ClearFoldersCommand { get; }
        public DelegateCommand OpenWebsiteCommand { get; }

        public DelegateCommand InstallPathVariableCommand { get; }
        public DelegateCommand ShowChangeLogCommand { get; }

        public ICommand StartShellCommand { get; }
        public ICommand StartPreviewCommand { get; }
        public ICommand OpenSelectedFolderCommand { get; }
        public ICommand OpenInVsCodeCommand { get; }
        public ICommand CheckUpdateCommand { get; }
        public DelegateCommand RemoveItemCommand { get; }

        public FolderList FolderList { get; }

        public string Version { get; }

        private const string EnvPathVariable = "PATH";
        private readonly IMainWindow _mainWindow;

        public MainViewModel(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            FolderList = new FolderList();
            OpenFolderCommand = new DelegateCommand(OnOpenFolder);
            ClearFoldersCommand = new DelegateCommand(OnClearFolders);
            OpenWebsiteCommand = new DelegateCommand(OnOpenWebsite);

            InstallPathVariableCommand = new DelegateCommand(OnInstallPath, OnCaninstallPath);
            ShowChangeLogCommand = new DelegateCommand((o) => _mainWindow.ShowChangeLog());

            StartShellCommand = new StartShellCommand();
            StartPreviewCommand = new RunProgramCommand("BookGen.exe", "preview");
            CheckUpdateCommand = new RunProgramCommand("BookGen.exe", "Update");
            OpenSelectedFolderCommand = new RunProgramCommand("Explorer.exe", "");
            OpenInVsCodeCommand = new RunVsCodeCommand();
            RemoveItemCommand = new DelegateCommand(OnRemoveItem);
            Version = GetVersion();

            ProcessArguments();
        }

        private void ProcessArguments()
        {
            var args = Environment.GetCommandLineArgs().ToList();
            var launchIndex = args.IndexOf("launch");
            if (launchIndex > -1 &&
                (launchIndex + 1) < args.Count)
            {
                StartShellCommand.Execute(args[launchIndex + 1]);
            }
        }

        private void OnRemoveItem(object? obj)
        {
            if (obj is string folder)
            {
                MessageBoxResult confirm = MessageBox.Show(
                    string.Format(Properties.Resources.RemoveFolder, folder),
                    Properties.Resources.Question,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    FolderList.Remove(folder);
                }
            }
        }

        private string GetVersion()
        {
            var name = typeof(App).Assembly.GetName();
            return name?.Version?.ToString() ?? "Couldn't get version";
        }

        private bool OnCaninstallPath(object? obj)
        {
            const EnvironmentVariableTarget scope = EnvironmentVariableTarget.User; // or User
            string? oldValue = Environment.GetEnvironmentVariable(EnvPathVariable, scope);

            return oldValue != null
                && !oldValue.Contains(AppContext.BaseDirectory);
        }

        private void OnInstallPath(object? obj)
        {
            if (MessageBoxEx.Show(Properties.Resources.InstallToPathVar,
                                  Properties.Resources.Question,
                                  MessageBoxButton.YesNo,
                                  MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                const EnvironmentVariableTarget scope = EnvironmentVariableTarget.User; // or User
                string? oldValue = Environment.GetEnvironmentVariable(EnvPathVariable, scope);
                string? newValue = oldValue + ";" + AppContext.BaseDirectory;
                Environment.SetEnvironmentVariable(EnvPathVariable, newValue, scope);
                InstallPathVariableCommand.RaiseCanExecuteChanged();
            }
        }

        private void OnClearFolders(object? obj)
        {
            if (MessageBoxEx.Show(Properties.Resources.ClearRecentList,
                                  Properties.Resources.Question,
                                  MessageBoxButton.YesNo,
                                  MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                FolderList.Clear();
            }
        }

        private void OnOpenWebsite(object? obj)
        {
            using (var p = new System.Diagnostics.Process())
            {
                p.StartInfo.FileName = "https://webmaster442.github.io/BookGen/";
                p.StartInfo.UseShellExecute = true;
                p.Start();
            }
        }

        private void OnOpenFolder(object? obj)
        {
            if (TryselectFolder(out string selected))
            {
                FolderList.Add(selected);
            }
        }

        private static bool TryselectFolder(out string folderPath)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog
            {
                Description = Properties.Resources.FolderselectDescription,
                UseDescriptionForTitle = true,
                ShowNewFolderButton = false,
            };
            if (dialog.ShowDialog() == true)
            {
                folderPath = dialog.SelectedPath;
                return true;
            }
            else
            {
                folderPath = string.Empty;
                return false;
            }
        }
    }
}
