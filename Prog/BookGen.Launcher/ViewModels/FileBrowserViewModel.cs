using BookGen.Launcher.ViewModels.Commands;
using BookGen.Launcher.ViewModels.FileBrowser;

namespace BookGen.Launcher.ViewModels
{
    internal class FileBrowserViewModel : ObservableObject
    {
        private string _currentDir;

        public string CurrentDir
        {
            get => _currentDir;
            set
            {
                SetProperty(ref _currentDir, value);
                Update();
            }
        }

        public ObservableCollectionEx<FileBrowserItemViewModel> Items { get; }
        public ObservableCollectionEx<FileBrowserTreeViewModel> TreeItems { get; }
        public RunVsCodeCommand RunVsCodeCommand { get; }
        public StartShellCommand StartShellCommand { get; }
        public RelayCommand RefreshCommand { get; }
        public RelayCommand<FileBrowserTreeViewModel> TreeItemSelectedCommand {get; }

        public FileBrowserViewModel(string currentDir)
        {
            _currentDir = currentDir;
            Items = new ObservableCollectionEx<FileBrowserItemViewModel>();
            TreeItems = new ObservableCollectionEx<FileBrowserTreeViewModel>();
            RunVsCodeCommand = new RunVsCodeCommand();
            StartShellCommand = new StartShellCommand();
            RefreshCommand = new RelayCommand(Update);
            TreeItemSelectedCommand = new RelayCommand<FileBrowserTreeViewModel>(OnTreeSelected);
            FillDirectory(currentDir);
            Update();
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
