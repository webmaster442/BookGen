//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;
using System.Text;

namespace BookGen.Modules
{
    internal class BuildModule : StateModuleBase
    {
        public BuildModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Build";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem("Build",
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

            CurrentState.GeneratorRunner = Program.CreateRunner(args.Verbose, args.WorkDir);
            switch (args.Action)
            {
                case ActionType.BuildWeb:
                    CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoBuild());
                    break;
                case ActionType.Clean:
                    CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoClean());
                    break;
                case ActionType.Test:
                    CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoTest());
                    break;
                case ActionType.BuildPrint:
                    CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoPrint());
                    break;
                case ActionType.BuildWordpress:
                    CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoWordpress());
                    break;
                case ActionType.BuildEpub:
                    CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoEpub());
                    break;
                case ActionType.ValidateConfig:
                    CurrentState.GeneratorRunner.Initialize();
                    break;
            }

            return true;
        }

        public override string GetHelp()
        {
            StringBuilder result = new StringBuilder(4096);
            result.Append(HelpUtils.GetHelpForModule(nameof(BuildModule)));
            HelpUtils.DocumentActions(result);
            return result.ToString();

        }
    }
}
