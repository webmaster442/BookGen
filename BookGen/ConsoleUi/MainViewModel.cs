//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui;
using System.Diagnostics;

namespace BookGen.ConsoleUi
{
    internal class MainViewModel
    {
        public DelegateCommand ButtonAction { get; }

        public MainViewModel()
        {
            ButtonAction = new DelegateCommand(OnButton);
        }

        private void OnButton()
        {
            Debugger.Break();
        }
    }
}
