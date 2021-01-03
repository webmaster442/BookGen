//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.ConsoleUi;
using BookGen.Core;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;

namespace BookGen.Modules
{
    internal class InitModule : StateModuleBase
    {
        private readonly Ui.ConsoleUi uiRunner;

        public InitModule(ProgramState currentState) : base(currentState)
        {
            uiRunner = new Ui.ConsoleUi();
        }

        public override string ModuleCommand => "Init";

        public override AutoCompleteItem AutoCompleteInfo => new AutoCompleteItem("Init", "-d", "--dir", "-v", "--verbose");

        public override bool Execute(string[] arguments)
        {
            BookGenArgumentBase args = new BookGenArgumentBase();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            Api.LogLevel logLevel = args.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

            CurrentState.Log.LogLevel = logLevel;

            System.IO.Stream? Ui = typeof(GuiModule).Assembly.GetManifestResourceStream("BookGen.ConsoleUi.InitializeView.xml");
            var vm = new InitializeViewModel(CurrentState.Log, new FsPath(args.Directory));

            if (Ui != null)
            {
                uiRunner.Run(Ui, vm);
                return true;
            }
            return false;
        }

        public override void Abort()
        {
            uiRunner?.SuspendUi();
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(InitModule));
        }
    }
}
