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

namespace BookGen.Modules
{
    internal sealed class InitModule : ModuleWithState, IDisposable, IAsyncModule
    {
        private InitMenu? _initMenu;


        public InitModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Init";

        public override AutoCompleteItem AutoCompleteInfo => new AutoCompleteItem(ModuleCommand,
                                                                                  "-d",
                                                                                  "--dir",
                                                                                  "-v",
                                                                                  "--verbose");

        public async Task<ModuleRunResult> ExecuteAsync(string[] arguments)
        {
            var args = new BookGenArgumentBase();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            CurrentState.Log.LogLevel = args.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

            CheckLockFileExistsAndExitWhenNeeded(args.Directory);

            using (var l = new FolderLock(args.Directory))
            {

                _initMenu = new InitMenu(CurrentState.Log, new FsPath(args.Directory));
                await _initMenu.Run();
                return ModuleRunResult.Succes;

            }
        }

        public override ModuleRunResult Execute(string[] arguments)
        {
            return ModuleRunResult.AsyncModuleCalledInSyncMode;
        }

        public override void Abort()
        {
            _initMenu?.Cancel();
        }

        public void Dispose()
        {
            if (_initMenu != null)
            {
                _initMenu.Dispose();
                _initMenu = null;
            }
        }
    }
}
