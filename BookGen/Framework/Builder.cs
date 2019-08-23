//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BookGen.Framework
{
    internal abstract class Builder
    {
        private readonly List<IGeneratorStep> _steps;
        private readonly ILog _log;

        protected RuntimeSettings Settings { get; }

        protected Template Template { get; }

        protected FsPath WorkDir { get; }

        protected readonly ShortCodeLoader _loader;

        protected Builder(string workdir, Config configuration, ILog log, BuildConfig current)
        {
            WorkDir = new FsPath(workdir);
            Settings = new RuntimeSettings
            {
                SourceDirectory = WorkDir,
                ImageDirectory = WorkDir.Combine(configuration.ImageDir),
                TocPath = WorkDir.Combine(configuration.TOCFile),
                Configuration = configuration,
                TocContents = MarkdownUtils.ParseToc(WorkDir.Combine(configuration.TOCFile).ReadFile()),
                MetataCache = new Dictionary<string, string>(100),
                InlineImgCache = new Dictionary<string, string>(100),
                CurrentBuildConfig = current,
            };

            _loader = new ShortCodeLoader(log, Settings);
            _loader.LoadAll();

            Template = new Template(configuration, new ShortCodeParser(_loader.Imports));

            _steps = new List<IGeneratorStep>();
            _log = log;
        }

        protected abstract string ConfigureTemplate();

        protected abstract FsPath ConfigureOutputDirectory(FsPath workingDirectory);

        public void AddStep(IGeneratorStep step)
        {
            switch (step)
            {
                case ITemplatedStep templated:
                    templated.Content = Template;
                    templated.Template = Template;
                    _steps.Add(templated);
                    break;
                case IGeneratorContentFillStep contentFill:
                    contentFill.Content = Template;
                    _steps.Add(contentFill);
                    break;
                default:
                    _steps.Add(step);
                    break;
            }
        }

        public TimeSpan Run()
        {
            Settings.OutputDirectory = ConfigureOutputDirectory(WorkDir);
            Template.TemplateContent = ConfigureTemplate();

            DateTime start = DateTime.Now;
            try
            {
                int stepCounter = 1;
                foreach (var step in _steps)
                {
                    _log.Info("Step {0} of {1}", stepCounter, _steps.Count);
                    step.RunStep(Settings, _log);
                    ++stepCounter;
                }
                return DateTime.Now - start;
            }
            catch (Exception ex)
            {
                _log.Critical(ex);
#if DEBUG
                Debugger.Break();
#endif
                return DateTime.Now - start;
            }
        }
    }
}
