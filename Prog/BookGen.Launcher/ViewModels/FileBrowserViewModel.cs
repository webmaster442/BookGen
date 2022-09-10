using BookGen.Launcher.ViewModels.FileBrowser;
using CommunityToolkit.Mvvm.ComponentModel;

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

        public FileBrowserViewModel(string currentDir)
        {
            _currentDir = currentDir;
            Items = new ObservableCollectionEx<FileBrowserItemViewModel>();
            Update();
        }

        private void Update()
        {
            Items.Clear();
            Items.AddRange(ItemViewModelFactory.CreateModels(CurrentDir));
        }
    }
}
