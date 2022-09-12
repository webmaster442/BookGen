using BookGen.Launcher.ViewModels.Commands;
using BookGen.Launcher.ViewModels.FileBrowser;

namespace BookGen.Launcher.ViewModels
{
    internal class FileBrowserViewModel : ObservableObject
    {
        private readonly IMainViewModel _mainViewModel;
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
        public RelayCommand<FileBrowserItemViewModel> PreviewCommand { get; }

        public FileBrowserViewModel(IMainViewModel mainViewModel, string currentDir)
        {
            _mainViewModel = mainViewModel;
            _currentDir = currentDir;
            Items = new ObservableCollectionEx<FileBrowserItemViewModel>();
            TreeItems = new ObservableCollectionEx<FileBrowserTreeViewModel>();
            RunVsCodeCommand = new RunVsCodeCommand();
            StartShellCommand = new StartShellCommand();
            RefreshCommand = new RelayCommand(Update);
            TreeItemSelectedCommand = new RelayCommand<FileBrowserTreeViewModel>(OnTreeSelected);
            PreviewCommand = new RelayCommand<FileBrowserItemViewModel>(OnPreview);
            FillDirectory(currentDir);
            Update();
        }

        private void OnPreview(FileBrowserItemViewModel? obj)
        {
            if (obj != null)
            {
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
