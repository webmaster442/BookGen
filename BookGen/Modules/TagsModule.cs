//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Utilities;
using System.Diagnostics;

namespace BookGen.Modules
{
    internal class TagsModule : ModuleWithState
    {
        public TagsModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Tags";

        public override ModuleRunResult Execute(string[] arguments)
        {
            BookGenArgumentBase args = new();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            CurrentState.Log.LogLevel = args.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

            CheckLockFileExistsAndExitWhenNeeded(args.Directory);

            using (var l = new FolderLock(args.Directory))
            {
                ProjectLoader loader = new ProjectLoader(CurrentState.Log, args.Directory);

                return loader.TryLoadProjectAndExecuteOperation((config, toc) =>
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();



                    CurrentState.Log.Info("Total runtime: {0}ms", stopwatch.ElapsedMilliseconds);

                    return true;

                }).ToSuccesOrError();
            }
        }
    }
}
