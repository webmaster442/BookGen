//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.Mvvm;
using BookGen.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.ConsoleUi
{
    internal class HelpViewModel : ViewModelBase
    {
        private int _selectedIndex;
        private List<Framework.ModuleBase> _modules;

        public List<string> AvailableCommands { get; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                UpdateText(value);
            }
        }

        public string CommandText { get; set; }

        public HelpViewModel(IEnumerable<Framework.ModuleBase> modules)
        {

            CommandText = string.Empty;
            _modules = new List<Framework.ModuleBase>(modules);

            AvailableCommands = modules
                                    .Select(module => module.ModuleCommand)
                                    .ToList();

            SelectedIndex = 0;

        }

        private void UpdateText(int value)
        {
            string moduleName = AvailableCommands[value];
            CommandText = _modules
                .Find(module => module.ModuleCommand == moduleName)
                ?.GetHelp() ?? string.Empty;
            View?.UpdateViewFromModel();
        }
    }
}
