//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Ui.ArgumentParser;
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

        public override bool Execute(string[] arguments)
        {
            BuildParameters args = new BuildParameters();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            FolderLock.ExitIfFolderIsLocked(args.Directory, CurrentState.Log);

            using (var l = new FolderLock(args.Directory))
            {

                CurrentState.GeneratorRunner = Program.CreateRunner(args.Verbose, args.Directory);
                CurrentState.GeneratorRunner.NoWait = args.NoWaitForExit;

                switch (args.Action)
                {
                    case BuildAction.BuildWeb:
                        CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoBuild());
                        break;
                    case BuildAction.Clean:
                        CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoClean());
                        break;
                    case BuildAction.Test:
                        CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoTest());
                        break;
                    case BuildAction.BuildPrint:
                        CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoPrint());
                        break;
                    case BuildAction.BuildWordpress:
                        CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoWordpress());
                        break;
                    case BuildAction.BuildEpub:
                        CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoEpub());
                        break;
                    case BuildAction.ValidateConfig:
                        CurrentState.GeneratorRunner.Initialize();
                        break;
                }
            }

            return true;
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
