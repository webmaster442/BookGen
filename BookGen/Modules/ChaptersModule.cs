//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Ui.ArgumentParser;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGen.Modules
{
    internal class ChaptersModule : StateModuleBase
    {
        public ChaptersModule(ProgramState currentState) : base(currentState)
        {
        }

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem("Chapters",
                                            "-a",
                                            "--action",
                                            "Scan",
                                            "Create",
                                            "GenSummary");
            }
        }

        public override string ModuleCommand => "Chapters";

        public override bool Execute(string[] arguments)
        {
            ChaptersParameters args = new ChaptersParameters();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            var log = new ConsoleLog(LogLevel.Info);

            var loader = new ProjectLoader(log, args.WorkDir);

            if (!loader.TryLoadAndValidateConfig(out Config? configuration))
            {
                return false;
            }

            switch (args.Action)
            {
                case ChaptersAction.GenSummary:
                    GenerateSummaryFile(configuration);
                    break;
                case ChaptersAction.Scan:
                    ScanMarkdownFiles(args.WorkDir);
                    break;
            }

            return true;
        }

        private void ScanMarkdownFiles(string workDir)
        {
            FsPath destination = new FsPath(workDir, ".chapters");

        }

        private void GenerateSummaryFile(Config? configuration)
        {
            throw new NotImplementedException();
        }

        public override string GetHelp()
        {
            throw new NotImplementedException();
        }
    }
}
