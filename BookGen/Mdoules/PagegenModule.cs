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
                        break;
                }

                return true;
            }

            return false;
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
