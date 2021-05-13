//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.Mvvm;
using BookGen.Utilities;
using System.Collections.Generic;

namespace BookGen.ConsoleUi
{
    public class HelpViewModel : ViewModelBase
    {
        private int _selectedIndex;

        public List<string> AvailableCommands { get; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                UpdateText(value);
                Notify(nameof(CommandText));
            }
        }

        public string CommandText { get; set; }

        public HelpViewModel(IEnumerable<string> commands)
        {
            CommandText = string.Empty;
            AvailableCommands = new List<string>(commands);
            SelectedIndex = 0;
        }

        private void UpdateText(int value)
        {
            var moduleName = AvailableCommands[value];
            CommandText = HelpUtils.GetHelpForModule(moduleName);
        }
    }
}
