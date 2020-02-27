//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------
/*
using BookGen.Api;
using BookGen.Core;
using BookGen.Gui.Elements;
using System.Collections.Generic;

namespace BookGen.Gui
{
    internal class InteractiveInitializer : ConsoleMenuBase
    {
        private const string CreateConfig = nameof(CreateConfig);
        private const string CreateMdFiles = nameof(CreateMdFiles);
        private const string CreateTemplates = nameof(CreateTemplates);
        private const string CreateScripts = nameof(CreateScripts);
        private const string CreateVsTasks = nameof(CreateVsTasks);

        private readonly ILog _log;
        private readonly FsPath _configFile;
        private readonly FsPath _workDir;
        private readonly ProgramState _state;

        public InteractiveInitializer(ILog log, FsPath WorkDir, ProgramState state)
        {
            _log = log;
            _workDir = WorkDir;
            _state = state;
            _configFile = WorkDir.Combine("bookgen.json");
        }

        public override IEnumerable<ConsoleUiElement> CreateElements()
        {
            yield return new TextBlock
            {
                Content = $"Working directory: {_workDir}\r\n\r\n"
            };
            yield return new BoolInput
            {
                Name = CreateMdFiles,
                Content = "    Create summary.md/overwrite and index.md files?"
            };
            yield return new BoolInput
            {
                Name = CreateTemplates,
                Content = "    Extract/overwrite templates for customization?"
            };
            yield return new BoolInput
            {
                Name = CreateScripts,
                Content = "    Create/overwrite script file and script project?"
            };
            yield return new BoolInput
            {
                Name = CreateConfig,
                Content = "    Create/overwrite configuration file?"
            };
            yield return new BoolInput
            {
                Name = CreateVsTasks,
                Content = "    Create/overwrite VS Tasks json file?"
            };
        }

        protected override void ProcessInputs()
        {
            bool extractedTemlate = false;
            bool createdmdfiles = false;
            bool extractedScript = false;
            if (FindElement<BoolInput>(CreateMdFiles)?.Value == true)
            {
                _log.Info("Creating summary.md & index.md...");
                InitializerMethods.DoCreateMdFiles(_log, _workDir);
                createdmdfiles = true;
            }
            if (FindElement<BoolInput>(CreateTemplates)?.Value == true)
            {
                _log.Info("Extracting templates...");
                InitializerMethods.ExtractTemplates(_log, _workDir);
                extractedTemlate = true;
            }
            if (FindElement<BoolInput>(CreateScripts)?.Value == true)
            {
                _log.Info("Creating Script project...");
                InitializerMethods.CreateScriptProject(_log, _workDir, _state.ProgramDirectory);
                extractedScript = true;
            }
            if (FindElement<BoolInput>(CreateConfig)?.Value == true)
            {
                _log.Info("Creating and configuring config file...");
                InitializerMethods.DoCreateConfig(_log, _configFile, createdmdfiles, extractedTemlate, extractedScript);
            }
            if (FindElement<BoolInput>(CreateVsTasks)?.Value == true)
            {
                _log.Info("Creating VS Code Tasks");
                InitializerMethods.DoCreateTasks(_log, _workDir);
            }
            ShouldRun = false;
        }
    }
}
*/