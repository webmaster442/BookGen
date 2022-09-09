using System.ComponentModel;

namespace BookGen.Launcher.Interfaces
{
    internal interface IMainWindow
    {
        void LoadIntoMain(INotifyPropertyChanged viewModel);
        void LoadIntoPopup(INotifyPropertyChanged viewModel);
        void ClosePopup();
    }
}
