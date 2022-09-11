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

        public FileBrowserViewModel(string currentDir)
        {
            _currentDir = currentDir;
            Items = new ObservableCollectionEx<FileBrowserItemViewModel>();
            TreeItems = new ObservableCollectionEx<FileBrowserTreeViewModel>();
            FillDirectory(currentDir);
            Update();
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
        }
    }
}
