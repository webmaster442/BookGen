//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Ui.Mvvm
{
    public interface IView
    {
        void SuspendUi();
        void ResumeUi();
        void ExitApp();
        void UpdateBindingsToModel();
    }
}
