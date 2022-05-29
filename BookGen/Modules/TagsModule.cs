//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
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

                    FsPath tags = new FsPath(args.Directory, "tags.json");
                    
                    if (!loader.TryGetTags(out TagUtils tagUtils))
                    {
                        return false;
                    }

                    tagUtils.DeleteNoLongerExisting(toc);
                    tagUtils.CreateNotYetExisting(toc);

                    PrintStats(CurrentState.Log, tagUtils);


                    SerializeTagCollection(args.Directory, CurrentState.Log, tagUtils.TagCollection);

                    CurrentState.Log.Info("Total runtime: {0}ms", stopwatch.ElapsedMilliseconds);
                    return true;

                }).ToSuccesOrError();
            }
        }

        private static void PrintStats(ILog log, TagUtils tagUtils)
        {
            log.Info("Total tags: {0}", tagUtils.TotalTagCount);
            log.Info("Total unique tags: {0}", tagUtils.UniqueTagCount);
            log.Info("Files without tags: {0}", tagUtils.FilesWithOutTags);
        }

        private static void SerializeTagCollection(string directory, ILog log, Dictionary<string, string[]> tagCollection)
        {
            FsPath tags = new FsPath(directory, "tags.json");
            tags.SerializeJson(tagCollection, log, true);
        }
    }
}
