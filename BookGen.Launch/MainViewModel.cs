using BookGen.Gui.Wpf;
using BookGen.Launch.Code;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BookGen.Launch
{
    internal class MainViewModel : ViewModelBase
    {
        public DelegateCommand OpenFolderCommand { get; }
        public DelegateCommand ClearFoldersCommand { get; }
        public DelegateCommand OpenWebsiteCommand { get; }

        public DelegateCommand StartShellCommand { get; }
        public DelegateCommand StartPreviewCommand { get; }
        public DelegateCommand OpenSelectedFolderCommand { get; }
        public DelegateCommand OpenInVsCodeCommand { get; }

        public ObservableCollection<ItemViewModel> Items { get; }

        public MainViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>(GetModels());
            OpenFolderCommand = new DelegateCommand(OnOpenFolder);
            ClearFoldersCommand = new DelegateCommand(OnClearFolders);
            OpenWebsiteCommand = new DelegateCommand(OnOpenWebsite);

            StartShellCommand = new DelegateCommand(OnStartShell);
            StartPreviewCommand = new DelegateCommand(OnStartPreview);
            OpenSelectedFolderCommand = new DelegateCommand(OnOpenSelectedFolder);
            OpenInVsCodeCommand = new DelegateCommand(OnOpenVsCodeCommand);
        }

        private void OnOpenVsCodeCommand(object? obj)
        {
        }

        private void OnOpenSelectedFolder(object? obj)
        {
        }

        private void OnStartPreview(object? obj)
        {
        }

        private void OnStartShell(object? obj)
        {

        }

        private void OnClearFolders(object? obj)
        {
            if (MessageBox.Show(Properties.Resources.ClearRecentList,
                    Properties.Resources.Question,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Items.Clear();
                SaveList();
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
                Items.Add(new ItemViewModel
                {
                    FullPath = selected
                });
                SaveList();
            }
        }

        private void SaveList()
        {
            var list = Items.Select(x => x.FullPath).ToList();
            FolderList.SaveFolders(list);
            App.UpdateJumplist(list);
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

        private IEnumerable<ItemViewModel> GetModels()
        {
            return FolderList.GetFolders().Select(x => new ItemViewModel
            {
                FullPath = x
            });
        }
    }
}
