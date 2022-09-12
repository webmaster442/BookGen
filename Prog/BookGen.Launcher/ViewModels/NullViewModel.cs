//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
