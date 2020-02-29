//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui.Mvvm
{
    public abstract class ViewModelBase
    {
        protected IView? View { get; private set; }

        public void InjectView(IView view)
        {
            View = view;
        }
    }
}
