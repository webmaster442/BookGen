using System.Diagnostics;
using System.Text.Json;

namespace BookGen.Launcher.ViewModels
{
    internal class StartViewModel : ObservableObject
    {
        private List<string> _elements;
        private string _filter;
        private readonly string _fileName;
        private readonly IMainViewModel _mainViewModel;

        public string Version { get; }

        public BindingList<ItemViewModel> View { get; }

        public RelayCommand<string> OpenFolderCommand { get; }
        public RelayCommand<string> RemoveFolderCommand { get; }
        public RelayCommand<string> FolderSelectCommand { get; }
        public RelayCommand ClearFoldersCommand { get; }

        public StartViewModel(IMainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _filter = string.Empty;
            _elements = new List<string>();
            _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "bookgenlauncher.json");

            OpenFolderCommand = new RelayCommand<string>(OnOpenFolder);
            ClearFoldersCommand = new RelayCommand(OnClearFolders);
            RemoveFolderCommand = new RelayCommand<string>(OnRemoveFolder);
            FolderSelectCommand = new RelayCommand<string>(OnFolderSelect);

            View = new BindingList<ItemViewModel>();
            Version = GetVersion();

            LoadFolderList();
        }

        private void LoadFolderList()
        {
            string? json = ReadFile();

            if (!string.IsNullOrEmpty(json))
            {
                string[]? deserialized = JsonSerializer.Deserialize<string[]>(json);
                if (deserialized != null)
                {
                    _elements = new List<string>(deserialized);
                }
            }

            string[] arguments = Environment.GetCommandLineArgs();
            if (arguments.Length == 2
                && Directory.Exists(arguments[1]))
            {
                _elements.Add(arguments[1]);
            }

            App.UpdateJumplist(_elements);
            ApplyFilter();
        }

        public string Filter
        {
            get => _filter;
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    ApplyFilter();
                    OnPropertyChanged(nameof(Filter));
                }
            }
        }

        public bool IsEmpty => View.Count < 1;

        public void SaveFolders()
        {
            string? text = JsonSerializer.Serialize(_elements);
            WriteFile(text);
            App.UpdateJumplist(_elements);
        }

        private void ApplyFilter()
        {
            if (!string.IsNullOrEmpty(Filter))
            {
                IEnumerable<string>? subset = _elements.Where(x => x.Contains(Filter));
                CreateItems(subset);
            }
            else
            {
                CreateItems(_elements);
            }
            OnPropertyChanged(nameof(IsEmpty));
        }

        private void CreateItems(IEnumerable<string> subset)
        {
            View.RaiseListChangedEvents = false;
            View.Clear();
            foreach (string? item in subset)
            {
                View.Add(new ItemViewModel
                {
                    FullPath = item
                });
            }
            View.RaiseListChangedEvents = true;
            View.ResetBindings();
        }

        private string GetVersion()
        {
            System.Reflection.AssemblyName? name = typeof(App).Assembly.GetName();
            return name?.Version?.ToString() ?? "Couldn't get version";
        }

        private string ReadFile()
        {
            if (File.Exists(_fileName))
            {
                try
                {
                    return File.ReadAllText(_fileName);
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        private void WriteFile(string content)
        {
            try
            {
                File.WriteAllText(_fileName, content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void OnOpenFolder(string? obj)
        {
            if (Dialog.TryselectFolderDialog(out string selected))
            {
                if (_elements.Contains(selected))
                {
                    _elements.Remove(selected);
                    _elements.Insert(0, selected);
                }
                else
                {
                    _elements.Insert(0, selected);
                }
                ApplyFilter();
                SaveFolders();
            }
        }

        private void OnClearFolders()
        {
            if (Dialog.ShowMessageBox(Properties.Resources.ClearRecentList,
                                  Properties.Resources.Question,
                                  MessageBoxButton.YesNo,
                                  MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _elements.Clear();
                ApplyFilter();
                SaveFolders();
            }
        }

        private void OnRemoveFolder(string? obj)
        {
            if (obj is string folder)
            {
                MessageBoxResult confirm = Dialog.ShowMessageBox(
                    string.Format(Properties.Resources.RemoveFolder, folder),
                    Properties.Resources.Question,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    _elements.Remove(folder);
                    ApplyFilter();
                    SaveFolders();
                }
            }
        }

        private void OnFolderSelect(string? obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                _mainViewModel.OpenContent(new ViewModels.FileBrowserViewModel(_mainViewModel, obj));
            }
        }
    }
}
