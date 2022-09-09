using BookGen.Launcher.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace BookGen.Launcher
{
    internal class MainWindowViewModel : ObservableObject
    {
        private readonly IMainWindow _mainWindow;
        private bool _isPopupOpen;

        public MainWindowViewModel(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            ClosePopupCommand = new RelayCommand(OnClosePopup);
            OpenContent(new ViewModels.StartViewModel());
        }

        public RelayCommand ClosePopupCommand { get; }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set => SetProperty(ref _isPopupOpen, value);
        }

        private void OpenContent(INotifyPropertyChanged viewModel)
        {
            _mainWindow.LoadIntoMain(viewModel);
            IsPopupOpen = false;
        }

        private void OpenPopupContent(INotifyPropertyChanged viewModel)
        {
            _mainWindow.LoadIntoPopup(viewModel);
            IsPopupOpen = true;
        }

        private void OnClosePopup()
        {
            _mainWindow.ClosePopup();
            IsPopupOpen = false;
        }

    }
}
