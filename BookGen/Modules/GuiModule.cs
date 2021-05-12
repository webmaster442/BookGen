//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.ConsoleUi;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Ui.ArgumentParser;
using System;

namespace BookGen.Modules
{
    internal sealed class GuiModule : ModuleWithState, IDisposable
    {
        private Ui.ConsoleUi? uiRunner;

        public GuiModule(ProgramState currentState) : base(currentState)
        {
            uiRunner = new Ui.ConsoleUi();
        }

        public override string ModuleCommand => "Gui";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-d",
                                            "--dir",
                                            "-v",
                                            "--verbose");
            }
        }

        public override bool Execute(string[] arguments)
        {

            BookGenArgumentBase args = new BookGenArgumentBase();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            CurrentState.Gui = true;
            CurrentState.GeneratorRunner = Program.CreateRunner(args.Verbose, args.Directory);

            FolderLock.ExitIfFolderIsLocked(args.Directory, CurrentState.Log);

            using (var l = new FolderLock(args.Directory))
            {
                System.IO.Stream? Ui = typeof(GuiModule).Assembly.GetManifestResourceStream("BookGen.ConsoleUi.MainView.xml");
                var vm = new MainViewModel(CurrentState.GeneratorRunner);

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
