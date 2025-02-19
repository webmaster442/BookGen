﻿//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.CommandArguments;
using BookGen.Domain.Configuration;
using BookGen.Framework;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;

namespace BookGen.Commands;

[CommandName("externallinks")]
internal partial class ExternalLinksCommand : Command<ExternalLinksArguments>
{
    private readonly ILogger _log;
    private readonly IMutexFolderLock _folderLock;
    private readonly ProgramInfo _programInfo;

    [GeneratedRegex(@"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?")]
    private partial Regex Links();

    public ExternalLinksCommand(ILogger log, IMutexFolderLock folderLock, ProgramInfo programInfo)
    {
        _log = log;
        _folderLock = folderLock;
        _programInfo = programInfo;
    }

    public override int Execute(ExternalLinksArguments arguments, string[] context)
    {
        _programInfo.EnableVerboseLogingIfRequested(arguments);

        _folderLock.CheckLockFileExistsAndExitWhenNeeded(_log, arguments.Directory);

        var loader = new ProjectLoader(arguments.Directory, _log, _programInfo);

        if (loader.LoadProject())
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            RuntimeSettings settings = loader.CreateRuntimeSettings(new BuildConfig());

            string content = ExtractLinksToMdFile(settings, _log);

            arguments.OutputFile.WriteFile(_log, content);

            stopwatch.Stop();
            _log.LogInformation("Total runtime: {runtime}ms", stopwatch.ElapsedMilliseconds);

            return Constants.Succes;
        }

        return Constants.GeneralError;
    }

    private string ExtractLinksToMdFile(RuntimeSettings settings, ILogger log)
    {
        var links = new ConcurrentBag<string>();
        var results = new StringBuilder();

        foreach (string? chapter in settings.TocContents.Chapters)
        {
            log.LogInformation("Processing chapter: {chapter}", chapter);
            results.AppendFormat("## {0}\r\n\r\n", chapter);
            links.Clear();

            Parallel.ForEach(settings.TocContents.GetLinksForChapter(chapter), link =>
            {
                Interfaces.FsPath? input = settings.SourceDirectory.Combine(link.Url);

                string? contents = input.ReadFile(log);

                foreach (Match? match in Links().Matches(contents))
                {
                    if (match != null)
                        links.Add(match.Value);
                }
            });

            foreach (string link in links.Distinct().OrderBy(s => s))
            {
                results.AppendLine(link);
            }
            results.AppendLine();
        }

        return results.ToString();
    }
}
