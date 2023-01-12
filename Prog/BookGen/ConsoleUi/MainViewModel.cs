//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

/*
using BookGen.Gui.Mvvm;
using BookGen.Infrastructure;
using BookGen.Modules;
using System.Diagnostics;

namespace BookGen.ConsoleUi
{
    internal sealed class MainViewModel : ViewModelBase
    {
        private readonly GeneratorRunner _runner;
        private readonly IModuleApi _api;

        public DelegateCommand ValidateConfigCommand { get; }
        public DelegateCommand CleanOutDirCommand { get; }
        public DelegateCommand BuildTestCommand { get; }
        public DelegateCommand BuildReleaseCommand { get; }
        public DelegateCommand BuildPrintCommand { get; }
        public DelegateCommand BuildEpubCommand { get; }
        public DelegateCommand BuildWordpressCommand { get; }
        public DelegateCommand BuildPostprocessCommand { get; }

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

        public MainViewModel(GeneratorRunner runner, IModuleApi api)
        {
            _runner = runner;
            _api = api;
            WorkDirectory = _runner.WorkDirectory;
            ValidateConfigCommand = new DelegateCommand(() => _runner.Initialize());
            CleanOutDirCommand = new DelegateCommand(() => _runner.InitializeAndExecute(x => x.DoClean()));
            BuildTestCommand = new DelegateCommand(() => _runner.InitializeAndExecute(x => x.DoTest()));
            BuildReleaseCommand = new DelegateCommand(() => _runner.InitializeAndExecute(x => x.DoBuild()));
            BuildPrintCommand = new DelegateCommand(() => _runner.InitializeAndExecute(x => x.DoPrint()));
            BuildEpubCommand = new DelegateCommand(() => _runner.InitializeAndExecute(x => x.DoEpub()));
            BuildWordpressCommand = new DelegateCommand(() => _runner.InitializeAndExecute(x => x.DoWordpress()));
            BuildPostprocessCommand = new DelegateCommand(() => _runner.InitializeAndExecute(x => x.DoPostProcess()));
            HelpCommand = new DelegateCommand(() => View?.SwitchToView(GuiModule.HelpView));
            ExitCommand = new DelegateCommand(() => View?.ExitApp(), false);
            ServeCommand = new DelegateCommand(() => StartModuleInWorkdir("serve"));
            StatCommand = new DelegateCommand(() => StartModuleInWorkdir("stat"));
            PreviewCommand = new DelegateCommand(() => StartModuleInWorkdir("preview"));
            UpdateCommand = new DelegateCommand(() =>
            {
                using (var p = new Process())
                {
                    p.StartInfo.WorkingDirectory = AppContext.BaseDirectory;
                    p.StartInfo.FileName = "BookGen.Update.exe";
                    p.Start();
                }
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
*/