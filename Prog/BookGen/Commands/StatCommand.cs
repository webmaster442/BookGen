//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Domain.Configuration;
using BookGen.Framework;
using BookGen.Gui;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;

namespace BookGen.Commands;

[CommandName("stat")]
internal class StatCommand : Command<StatArguments>
{
    private readonly ILog _log;
    private readonly ITerminal _terminal;
    private readonly IMutexFolderLock _folderLock;
    private readonly ProgramInfo _programInfo;

    public StatCommand(ILog log,
                       ITerminal terminal,
                       IMutexFolderLock folderLock,
                       ProgramInfo programInfo)
    {
        _log = log;
        _terminal = terminal;
        _folderLock = folderLock;
        _programInfo = programInfo;
    }

    public override int Execute(StatArguments arguments, string[] context)
    {
        if (!string.IsNullOrEmpty(arguments.Input))
        {
            var singleResult = StatisticsCollector.ComputeStatistics(arguments.Input, _log);
            Print(singleResult, null);
        }

        _folderLock.CheckLockFileExistsAndExitWhenNeeded(_log, arguments.Directory);

        var loader = new ProjectLoader(arguments.Directory, _log, _programInfo);
        bool result = loader.LoadProject();

        if (!result)
        {
            return Constants.GeneralError;
        }

        RuntimeSettings settings = loader.CreateRuntimeSettings(new BuildConfig());

        Dictionary<string, StatisticResult> results = new();
        StatisticResult sumStat = new();
        foreach (var chapter in loader.Toc.Chapters)
        {
            var chapterFiles = loader.Toc.GetLinksForChapter(chapter).Select(l => l.Url);
            var chapterStat = StatisticsCollector.ComputeStatistics(chapterFiles, _log);
            sumStat += chapterStat;
            results.Add(chapter, chapterStat);
        }

        Print(sumStat, results);

        return Constants.Succes;
    }

    private void Print(StatisticResult sumStat, IDictionary<string, StatisticResult>? results)
    {
        _terminal.Table<object>(sumStat.ToTable());
        if (results != null)
        {
            var graphData = results.ToDictionary(x => x.Key, x => (double)x.Value.Bytes);
            _terminal.BreakDownChart(graphData);
        }
    }
}