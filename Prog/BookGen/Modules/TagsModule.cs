//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Interfaces;
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
            TagsArguments args = new();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            CurrentState.Log.LogLevel = args.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

            CheckLockFileExistsAndExitWhenNeeded(args.Directory);

            using (var l = new FolderLock(args.Directory))
            {
                var loader = new ProjectLoader(CurrentState.Log, args.Directory);
                return loader.TryLoadProjectAndExecuteOperation((config, toc) =>
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var tags = new FsPath(args.Directory, "tags.json");

                    var tagUtils = loader.GetWritableTagutils(config.BookLanguage);

                    tagUtils.DeleteNoLongerExisting(toc);
                    tagUtils.CreateNotYetExisting(toc);
                    
                    if (args.AutoGenerateTags)
                        tagUtils.AutoGenerate(toc, args.AutoKeyWordCount);

                    PrintStats(CurrentState.Log, tagUtils);


                    SerializeTagCollection(args.Directory, CurrentState.Log, tagUtils.TagCollection);

                    CurrentState.Log.Info("Total runtime: {0}ms", stopwatch.ElapsedMilliseconds);
                    return true;

                }) ? ModuleRunResult.Succes : ModuleRunResult.GeneralError;
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
            var tags = new FsPath(directory, "tags.json");
            tags.SerializeJson(tagCollection, log, true);
        }
    }
}
