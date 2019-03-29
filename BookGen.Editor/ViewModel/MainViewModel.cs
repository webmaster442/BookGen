//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Framework;
using System;
using System.Windows.Input;

namespace BookGen.Editor.ViewModel
{
    internal class MainViewModel: ViewModelBase
    {
        public FileBrowserViewModel FileBrowserModel { get; set; }
        public ActionsViewModel Actions { get; set; }
        public ICommand OpenDirectoryCommand { get; }

        public MainViewModel()
        {
            FileBrowserModel = new FileBrowserViewModel();
            Actions = new ActionsViewModel();
            FileBrowserModel.RootDir = Environment.CurrentDirectory;
            Actions.RootDir = Environment.CurrentDirectory;
            OpenDirectoryCommand = DelegateCommand.CreateCommand(OnOpenDirectory);
        }

        private void OnOpenDirectory(object obj)
        {
            using (var fb = new System.Windows.Forms.FolderBrowserDialog())
            {
                fb.Description = "Select folder";
                fb.SelectedPath = FileBrowserModel.RootDir;
                if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileBrowserModel.RootDir = fb.SelectedPath;
                    Actions.RootDir = fb.SelectedPath;
                }
            }
        }
    }
}
