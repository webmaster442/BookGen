//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui;

namespace BookGen.Framework.InternalGui
{
    internal sealed class MainViewController : IMainViewController
    {
        private readonly GeneratorRunner _runner;

        public MainViewController(GeneratorRunner runner)
        {
            _runner = runner;
        }

        public string WorkDir => _runner.WorkDirectory;

        public IConsoleUi? Ui { get; set; }

        public void BuildEpub() => _runner.InitializeAndExecute(x => x.DoEpub());

        public void BuildPrint() => _runner.InitializeAndExecute(x => x.DoPrint());

        public void BuildRelease() => _runner.InitializeAndExecute(x => x.DoBuild());

        public void BuildTest() => _runner.InitializeAndExecute(x => x.DoTest());

        public void BuildWordpress() => _runner.InitializeAndExecute(x => x.DoWordpress());

        public void CleanOutDir() => _runner.InitializeAndExecute(x => x.DoClean());

        public void ValidateConfig() => _runner.Initialize();
    }
}