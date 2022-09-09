using BookGen.Launcher.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;

namespace BookGen.Launcher
{
    internal class MainWindowViewModel : ObservableObject
    {
        private bool _isPopupOpen;
        private bool _isMenuOpen;
        private INotifyPropertyChanged? _popupContent;
        private INotifyPropertyChanged? _mainContent;

        public RelayCommand ClosePopupCommand { get; }
        public RelayCommand<string> OpenBrowserCommand { get; }

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
            ClosePopupCommand = new RelayCommand(OnClosePopup);
            OpenBrowserCommand = new RelayCommand<string>(OnOpenBrowser);
            OpenContent(new ViewModels.StartViewModel());
        }

        private void OnOpenBrowser(string? obj)
        {
            OpenPopupContent(new ViewModels.WebViewModel(obj));
        }

        private void OpenContent(INotifyPropertyChanged viewModel)
        {
            MainContent = viewModel;
            PopupContent = NullViewModel.Instance;
            IsPopupOpen = false;
            IsMenuOpen = false;
        }

        private void OpenPopupContent(INotifyPropertyChanged viewModel)
        {
            PopupContent = viewModel;
            IsMenuOpen = false;
            IsPopupOpen = true;
        }

        private void OnClosePopup()
        {
            PopupContent = NullViewModel.Instance;
            IsPopupOpen = false;
        }

    }
}
