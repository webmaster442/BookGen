//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Gui.Elements;
using System.Collections.Generic;

namespace BookGen.Gui
{
    internal class InteractiveInitializer : ConsoleMenuBase
    {
        private const string CreateConfig = "CreateConfig";
        private const string CreateMdFiles = "CreateMdFiles";
        private const string CreateTemplates = "CreateTemplates";

        private readonly ILog _log;
        private readonly FsPath _configFile;
        private readonly FsPath _workDir;

        public InteractiveInitializer(ILog log, FsPath WorkDir)
        {
            _log = log;
            _workDir = WorkDir;
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
                    Name = CreateConfig,
                    Content = "Create Default configuration file?"
                };
            }
        }

        protected override void ProcessInputs()
        {
            bool extractedTemlate =false;
            bool createdmdfiles = false;
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
            if (FindElement<BoolInput>(CreateConfig)?.Value == true)
            {
                InitializerMethods.DoCreateConfig(_log, _configFile, createdmdfiles, extractedTemlate);
            }
            ShouldRun = false;
        }
    }
}
