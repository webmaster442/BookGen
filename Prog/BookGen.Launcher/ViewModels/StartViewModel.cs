//-----------------------------------------------------------------------------
// (c) 2021-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Threading.Tasks;

using BookGen.Settings;

namespace BookGen.Launcher.ViewModels;

internal sealed partial class StartViewModel : ObservableObject
{
    private List<string> _elements;
    private string _filter;
    private readonly IMainViewModel _mainViewModel;

    public string Version { get; }

    public BindingList<ItemViewModel> View { get; }

    public StartViewModel(IMainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        _filter = string.Empty;
        _elements = new List<string>();

        View = new BindingList<ItemViewModel>();
        Version = GetVersion();

        LoadFolderList();
    }

    private void LoadFolderList()
    {
        try
        {
            var manager = FileProvider.GetSettingsManager();
            var deserialized = manager.DeserializeAsync<string[]>(FileProvider.Keys.Launcher).GetAwaiter().GetResult();
            if (deserialized == null)
            {
                _elements = new List<string>();
            }
            else
            {
                _elements = new List<string>(deserialized);
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
        catch (Exception ex)
        {
            Dialog.ShowMessageBox(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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

    private async Task SaveFolders()
    {
        try
        {
            var manager = FileProvider.GetSettingsManager();
            await manager.SerializeAsync<List<string>>(FileProvider.Keys.Launcher, _elements);
            App.UpdateJumplist(_elements);
        }
        catch (Exception ex)
        {
            Dialog.ShowMessageBox(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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

    private static string GetVersion()
    {
        System.Reflection.AssemblyName? name = typeof(App).Assembly.GetName();
        return name?.Version?.ToString() ?? "Couldn't get version";
    }


    [RelayCommand]
    private async Task OpenFolder(string? obj)
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
            await SaveFolders();
        }
    }

    [RelayCommand]
    private async Task ClearFolders()
    {
        if (Dialog.ShowMessageBox(Properties.Resources.ClearRecentList,
                              Properties.Resources.Question,
                              MessageBoxButton.YesNo,
                              MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            _elements.Clear();
            ApplyFilter();
            await SaveFolders();
        }
    }

    [RelayCommand]
    private async Task RemoveFolder(string? obj)
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
                await SaveFolders();
            }
        }
    }

    [RelayCommand]
    private void FolderSelect(string? obj)
    {
        if (!string.IsNullOrEmpty(obj))
        {
            _mainViewModel.OpenContent(new ViewModels.FileBrowserViewModel(_mainViewModel, obj));
        }
    }
}
