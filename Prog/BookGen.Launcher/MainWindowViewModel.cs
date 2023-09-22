//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices;

namespace BookGen.Launcher;

internal sealed partial class MainWindowViewModel : ObservableObject, IMainViewModel
{

    private string _popupTitle;

    public string AppTitle
    {
        get
        {
            if (!string.IsNullOrEmpty(_popupTitle))
            {
                return $"BookGen Launcher - {_popupTitle}";
            }
            return "BookGen Launcher";
        }
    }

    [ObservableProperty]
    private bool _isPopupOpen;

    [ObservableProperty]
    private bool _isMenuOpen;

    [ObservableProperty]
    private bool _isTodoOpen;
    
    [ObservableProperty]
    private INotifyPropertyChanged? _popupContent;

    [ObservableProperty]
    private INotifyPropertyChanged? _mainContent;

    [ObservableProperty]
    private TodoViewModel _todoViewModel;

    public MainWindowViewModel()
    {
        _popupTitle = string.Empty;
        _todoViewModel = new TodoViewModel();
        Start();
    }

    [RelayCommand]
    private void TerminalInstall()
    {
        var result = TerminalProfileInstaller.TryInstall();
        if (result == null)
            MessageBox.Show("Windows Terminal not installed. Profile installation can't continue", "Terminal install", MessageBoxButton.OK, MessageBoxImage.Warning);
        else if (result == true)
            MessageBox.Show("Terminal profile installed", "Terminal install", MessageBoxButton.OK, MessageBoxImage.Information);
        else
            MessageBox.Show("Terminal profile install failed", "Terminal install", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    [RelayCommand]
    private void Start()
    {
        OpenContent(new ViewModels.StartViewModel(this));
    }

    [RelayCommand]
    private void OpenSettings()
    {
        OpenPopupContent(new ViewModels.SettingsViewModel(), "Settings");
    }

    [RelayCommand]
    private void OpenBrowser(string? obj)
    {
        OpenPopupContent(new ViewModels.WebViewModel(obj), obj ?? string.Empty);
    }

    private void OpenContent(INotifyPropertyChanged viewModel)
    {
        MainContent = viewModel;
        PopupContent = NullViewModel.Instance;
        IsPopupOpen = false;
        IsMenuOpen = false;
    }

    private void OpenPopupContent(INotifyPropertyChanged viewModel, string title)
    {
        _popupTitle = title;
        PopupContent = viewModel;
        IsMenuOpen = false;
        IsPopupOpen = true;
        OnPropertyChanged(nameof(AppTitle));
    }

    [RelayCommand]
    private void ClosePopup()
    {
        _popupTitle = string.Empty;
        PopupContent = NullViewModel.Instance;
        IsPopupOpen = false;
        OnPropertyChanged(nameof(AppTitle));
    }

    void IMainViewModel.OpenPopupContent(INotifyPropertyChanged viewModel, string title)
    {
        OpenPopupContent(viewModel, title);
    }

    void IMainViewModel.OpenContent(INotifyPropertyChanged viewModel)
    {
        OpenContent(viewModel);
    }

    void IMainViewModel.ClosePopup()
    {
        ClosePopup();
    }
}
