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

        public ObservableCollection<ItemViewModel> Items { get; }

        public MainViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>(GetModels());
            OpenFolderCommand = new DelegateCommand(OnOpenFolder);
            ClearFoldersCommand = new DelegateCommand(OnClearFolders);
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
