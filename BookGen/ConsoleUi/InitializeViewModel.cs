//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Gui.Mvvm;

namespace BookGen.ConsoleUi
{
    internal class InitializeViewModel : ViewModelBase
    {
        private readonly ILog _log;
        private readonly FsPath _workDir;

        public bool CreateConfig { get; set; }
        public bool CreateMdFiles { get; set; }
        public bool CreateTemplates { get; set; }
        public bool CreateScripts { get; set; }
        public bool CreateVsTasks { get; set; }

        public int ConfigFormat { get; set; }

        public string WorkDirectory => _workDir.ToString();

        public DelegateCommand ExecuteCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public InitializeViewModel(ILog log, FsPath WorkDir)
        {
            _log = log;
            _workDir = WorkDir;
            ConfigFormat = 0;
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
                InitializerMethods.CreateScriptProject(_log, _workDir, Program.CurrentState.ProgramDirectory);
            }
            if (CreateConfig)
            {
                _log.Info("Creating and configuring config file...");
                bool configInYaml = ConfigFormat == 1;
                InitializerMethods.CreateConfig(_log, _workDir, configInYaml, CreateMdFiles, CreateTemplates, CreateScripts);
                //InitializerMethods.DoCreateConfig(_log, _configFile, CreateMdFiles, CreateTemplates, CreateScripts);
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
