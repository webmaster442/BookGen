//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Ui.Mvvm;

namespace BookGen.ConsoleUi
{
    internal class InitializeViewModel: ViewModelBase
    {
        private readonly ILog _log;
        private readonly FsPath _configFile;
        private readonly FsPath _workDir;

        public bool CreateConfig { get; set; }
        public bool CreateMdFiles { get; set; }
        public bool CreateTemplates { get; set; }
        public bool CreateScripts { get; set; }
        public bool CreateVsTasks { get; set; }

        public DelegateCommand ExecuteCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public InitializeViewModel(ILog log, FsPath WorkDir)
        {
            _log = log;
            _workDir = WorkDir;
            _configFile = WorkDir.Combine("bookgen.json");
            ExecuteCommand = new DelegateCommand(OnExecute);
            CancelCommand = new DelegateCommand(() => View?.ExitApp());

            CreateConfig = true;
            CreateMdFiles = true;
            CreateTemplates = false;
            CreateScripts = false;
            CreateVsTasks = true;
        }

        private void OnExecute()
        {
            View?.UpdateBindingsToModel();
            View?.SuspendUi();
            if (CreateMdFiles)
            {
                _log.Info("Creating summary.md & index.md...");
                InitializerMethods.DoCreateMdFiles(_log, _workDir);
            }
            if (CreateTemplates)
            {
                _log.Info("Extracting templates...");
                InitializerMethods.ExtractTemplates(_log, _workDir);
            }
            if (CreateScripts)
            {
                _log.Info("Creating Script project...");
                InitializerMethods.CreateScriptProject(_log, _workDir,  Program.CurrentState.ProgramDirectory);
            }
            if (CreateConfig)
            {
                _log.Info("Creating and configuring config file...");
                InitializerMethods.DoCreateConfig(_log, _configFile, CreateMdFiles, CreateTemplates, CreateScripts);
            }
            if (CreateVsTasks)
            {
                _log.Info("Creating VS Code Tasks");
                InitializerMethods.DoCreateTasks(_log, _workDir);
            }
            View?.ExitApp();
        }
    }
}
