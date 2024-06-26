﻿//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.CommandArguments;
using BookGen.Framework;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;

namespace BookGen.Commands;

[CommandName("tags")]
internal class TagsCommand : Command<TagsArguments>
{
    private readonly ILog _log;
    private readonly IMutexFolderLock _folderLock;
    private readonly ProgramInfo _programInfo;

    public TagsCommand(ILog log, IMutexFolderLock folderLock, ProgramInfo programInfo)
    {
        _log = log;
        _folderLock = folderLock;
        _programInfo = programInfo;
    }

    public override int Execute(TagsArguments arguments, string[] context)
    {
        _log.EnableVerboseLogingIfRequested(arguments);
        _folderLock.CheckLockFileExistsAndExitWhenNeeded(_log, arguments.Directory);

        var loader = new ProjectLoader(arguments.Directory, _log, _programInfo);

        bool result = loader.LoadProject();

        if (result)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tagUtils = new WritableTagUtils(loader.Tags,
                                                loader.Configuration.BookLanguage,
                                                _log);

            tagUtils.DeleteNoLongerExisting(loader.Toc);
            tagUtils.CreateNotYetExisting(loader.Toc);

            if (arguments.AutoGenerateTags)
                tagUtils.AutoGenerate(loader.Toc, arguments.AutoKeyWordCount);

            PrintStats(_log, tagUtils);

            var procectFiles = loader.Toc.Files.ToArray();

            SerializeTagCollection(arguments.Directory, _log, tagUtils.TagCollection.OrderBy(x => Array.IndexOf(procectFiles, x)).ToDictionary());

            _log.Info("Total runtime: {0}ms", stopwatch.ElapsedMilliseconds);

            return Constants.Succes;
        }

        return Constants.GeneralError;
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
