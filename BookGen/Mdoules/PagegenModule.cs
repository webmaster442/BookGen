//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Domain;
using BookGen.Domain.ArgumentParsing;
using BookGen.GeneratorSteps.MarkdownGenerators;
using BookGen.Utilities;
using System;
using System.Collections.Generic;

namespace BookGen.Mdoules
{
    internal class PagegenModule : ModuleBase
    {
        public PagegenModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "PageGen";

        public bool TryGetArguments(ArgumentParser arguments, out PageGenParameters parsed)
        {
            parsed = new PageGenParameters();

            bool pageTypeSpecified = Enum.TryParse(arguments.GetSwitchWithValue("p", "page"), true, out PageType parsedPageType);
            parsed.PageType = parsedPageType;

            var dir = arguments.GetSwitchWithValue("d", "dir");

            if (!string.IsNullOrEmpty(dir))
                parsed.WorkDir = dir;

            return pageTypeSpecified;
        }

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            if (!TryGetArguments(tokenizedArguments, out PageGenParameters parameters))
                return false;

            ILog log = new ConsoleLog(LogLevel.Info);

            ProjectLoader loader = new ProjectLoader(log, parameters.WorkDir);

            if (loader.TryLoadAndValidateConfig(out var config)
                && loader.TryLoadAndValidateToc(config, out var toc)
                && config != null
                && toc != null)
            {
                var settings = loader.CreateRuntimeSettings(config, toc, new BuildConfig());

                switch (parameters.PageType)
                {
                    case PageType.ExternalLinks:
                        RunGetLinks(settings, log);
                        break;
                    case PageType.Phrases:
                        RunGetPhrases(settings, log);
                        break;
                }

                return true;
            }

            return false;
        }

        private void RunGetPhrases(RuntimeSettings settings, ILog log)
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
                stopwords.Add(" "); //add a single space as fallback;
            }

            var chapterSummerizer = new ChapterSummarizer(stopwords);
            string? content = chapterSummerizer.RunStep(settings, log);

            log.Info("Writing file: terms.md...");
            FsPath output = settings.SourceDirectory.Combine("links.md");
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

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(PagegenModule));
        }
    }
}
