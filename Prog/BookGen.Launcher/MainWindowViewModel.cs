namespace BookGen.Launcher
{
    internal class MainWindowViewModel : ObservableObject, IMainViewModel
    {
        private bool _isPopupOpen;
        private bool _isMenuOpen;
        private INotifyPropertyChanged? _popupContent;
        private INotifyPropertyChanged? _mainContent;
        private string _popupTitle;

        public RelayCommand ClosePopupCommand { get; }
        public RelayCommand OpenSettingsCommand { get; }
        public RelayCommand StartCommand { get; }
        public RelayCommand<string> OpenBrowserCommand { get; }

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

        public INotifyPropertyChanged? PopupContent
        {
            get => _popupContent;
            set => SetProperty(ref _popupContent, value);
        }

        public INotifyPropertyChanged? MainContent
        {
            get => _mainContent;
            set => SetProperty(ref _mainContent, value);
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set => SetProperty(ref _isPopupOpen, value);
        }

        public bool IsMenuOpen
        {
            get => _isMenuOpen;
            set => SetProperty(ref _isMenuOpen, value);
        }

        public MainWindowViewModel()
        {
            _popupTitle = string.Empty;
            ClosePopupCommand = new RelayCommand(OnClosePopup);
            OpenBrowserCommand = new RelayCommand<string>(OnOpenBrowser);
            OpenSettingsCommand = new RelayCommand(OnOpenSettings);
            StartCommand = new RelayCommand(OnStart);
            OnStart();
        }

        private void OnStart()
        {
            OpenContent(new ViewModels.StartViewModel(this));
        }

        private void OnOpenSettings()
        {
            OpenPopupContent(new ViewModels.SettingsViewModel(), "Settings");
        }

        private void OnOpenBrowser(string? obj)
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

        private void OnClosePopup()
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
            OnClosePopup();
        }
    }
}
