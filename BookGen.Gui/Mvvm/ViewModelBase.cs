//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

namespace BookGen.Ui.Mvvm
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public IView? View { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void Notify(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void InjectView(IView view)
        {
            View = view;
        }
    }
}
