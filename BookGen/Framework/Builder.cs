//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Domain;
using BookGen.Framework.Scripts;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BookGen.Framework
{
    internal abstract class Builder
    {
        private readonly List<IGeneratorStep> _steps;

        protected readonly ILog _log;

        protected RuntimeSettings Settings { get; }

        protected TemplateProcessor Template { get; }

        protected FsPath WorkDir { get; }

        protected readonly ShortCodeLoader _loader;

        protected Builder(string workdir, Config configuration, ILog log, BuildConfig current, CsharpScriptHandler scriptHandler)
        {
            WorkDir = new FsPath(workdir);
            Settings = new RuntimeSettings
            {
                SourceDirectory = WorkDir,
                Configuration = configuration,
                TocContents = MarkdownUtils.ParseToc(WorkDir.Combine(configuration.TOCFile).ReadFile(log)),
                MetataCache = new Dictionary<string, string>(100),
                InlineImgCache = new Dictionary<string, string>(100),
                CurrentBuildConfig = current,
            };

            if (string.IsNullOrEmpty(configuration.ImageDir))
                Settings.ImageDirectory = FsPath.Empty;
            else
                Settings.ImageDirectory = WorkDir.Combine(configuration.ImageDir);

            _loader = new ShortCodeLoader(log, Settings);
            _loader.LoadAll();

            scriptHandler.SetHostFromRuntimeSettings(Settings);

            Template = new TemplateProcessor(configuration, new ShortCodeParser(_loader.Imports, scriptHandler, configuration.Translations, log));

            _steps = new List<IGeneratorStep>();
            _log = log;
        }

        protected abstract string ConfigureTemplateContent();

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
            Template.TemplateContent = ConfigureTemplateContent();

            DateTime start = DateTime.Now;
            try
            {
                int stepCounter = 1;
                foreach (var step in _steps)
                {
                    Settings.CurrentTargetFile = FsPath.Empty;
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
