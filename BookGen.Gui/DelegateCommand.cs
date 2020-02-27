//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Gui
{
    public sealed class DelegateCommand
    {
        public Action Action { get; }

        public DelegateCommand(Action action)
        {
            Action = action;
        }
    }
}
