//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Framework.InternalGui
{
    internal sealed class MainViewController : IMainViewController
    {
        private readonly GeneratorRunner _runner;
        private readonly List<ModuleBase> _modules;
        private int _selectedHelpIndex;

        public MainViewController(GeneratorRunner runner, 
                                  IEnumerable<ModuleBase> modules)
        {
            _runner = runner;
            _modules = modules.ToList();
            _runner.IsGuiMode = true;
            CurrentHelpText = string.Empty;
            HelpItemSource = modules.Select(module => module.ModuleCommand).ToArray();
        }

        public string WorkDir => _runner.WorkDirectory;

        public IConsoleUi? Ui { get; set; }
        public string CurrentHelpText { get; set; }

        public string[] HelpItemSource { get; }

        public int SelectedHelpIndex
        {
            get { return _selectedHelpIndex; }
            set 
            { 
                _selectedHelpIndex = value;
                UpdateText(value);
            }
        }


        private void UpdateText(int value)
        {
            string moduleName = HelpItemSource[value];
            CurrentHelpText = ReformatText(moduleName);
            Ui?.RefreshCurrentView();
        }

        private string ReformatText(string moduleName)
        {
            var text = _modules
                .Find(module => module.ModuleCommand == moduleName)
                ?.GetHelp() ?? string.Empty;

            return text.Replace("  ", "");
        }


        public void BuildEpub() => _runner.InitializeAndExecute(x => x.DoEpub());

        public void BuildPrint() => _runner.InitializeAndExecute(x => x.DoPrint());

        public void BuildRelease() => _runner.InitializeAndExecute(x => x.DoBuild());

        public void BuildTest() => _runner.InitializeAndExecute(x => x.DoTest());

        public void BuildWordpress() => _runner.InitializeAndExecute(x => x.DoWordpress());

        public void CleanOutDir() => _runner.InitializeAndExecute(x => x.DoClean());

        public void ValidateConfig() => _runner.Initialize();

        public void SelectedHelpItemChange(int index)
        {
            SelectedHelpIndex = index;
        }
    }
}