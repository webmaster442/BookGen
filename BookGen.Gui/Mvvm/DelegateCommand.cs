//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Gui.Mvvm
{
    public sealed class DelegateCommand
    {
        public Action Action { get; }

        public DelegateCommand(Action action)
        {
            Action = action;
        }

        public DelegateCommand(ViewModelBase model, Action action, bool suspendsUi = true)
        {
            if (suspendsUi)
            {
                Action = () =>
                {
                    model?.View?.SuspendUi();
                    action?.Invoke();
                    model?.View?.ResumeUi();
                };
            }
            else
            {
                Action = action;
            }
        }
    }
}
