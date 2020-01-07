//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
                Content = $"Working directory: {_workDir}\r\n"
            };
            if (_configFile.IsExisting)
            {
                yield return new TextBlock
                {
                    Content = "BookGen.json Configuration exists in folder. Can't continue\r\n"
                };
            }
            else
            {
                yield return new BoolInput
                {
                    Name = CreateMdFiles,
                    Content = "Create summary.md and index.md files?"
                };
                yield return new BoolInput
                {
                    Name = CreateTemplates,
                    Content = "Extract templates for customization?"
                };
                yield return new BoolInput
                {
                    Name = CreateScripts,
                    Content = "Create script file and script project?"
                };
                yield return new BoolInput
                {
                    Name = CreateConfig,
                    Content = "Create Default configuration file?"
                };
            }
        }

        protected override void ProcessInputs()
        {
            bool extractedTemlate =false;
            bool createdmdfiles = false;
            bool extractedScript = false;
            if (FindElement<BoolInput>(CreateMdFiles)?.Value == true)
            {
                InitializerMethods.DoCreateMdFiles(_log, _workDir);
                createdmdfiles = true;
            }
            if (FindElement<BoolInput>(CreateTemplates)?.Value == true)
            {
                InitializerMethods.ExtractTemplates(_log, _workDir);
                extractedTemlate = true;
            }
            if (FindElement<BoolInput>(CreateScripts)?.Value == true)
            {
                InitializerMethods.CreateScriptProject(_log, _workDir, _state.ProgramDirectory);
                extractedScript = true;
            }
            if (FindElement<BoolInput>(CreateConfig)?.Value == true)
            {
                InitializerMethods.DoCreateConfig(_log, _configFile, createdmdfiles, extractedTemlate, extractedScript);
            }
            ShouldRun = false;
        }
    }
}
