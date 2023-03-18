//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Domain.Configuration;
using BookGen.Framework;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;

namespace BookGen.Commands;

[CommandName("stat")]
internal class StatCommand : Command<StatArguments>
{
    private readonly ILog _log;
    private readonly ProgramInfo _programInfo;

    public StatCommand(ILog log, ProgramInfo programInfo)
    {
        _log = log;
        _programInfo = programInfo;
    }

    private bool TryComputeStat(string input, ref StatisticsData stat)
    {
        try
        {
            string? line = null;
            using (StreamReader? reader = File.OpenText(input))
            {
                stat.Bytes += reader.BaseStream.Length;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        stat.Chars += line.Length;
                        ++stat.ParagraphLines;
                        stat.Words += line.GetWordCount();
                        stat.PageCountLines += line.Length < 80 ? 1 : line.Length / 80;
                    }
                }
                while (line != null);

                return true;
            }
        }
        catch (Exception ex)
        {
            _log.Warning("ReadFile failed: {0}", input);
            _log.Detail(ex.Message);
            return false;
        }
    }

    public override int Execute(StatArguments arguments, string[] context)
    {
        var stat = new StatisticsData();
        if (!string.IsNullOrEmpty(arguments.Input))
        {
            if (TryComputeStat(arguments.Input, ref stat))
            {
                _log.PrintLine("");
                _log.PrintLine(stat);
                return Constants.Succes;
            }
            return Constants.GeneralError;
        }

        _log.CheckLockFileExistsAndExitWhenNeeded(arguments.Directory);

        using (var l = new FolderLock(arguments.Directory))
        {
            var loader = new ProjectLoader(arguments.Directory, _log, _programInfo);
            bool result = loader.LoadProject();

            if (result)
            {
                RuntimeSettings settings = loader.CreateRuntimeSettings(new BuildConfig());
                foreach (string? link in settings.TocContents.Files)
                {
                    if (!TryComputeStat(link, ref stat))
                    {
                        result = false;
                        break;
                    }
                }
            }

            if (result)
            {
                _log.PrintLine("");
                _log.PrintLine(stat);
            }

            return result ? Constants.Succes : Constants.GeneralError;
        }

    }
}
