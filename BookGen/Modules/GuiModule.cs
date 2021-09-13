//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Framework.InternalGui;
using BookGen.Gui;
using BookGen.Ui.ArgumentParser;
using System;
using System.Collections.Generic;

namespace BookGen.Modules
{
    internal sealed class GuiModule : ModuleWithState, IDisposable, IModuleCollection
    {
        private ConsoleUi? uiRunner;

        public GuiModule(ProgramState currentState) : base(currentState)
        {
            uiRunner = new ConsoleUi();
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

        public IEnumerable<ModuleBase>? Modules { get; set; }

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
                if (uiRunner != null)
                {
                    var controller = new MainViewController(CurrentState.GeneratorRunner);
                    uiRunner.RunMainView(controller);
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