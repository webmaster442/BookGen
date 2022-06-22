//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Gui.Mvvm
{
    public sealed class DelegateCommand
    {
        public Action Action { get; }
        public bool SuspendsUI { get; }

        public DelegateCommand(Action action, bool suspendsUi = true)
        {
            Action = action;
            SuspendsUI = suspendsUi;
        }
    }
}
