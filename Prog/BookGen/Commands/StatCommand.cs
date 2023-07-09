//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
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
    private readonly ProgramInfo _programInfo;

    public StatCommand(ILog log, ITerminal terminal, ProgramInfo programInfo)
    {
        _log = log;
        _terminal = terminal;
        _programInfo = programInfo;
    }

    public override int Execute(StatArguments arguments, string[] context)
    {
        if (!string.IsNullOrEmpty(arguments.Input))
        {
            var result = StatisticsCollector.ComputeStatistics(arguments.Input, _log);
            Print(result, null);
        }

        _log.CheckLockFileExistsAndExitWhenNeeded(arguments.Directory);

        using (var l = new FolderLock(arguments.Directory))
        {
            var loader = new ProjectLoader(arguments.Directory, _log, _programInfo);
            bool result = loader.LoadProject();

            if (!result)
            {
                return Constants.GeneralError;
            }

            RuntimeSettings settings = loader.CreateRuntimeSettings(new BuildConfig());

            Dictionary<string, StatisticResult> _results = new();
            StatisticResult sumStat = new();
            foreach (var chapter in loader.Toc.Chapters)
            {
                var chapterFiles = loader.Toc.GetLinksForChapter(chapter).Select(l => l.Url);
                var chapterStat = StatisticsCollector.ComputeStatistics(chapterFiles, _log);
                sumStat += chapterStat;
                _results.Add(chapter, chapterStat);
            }

            Print(sumStat, _results);
        }

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