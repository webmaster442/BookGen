//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookGen.Gui.Wpf
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void Set<T>(ref T field, T value, [CallerMemberName]string? propName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(propName);
            }
        }

        protected void RaisePropertyChanged(string? name = null)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
