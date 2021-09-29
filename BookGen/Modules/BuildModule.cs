//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Utilities;
using System.Text;

namespace BookGen.Modules
{
    internal class BuildModule : ModuleWithState
    {
        public BuildModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Build";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-n",
                                            "--nowait",
                                            "-v",
                                            "--verbose",
                                            "-d",
                                            "--dir",
                                            "-a",
                                            "--action",
                                            "Test",
                                            "BuildPrint",
                                            "BuildWeb",
                                            "BuildEpub",
                                            "BuildWordpress",
                                            "Clean",
                                            "ValidateConfig");
            }
        }

        public override ModuleRunResult Execute(string[] arguments)
        {
            BuildArguments args = new BuildArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            FolderLock.ExitIfFolderIsLocked(args.Directory, CurrentState.Log);

            using (var l = new FolderLock(args.Directory))
            {

                var runner = CurrentState.Api.CreateRunner(args.Verbose, args.Directory);
                runner.NoWait = args.NoWaitForExit;

                switch (args.Action)
                {
                    case BuildAction.BuildWeb:
                        runner.InitializeAndExecute(x => x.DoBuild());
                        break;
                    case BuildAction.Clean:
                        runner.InitializeAndExecute(x => x.DoClean());
                        break;
                    case BuildAction.Test:
                        runner.InitializeAndExecute(x => x.DoTest());
                        break;
                    case BuildAction.BuildPrint:
                        runner.InitializeAndExecute(x => x.DoPrint());
                        break;
                    case BuildAction.BuildWordpress:
                        runner.InitializeAndExecute(x => x.DoWordpress());
                        break;
                    case BuildAction.BuildEpub:
                        runner.InitializeAndExecute(x => x.DoEpub());
                        break;
                    case BuildAction.ValidateConfig:
                        runner.Initialize();
                        break;
                }
            }

            return ModuleRunResult.Succes;
        }

        public override string GetHelp()
        {
            StringBuilder result = new StringBuilder(4096);
            result.Append(HelpUtils.GetHelpForModule(nameof(BuildModule)));
            HelpUtils.DocumentActions<BuildAction>(result);
            return result.ToString();

        }
    }
}
