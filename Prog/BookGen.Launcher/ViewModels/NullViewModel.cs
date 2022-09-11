namespace BookGen.Launcher.ViewModels
{
    internal sealed class NullViewModel : INotifyPropertyChanged
    {
        private NullViewModel() 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }

        public static NullViewModel Instance => new();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
