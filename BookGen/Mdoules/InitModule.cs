//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.ConsoleUi;
using BookGen.Core;
using BookGen.Utilities;
using System;

namespace BookGen.Mdoules
{
    internal class InitModule : ModuleBase
    {
        private readonly Gui.ConsoleUi uiRunner;

        public InitModule(ProgramState currentState) : base(currentState)
        {
            uiRunner = new Gui.ConsoleUi();
        }

        public override string ModuleCommand => "Init";

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            var dir = tokenizedArguments.GetSwitchWithValue("d", "dir");

            if (string.IsNullOrEmpty(dir))
                dir = Environment.CurrentDirectory;

            var log = new ConsoleLog(Api.LogLevel.Detail);

            System.IO.Stream? Ui = typeof(GuiModule).Assembly.GetManifestResourceStream("BookGen.ConsoleUi.InitializeView.xml");
            var vm = new InitializeViewModel(log, new FsPath(dir));

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
