//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Ui.Mvvm
{
    public abstract class ViewModelBase
    {
        public IView? View { get; private set; }

        public void InjectView(IView view)
        {
            View = view;
        }
    }
}
