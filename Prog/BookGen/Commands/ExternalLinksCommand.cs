using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.CommandArguments;
using BookGen.Domain.Configuration;
using BookGen.Framework;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;
using System.Diagnostics;

namespace BookGen.Commands
{
    [CommandName("externallinks")]
    internal partial class ExternalLinksCommand : Command<ExternalLinksArguments>
    {
        private readonly ILog _log;
        private readonly ProgramInfo _programInfo;

        [GeneratedRegex(@"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?")]
        private partial Regex Links();

        public ExternalLinksCommand(ILog log, ProgramInfo programInfo)
        {
            _log = log;
            _programInfo = programInfo;
        }

        public override int Execute(ExternalLinksArguments arguments, string[] context)
        {
            _log.LogLevel = arguments.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

            _log.CheckLockFileExistsAndExitWhenNeeded(arguments.Directory);

            using (var l = new FolderLock(arguments.Directory))
            {
                var loader = new ProjectLoader(arguments.Directory, _log, _programInfo);

                if (loader.LoadProject())
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    RuntimeSettings settings = loader.CreateRuntimeSettings(new BuildConfig());

                    string content = ExtractLinksToMdFile(settings, _log);

                    arguments.OutputFile.WriteFile(_log, content);

                    stopwatch.Stop();
                    _log.Info("Total runtime: {0}ms", stopwatch.ElapsedMilliseconds);

                    return Constants.Succes;
                }

                return Constants.GeneralError;
            }
        }

        private string ExtractLinksToMdFile(RuntimeSettings settings, ILog log)
        {
            var links = new ConcurrentBag<string>();
            var results = new StringBuilder();

            foreach (string? chapter in settings.TocContents.Chapters)
            {
                log.Info("Processing chapter: {0}", chapter);
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
}
