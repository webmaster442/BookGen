using System.ComponentModel;

namespace BookGen.Launcher.ViewModels
{
    internal sealed class NullViewModel : INotifyPropertyChanged
    {
        private NullViewModel() { }

        public static NullViewModel Instance => new();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
