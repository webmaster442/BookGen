//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Domain;

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

                var loader = new ProjectLoader(CurrentState.Log, args.WorkDir);

                if (!loader.TryLoadAndValidateConfig(out Config? configuration)
                    || configuration == null)
                {
                    return ModuleRunResult.GeneralError;
                }

                switch (args.Action)
                {
                    case ChaptersAction.GenSummary:
                        return ChapterProcessingUtils.GenerateSummaryFile(args.WorkDir,
                                                                          configuration,
                                                                          CurrentState.Log)
                                                      .ToSuccesOrError();
                    case ChaptersAction.Scan:
                        ChapterProcessingUtils.ScanMarkdownFiles(args.WorkDir, configuration, CurrentState.Log);
                        break;
                }
            }

            return ModuleRunResult.Succes;
        }

        public override string GetHelp()
        {
            StringBuilder result = new StringBuilder(4096);
            result.Append(HelpUtils.GetHelpForModule(nameof(ChaptersModule)));
            HelpUtils.DocumentActions<ChaptersAction>(result);
            return result.ToString();
        }
    }
}
