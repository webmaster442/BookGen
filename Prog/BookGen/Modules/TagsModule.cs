﻿//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Interfaces;
using BookGen.ProjectHandling;
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
                var loader = new ProjectLoader(args.Directory, CurrentState.Log);

                bool result = loader.LoadProject();

                if (result)
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var tagUtils = new WritableTagUtils(loader.Tags,
                                                        loader.Configuration.BookLanguage,
                                                        CurrentState.Log);

                    tagUtils.DeleteNoLongerExisting(loader.Toc);
                    tagUtils.CreateNotYetExisting(loader.Toc);

                    if (args.AutoGenerateTags)
                        tagUtils.AutoGenerate(loader.Toc, args.AutoKeyWordCount);

                    PrintStats(CurrentState.Log, tagUtils);

                    SerializeTagCollection(args.Directory, CurrentState.Log, tagUtils.TagCollection);

                    CurrentState.Log.Info("Total runtime: {0}ms", stopwatch.ElapsedMilliseconds);

                    return ModuleRunResult.Succes;
                }

                return ModuleRunResult.GeneralError;
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
            var tags = new FsPath(directory, ".bookgen/tags.json");
            tags.SerializeJson(tagCollection, log, true);
        }
    }
}
