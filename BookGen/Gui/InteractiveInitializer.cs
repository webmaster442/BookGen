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
        const string CreateConfig = "CreateConfig";

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
                    Name = CreateConfig,
                    Content = "Create Default configuration file?"
                };
            }
        }

        protected override void ProcessInputs()
        {
            if (FindElement<BoolInput>(CreateConfig)?.Value == true)
            {
                InitializerMethods.DoCreateConfig(_log, _configFile);
            }
            ShouldRun = false;
        }
    }
}
