//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Launcher.ViewModels.Commands;
using BookGen.Launcher.ViewModels.FileBrowser;

namespace BookGen.Launcher.ViewModels
{
    internal sealed class FileBrowserViewModel : ObservableObject
    {
        private readonly IMainViewModel _mainViewModel;
        private string _currentDir;

        public string CurrentDir
        {
            get => _currentDir;
            set
            {
                SetProperty(ref _currentDir, value);
                TaskRunnerCommand.Folder = _currentDir;
                Update();
            }
        }

        public ObservableCollectionEx<FileBrowserItemViewModel> Items { get; }
        public ObservableCollectionEx<FileBrowserTreeViewModel> TreeItems { get; }
        public ObservableCollectionEx<BookGenTask> TaskItems { get; }

        public RunVsCodeCommand RunVsCodeCommand { get; }
        public StartShellCommand StartShellCommand { get; }
        public RunExplorerCommand RunExplorerCommand { get; }
        public TaskRunnerCommand TaskRunnerCommand { get; }
        public RelayCommand RefreshCommand { get; }
        public RelayCommand<FileBrowserTreeViewModel> TreeItemSelectedCommand { get; }
        public RelayCommand<FileBrowserItemViewModel> PreviewCommand { get; }

        public FileBrowserViewModel(IMainViewModel mainViewModel, string currentDir)
        {
            _mainViewModel = mainViewModel;
            _currentDir = currentDir;
            TaskRunnerCommand = new TaskRunnerCommand();
            Items = new ObservableCollectionEx<FileBrowserItemViewModel>();
            TreeItems = new ObservableCollectionEx<FileBrowserTreeViewModel>();
            TaskItems = new ObservableCollectionEx<BookGenTask>(new BookGenTasks());
            RunVsCodeCommand = new RunVsCodeCommand();
            StartShellCommand = new StartShellCommand();
            RunExplorerCommand = new RunExplorerCommand();
            RefreshCommand = new RelayCommand(Update);
            TreeItemSelectedCommand = new RelayCommand<FileBrowserTreeViewModel>(OnTreeSelected);
            PreviewCommand = new RelayCommand<FileBrowserItemViewModel>(OnPreview);
            FillDirectory(currentDir);
            Update();
        }

        private async void OnPreview(FileBrowserItemViewModel? obj)
        {
            if (obj != null)
            {
                if (PreviewHelper.IsMarkDown(obj.FullPath))
                {
                    (bool result, string output) = await PreviewHelper.BookGenExport(obj.FullPath, false, true);
                    if (!result
                        && !string.IsNullOrEmpty(output))
                    {
                        Dialog.ShowMessageBox(output, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    _mainViewModel.OpenPopupContent(new WebViewModel(output), obj.FullPath);
                    return;
                }
                _mainViewModel.OpenPopupContent(new PreviewViewModel(obj.FullPath), obj.FullPath);
            }
        }

        private void OnTreeSelected(FileBrowserTreeViewModel? obj)
        {
            if (obj != null)
            {
                CurrentDir = obj.FullPath;
            }
        }

        private void FillDirectory(string startDir)
        {
            TreeItems.Clear();
            TreeItems.AddRange(ModelsFactory.CreateTreeModels(startDir));
        }

        private void Update()
        {
            Items.Clear();
            Items.AddRange(ModelsFactory.CreateItemModels(CurrentDir));
            StartShellCommand.OnCanExecuteChanged();
            RunVsCodeCommand.OnCanExecuteChanged();
        }
    }
}
