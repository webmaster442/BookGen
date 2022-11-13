//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Configuration;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.ProjectHandling;
using System.Diagnostics;

namespace BookGen.Modules
{
    internal sealed partial class ExternalLinksModule : ModuleWithState
    {
        [GeneratedRegex(@"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?")]
        private partial Regex Links();

        public ExternalLinksModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "ExternalLinks";

        public override ModuleRunResult Execute(string[] arguments)
        {
            ExternalLinksArguments args = new();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            CurrentState.Log.LogLevel = args.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

            CheckLockFileExistsAndExitWhenNeeded(args.Directory);

            using (var l = new FolderLock(args.Directory))
            {
                var loader = new ProjectLoader(args.Directory, CurrentState.Log);

                if (loader.LoadProject())
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    RuntimeSettings settings = loader.CreateRuntimeSettings(new BuildConfig());

                    string content = ExtractLinksToMdFile(settings, CurrentState.Log);

                    args.OutputFile.WriteFile(CurrentState.Log, content);

                    stopwatch.Stop();
                    CurrentState.Log.Info("Total runtime: {0}ms", stopwatch.ElapsedMilliseconds);

                    return ModuleRunResult.Succes;
                }

                return ModuleRunResult.GeneralError;
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
