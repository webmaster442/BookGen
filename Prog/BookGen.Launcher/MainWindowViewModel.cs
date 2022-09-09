using BookGen.Launcher.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace BookGen.Launcher
{
    internal class MainWindowViewModel : ObservableObject
    {
        private bool _isPopupOpen;
        private INotifyPropertyChanged? _popupContent;
        private INotifyPropertyChanged? _mainContent;

        public MainWindowViewModel()
        {
            ClosePopupCommand = new RelayCommand(OnClosePopup);
            OpenContent(new ViewModels.StartViewModel());
        }

        public RelayCommand ClosePopupCommand { get; }

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

        private void OpenContent(INotifyPropertyChanged viewModel)
        {
            MainContent = viewModel;
            PopupContent = NullViewModel.Instance;
            IsPopupOpen = false;
        }

        private void OpenPopupContent(INotifyPropertyChanged viewModel)
        {
            PopupContent = viewModel;
            IsPopupOpen = true;
        }

        private void OnClosePopup()
        {
            PopupContent = NullViewModel.Instance;
            IsPopupOpen = false;
        }

    }
}
