//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Domain;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.GeneratorSteps.MarkdownGenerators;
using BookGen.Gui.ArgumentParser;
using BookGen.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace BookGen.Modules
{
    internal class PagegenModule : ModuleWithState
    {
        public PagegenModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "PageGen";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-d",
                                            "--dir",
                                            "-p",
                                            "--page",
                                            "ExternalLinks",
                                            "Chaptersummary");
            }
        }

        public override ModuleRunResult Execute(string[] arguments)
        {

            PageGenArguments args = new PageGenArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            CurrentState.Log.LogLevel = args.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

            FolderLock.ExitIfFolderIsLocked(args.Directory, CurrentState.Log);

            using (var l = new FolderLock(args.Directory))
            {
                ProjectLoader loader = new ProjectLoader(CurrentState.Log, args.Directory);

                return loader.TryLoadProjectAndExecuteOperation((config, toc) =>
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var settings = loader.CreateRuntimeSettings(config, toc, new BuildConfig());

                    switch (args.PageType)
                    {
                        case PageType.ExternalLinks:
                            RunGetLinks(settings, CurrentState.Log);
                            break;
                        case PageType.Chaptersummary:
                            RunChapterSummary(settings, CurrentState.Log);
                            break;
                    }

                    stopwatch.Stop();
                    CurrentState.Log.Info("Total runtime: {0}ms", stopwatch.ElapsedMilliseconds);

                    return true;
                }).ToSuccesOrError();
            }
        }

        private void RunChapterSummary(RuntimeSettings settings, ILog log)
        {
            var stopwordsFile = settings.SourceDirectory.Combine(settings.Configuration.StopwordsFile);

            if (!stopwordsFile.IsExisting)
            {
                log.Critical("Stopwords file specified in config doesn't exist.");
                return;
            }

            List<string> stopwords = ReadStopWords(stopwordsFile);

            if (stopwords.Count < 1)
            {
                log.Warning("Stopwords file doesn't contain words. Output may be unusable.");
                stopwords.Add(" "); //add a single space as fallback
            }

            var chapterSummerizer = new ChapterSummarizer(stopwords);
            string? content = chapterSummerizer.RunStep(settings, log);

            log.Info("Writing file: chaptersummary.md...");
            FsPath output = settings.SourceDirectory.Combine("chaptersummary.md");
            output.WriteFile(log, content);
        }

        private List<string> ReadStopWords(FsPath stopwordsFile)
        {
            string? line = null;
            List<string> results = new List<string>(100);
            using (var textreader = System.IO.File.OpenText(stopwordsFile.ToString()))
            {
                do
                {
                    line = textreader.ReadLine();
                    if (line != null
                        && !line.StartsWith("#"))
                    {
                        results.Add(line);
                    }
                }
                while (line != null);
            }

            return results;
        }

        private void RunGetLinks(RuntimeSettings settings, ILog log)
        {
            var generator = new GetLinksGenerator();
            string? content =  generator.RunStep(settings, log);

            log.Info("Writing file: links.md...");
            FsPath output = settings.SourceDirectory.Combine("links.md");
            output.WriteFile(log, content);
        }
    }
}
