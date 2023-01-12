//-----------------------------------------------------------------------------
// (c) 2020-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.ConsoleUi;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Infrastructure;
using BookGen.Interfaces;
using BookGen.ProjectHandling;

namespace BookGen.Modules
{
    internal sealed class GuiModule : ModuleWithState, IDisposable, IModuleCollection, IAsyncModule
    {
        private MainMenu? _mainMenu;
        private GeneratorRunner? _runner;
        private FolderLock? _folderLock;

        public const string MainView = "BookGen.ConsoleUi.MainView.xml";
        public const string HelpView = "BookGen.ConsoleUi.HelpView.xml";

        public GuiModule(ProgramState currentState) : base(currentState)
        {
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

        public async Task<ModuleRunResult> ExecuteAsync(string[] arguments)
        {
            var args = new BookGenArgumentBase();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            CurrentState.Gui = true;
            _runner = CurrentState.Api.CreateRunner(args.Verbose, args.Directory);

            _mainMenu = new MainMenu(_runner, CurrentState.Api);

            CheckLockFileExistsAndExitWhenNeeded(args.Directory);

            _folderLock = new FolderLock(args.Directory);

            if (_mainMenu != null)
            {
                await _mainMenu.Run();
                return ModuleRunResult.Succes;
            }

            return ModuleRunResult.GeneralError;
        }

        public override ModuleRunResult Execute(string[] arguments)
        {
            return ModuleRunResult.AsyncModuleCalledInSyncMode;
        }

        public override void Abort()
        {
            _mainMenu?.Cancel();
        }

        public void Dispose()
        {
            if (_mainMenu != null)
            {
                _mainMenu.Dispose();
                _mainMenu = null;
            }
            if (_folderLock != null)
            {
                _folderLock.Dispose();
                _folderLock = null;
            }

        }
    }
}