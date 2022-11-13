//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.ProjectHandling;

namespace BookGen.Modules
{
    internal class ChaptersModule : ModuleWithState
    {
        public ChaptersModule(ProgramState currentState) : base(currentState)
        {
        }

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-a",
                                            "--action",
                                            "-d",
                                            "--dir",
                                            "Scan",
                                            "GenSummary");
            }
        }

        public override string ModuleCommand => "Chapters";

        public override ModuleRunResult Execute(string[] arguments)
        {
            var args = new ChaptersArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            CheckLockFileExistsAndExitWhenNeeded(args.WorkDir);

            using (var l = new FolderLock(args.WorkDir))
            {

                var loader = new ProjectLoader(args.WorkDir, CurrentState.Log);

                if (!loader.LoadProject())
                {
                    return ModuleRunResult.GeneralError;
                }

                switch (args.Action)
                {
                    case ChaptersAction.GenSummary:
                        return ChapterProcessingUtils.GenerateSummaryFile(args.WorkDir,
                                                                          loader.Configuration,
                                                                          CurrentState.Log)
                                                      ? ModuleRunResult.Succes
                                                      : ModuleRunResult.GeneralError;
                    case ChaptersAction.Scan:
                        ChapterProcessingUtils.ScanMarkdownFiles(args.WorkDir, loader.Configuration, CurrentState.Log);
                        break;
                }
            }

            return ModuleRunResult.Succes;
        }

        public override string GetHelp()
        {
            var result = new StringBuilder(4096);
            result.Append(HelpUtils.GetHelpForModule(nameof(ChaptersModule)));
            HelpUtils.DocumentActions<ChaptersAction>(result);
            return result.ToString();
        }
    }
}
