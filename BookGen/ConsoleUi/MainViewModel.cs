//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Mvvm;
using System;

namespace BookGen.ConsoleUi
{
    internal class MainViewModel: ViewModelBase
    {
        private readonly GeneratorRunner _runner;

        public DelegateCommand InitializeCommand { get; }
        public DelegateCommand ValidateConfigCommand { get; }
        public DelegateCommand CleanOutDirCommand { get; }
        public DelegateCommand BuildTestCommand { get; }
        public DelegateCommand BuildReleaseCommand { get; }
        public DelegateCommand BuildPrintCommand { get; }
        public DelegateCommand BuildEpubCommand { get; }
        public DelegateCommand BuildWordpressCommand { get; }
        public DelegateCommand LaunchEditorCommand { get; }
        public DelegateCommand HelpCommand { get; }
        public DelegateCommand ExitCommand { get; }

        public string WorkDirectory
        {
            get;
        }

        public MainViewModel(GeneratorRunner runner)
        {
            _runner = runner;
            WorkDirectory = _runner.WorkDirectory;
            InitializeCommand = new DelegateCommand(this, () => _runner.DoInteractiveInitialize());
            ValidateConfigCommand = new DelegateCommand(this, () => _runner.Initialize());
            CleanOutDirCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoClean()));
            BuildTestCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoTest()));
            BuildReleaseCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoBuild()));
            BuildPrintCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoPrint()));
            BuildEpubCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoEpub()));
            BuildWordpressCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoWordpress()));
            LaunchEditorCommand = new DelegateCommand(this, () => _runner.InitializeAndExecute(x => x.DoEditor()));
            HelpCommand = new DelegateCommand(this, () => _runner.RunHelp());
            ExitCommand = new DelegateCommand(() => View?.ExitApp());
        }
    }
}
