//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Modules;
using BookGen.Ui.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.ConsoleUi
{
    internal class HelpViewModel : ViewModelBase
    {
        private int _selectedIndex;
        private readonly List<Framework.ModuleBase> _modules;
        private const string back = "<-- Back to previous menu";

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
                                    .OrderBy(x => x)
                                    .ToList();

            AvailableCommands.Add(back);

            SelectedIndex = 0;

        }

        private void UpdateText(int value)
        {
            string moduleName = AvailableCommands[value];

            if (moduleName == back)
            {
                View?.SwitchToView(GuiModule.MainView);
            }

            CommandText = ReformatText(moduleName);
            View?.UpdateViewFromModel();
        }

        private string ReformatText(string moduleName)
        {
            var text = _modules
                .Find(module => module.ModuleCommand == moduleName)
                ?.GetHelp() ?? string.Empty;

            return text.Replace("  ", "");
        }
    }
}
