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
    internal abstract class Generator
    {
        private readonly List<IGeneratorStep> _steps;
        private readonly ILog _log;

        protected RuntimeSettings Settings { get; }
        private Template Template { get; }
        protected GeneratorContent GeneratorContent { get; }

        protected Generator(string workdir, Config configuration, ILog log)
        {
            var dir = new FsPath(workdir);
            Settings = new RuntimeSettings
            {
                SourceDirectory = dir,
                OutputDirectory = dir.Combine(configuration.OutputDir),
                ImageDirectory = dir.Combine(configuration.ImageDir),
                TocPath = dir.Combine(configuration.TOCFile),
                Configruation = configuration,
                TocContents = MarkdownUtils.ParseToc(dir.Combine(configuration.TOCFile).ReadFile()),
                MetataCache = new Dictionary<string, string>(100),
                InlineImgCache = new Dictionary<string, string>(100)
            };
            Template = ConfigureTemplate();
            GeneratorContent = new GeneratorContent(configuration);
            _steps = new List<IGeneratorStep>();
            _log = log;
        }

        protected abstract Template ConfigureTemplate();

        public void AddStep(IGeneratorStep step)
        {
            switch (step)
            {
                case ITemplatedStep templated:
                    templated.Content = GeneratorContent;
                    templated.Template = Template;
                    _steps.Add(templated);
                    break;
                case IGeneratorContentFillStep contentFill:
                    contentFill.Content = GeneratorContent;
                    _steps.Add(contentFill);
                    break;
                default:
                    _steps.Add(step);
                    break;
            }
        }

        public TimeSpan Run()
        {
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
