//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.CommandArguments;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;

namespace BookGen.Commands;

[CommandName("tags")]
internal class TagsCommand : Command<TagsArguments>
{
    private readonly ILog _log;
    private readonly ProgramInfo _programInfo;

    public TagsCommand(ILog log, ProgramInfo programInfo)
    {
        _log = log;
        _programInfo = programInfo;
    }

    public override int Execute(TagsArguments arguments, string[] context)
    {
        _log.LogLevel = arguments.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

        _log.CheckLockFileExistsAndExitWhenNeeded(arguments.Directory);

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

            SerializeTagCollection(arguments.Directory, _log, tagUtils.TagCollection.OrderBy(x => x.Key).ToDictionary());

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
