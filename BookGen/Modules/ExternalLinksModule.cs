//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core.Configuration;
using BookGen.Domain;
using BookGen.Domain.ArgumentParsing;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Utilities;
using System.Diagnostics;

namespace BookGen.Modules
{
    internal class ExternalLinksModule : ModuleWithState
    {
        private readonly Regex _link;

        public ExternalLinksModule(ProgramState currentState) : base(currentState)
        {
            _link = new Regex(@"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?", RegexOptions.Compiled);
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
                ProjectLoader loader = new ProjectLoader(CurrentState.Log, args.Directory);

                return loader.TryLoadProjectAndExecuteOperation((config, toc) =>
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var settings = loader.CreateRuntimeSettings(config, toc, new TagUtils(), new BuildConfig());

                    string content = ExtractLinksToMdFile(settings, CurrentState.Log);

                    args.OutputFile.WriteFile(CurrentState.Log, content);

                    stopwatch.Stop();
                    CurrentState.Log.Info("Total runtime: {0}ms", stopwatch.ElapsedMilliseconds);

                    return true;
                }).ToSuccesOrError();
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
                    var input = settings.SourceDirectory.Combine(link.Url);

                    var contents = input.ReadFile(log);

                    foreach (Match? match in _link.Matches(contents))
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
