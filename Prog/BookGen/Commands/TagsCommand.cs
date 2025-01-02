//-----------------------------------------------------------------------------
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
    private readonly ILogger _log;
    private readonly IMutexFolderLock _folderLock;
    private readonly ProgramInfo _programInfo;

    public TagsCommand(ILogger log, IMutexFolderLock folderLock, ProgramInfo programInfo)
    {
        _log = log;
        _folderLock = folderLock;
        _programInfo = programInfo;
    }

    public override int Execute(TagsArguments arguments, string[] context)
    {
        _programInfo.EnableVerboseLogingIfRequested(arguments);
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

            _log.LogInformation("Total runtime: {runtime}ms", stopwatch.ElapsedMilliseconds);

            return Constants.Succes;
        }

        return Constants.GeneralError;
    }

    private static void PrintStats(ILogger log, TagUtils tagUtils)
    {
        log.LogInformation("Total tags: {tags}", tagUtils.TotalTagCount);
        log.LogInformation("Total unique tags: {unique}", tagUtils.UniqueTagCount);
        log.LogInformation("Files without tags: {without}", tagUtils.FilesWithOutTags);
    }

    private static void SerializeTagCollection(string directory, ILogger log, Dictionary<string, string[]> tagCollection)
    {
        var tags = new FsPath(directory, ".bookgen/tags.json");
        tags.SerializeJson(tagCollection, log, true);
    }
}
