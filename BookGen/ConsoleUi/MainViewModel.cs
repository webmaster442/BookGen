//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Gui.Mvvm;
using BookGen.Modules;
using System;

namespace BookGen.ConsoleUi
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly GeneratorRunner _runner;
        private readonly IMoudleApi _api;

        public DelegateCommand ValidateConfigCommand { get; }
        public DelegateCommand CleanOutDirCommand { get; }
        public DelegateCommand BuildTestCommand { get; }
        public DelegateCommand BuildReleaseCommand { get; }
        public DelegateCommand BuildPrintCommand { get; }
        public DelegateCommand BuildEpubCommand { get; }
        public DelegateCommand BuildWordpressCommand { get; }
        public DelegateCommand HelpCommand { get; }
        public DelegateCommand ExitCommand { get; }

        public DelegateCommand ServeCommand { get; }
        public DelegateCommand StatCommand { get; }
        public DelegateCommand PreviewCommand { get; }
        public DelegateCommand UpdateCommand { get; }

        public string WorkDirectory
        {
            get;
        }

        public MainViewModel(GeneratorRunner runner, IMoudleApi api)
        {
            _runner = runner;
            _api = api;
            WorkDirectory = _runner.WorkDirectory;
            ValidateConfigCommand = new DelegateCommand(this, () => _runner.Initialize());
            CleanOutDirCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoClean()));
            BuildTestCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoTest()));
            BuildReleaseCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoBuild()));
            BuildPrintCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoPrint()));
            BuildEpubCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoEpub()));
            BuildWordpressCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoWordpress()));
            HelpCommand = new DelegateCommand(this, () => View?.SwitchToView(GuiModule.HelpView));
            ExitCommand = new DelegateCommand(() => View?.ExitApp());
            ServeCommand = new DelegateCommand(this, () => StartModuleInWorkdir("serve"));
            StatCommand = new DelegateCommand(this, () => StartModuleInWorkdir("stat"));
            PreviewCommand = new DelegateCommand(this, () => StartModuleInWorkdir("preview"));
            UpdateCommand = new DelegateCommand(this, () =>
            {
                _api.ExecuteModule("update", Array.Empty<string>());
            });
        }

        private void StartModuleInWorkdir(string name)
        {
            _api.ExecuteModule(name, new string[]
            {
                "-d",
                _runner.WorkDirectory
            });
        }
    }
}
