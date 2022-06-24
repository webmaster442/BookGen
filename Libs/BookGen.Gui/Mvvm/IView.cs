﻿//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui.Mvvm
{
    public interface IView
    {
        void SuspendUi();
        void ResumeUi();
        void ExitApp();
        void UpdateBindingsToModel();
        void SwitchToView(string name);
        void UpdateViewFromModel();
    }
}