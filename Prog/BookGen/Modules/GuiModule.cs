//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.ConsoleUi;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Infrastructure;

namespace BookGen.Modules
{
    internal sealed class GuiModule : ModuleWithState, IDisposable, IModuleCollection
    {
        private Gui.ConsoleUi? uiRunner;
        private GeneratorRunner? _runner;
        private FolderLock? _folderLock;

        public const string MainView = "BookGen.ConsoleUi.MainView.xml";
        public const string HelpView = "BookGen.ConsoleUi.HelpView.xml";

        public GuiModule(ProgramState currentState) : base(currentState)
        {
            uiRunner = new Gui.ConsoleUi();
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

        public override ModuleRunResult Execute(string[] arguments)
        {

            BookGenArgumentBase args = new BookGenArgumentBase();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            CurrentState.Gui = true;
            _runner = CurrentState.Api.CreateRunner(args.Verbose, args.Directory);

            CheckLockFileExistsAndExitWhenNeeded(args.Directory);

            _folderLock = new FolderLock(args.Directory);

            if (uiRunner != null)
            {

                uiRunner.OnNavigaton += UiRunner_OnNavigaton;
                var (view, model) = UiRunner_OnNavigaton(MainView);
                
                uiRunner.Run(view, model);
                return ModuleRunResult.Succes;
            }

            return ModuleRunResult.GeneralError;
        }

        private static System.IO.Stream GetView(string name)
        {
            System.IO.Stream? result = typeof(GuiModule).Assembly.GetManifestResourceStream(name);
            if (result != null)
            {
                return result;
            }
            throw new InvalidOperationException($"Can't find view: {name}");
        }

        private (System.IO.Stream view, Gui.Mvvm.ViewModelBase model) UiRunner_OnNavigaton(string arg)
        {
            if (arg == MainView
                && _runner != null)
            {
                var vm = new MainViewModel(_runner, CurrentState.Api);
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
            if (_folderLock != null)
            {
                _folderLock.Dispose();
                _folderLock = null;
            }

        }
    }
}