//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.ConsoleUi;
using BookGen.Core;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using System;

namespace BookGen.Modules
{
    internal sealed class InitModule : ModuleWithState, IDisposable
    {
        private Gui.ConsoleUi? uiRunner;

        public InitModule(ProgramState currentState) : base(currentState)
        {
            uiRunner = new Gui.ConsoleUi();
        }

        public override string ModuleCommand => "Init";

        public override AutoCompleteItem AutoCompleteInfo => new AutoCompleteItem(ModuleCommand, "-d", "--dir", "-v", "--verbose");

        public override bool Execute(string[] arguments)
        {
            BookGenArgumentBase args = new BookGenArgumentBase();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            CurrentState.Log.LogLevel = args.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

            FolderLock.ExitIfFolderIsLocked(args.Directory, CurrentState.Log);

            using (var l = new FolderLock(args.Directory))
            {

                System.IO.Stream? Ui = typeof(GuiModule).Assembly.GetManifestResourceStream("BookGen.ConsoleUi.InitializeView.xml");
                var vm = new InitializeViewModel(CurrentState.Log, new FsPath(args.Directory));

                if (Ui != null)
                {
                    uiRunner?.Run(Ui, vm);
                    return true;
                }
            }
            return false;
        }

        public override void Abort()
        {
            uiRunner?.SuspendUi();
        }

        public void Dispose()
        {
            if (uiRunner != null)
            {
                uiRunner.Dispose();
                uiRunner = null;
            }
        }
    }
}
