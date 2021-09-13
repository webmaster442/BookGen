//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui;
using BookGen.Ui.ArgumentParser;
using System;
using System.Collections.Generic;
using System.Linq;

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

                    uiRunner.OnNavigaton += UiRunner_OnNavigaton;
                    var (view, model) = UiRunner_OnNavigaton(MainView);
                    uiRunner.Run(view, model);
                    return true;
                }
            }
            return false;
        }

        private System.IO.Stream GetView(string name)
        {
            System.IO.Stream? result = typeof(GuiModule).Assembly.GetManifestResourceStream(name);
            if (result != null)
            {
                return result;
            }
            throw new InvalidOperationException($"Can't find view: {name}");
        }

        private (System.IO.Stream view, Ui.Mvvm.ViewModelBase model) UiRunner_OnNavigaton(string arg)
        {
            if (arg == MainView 
                && CurrentState.GeneratorRunner != null)
            {
                var vm = new MainViewModel(CurrentState.GeneratorRunner);
                return (GetView(MainView), vm);
            }
            else if (arg == HelpView)
            {
                var helpvm = new HelpViewModel(Modules ?? Enumerable.Empty<ModuleBase>());
                return (GetView(HelpView), helpvm);
            }
            throw new InvalidOperationException($"Can't find view: {arg}");
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