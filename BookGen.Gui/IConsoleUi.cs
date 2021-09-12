//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui
{
    public interface IConsoleUi
    {
        void SuspendUi();
        void ResumeUi();
        void ExitApp();
    }
}
