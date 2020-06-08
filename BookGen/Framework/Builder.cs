//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core;
using BookGen.Domain;
using BookGen.Framework.Scripts;
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

        protected readonly ShortCodeLoader _loader;

        protected Builder(RuntimeSettings settings,
                          ILog log,
                          CsharpScriptHandler scriptHandler)
        {
            Settings = settings;

            _loader = new ShortCodeLoader(log, Settings, Program.AppSetting);
            _loader.LoadAll();

            scriptHandler.SetHostFromRuntimeSettings(Settings);

            Template = new TemplateProcessor(settings.Configuration, new ShortCodeParser(_loader.Imports, scriptHandler, settings.Configuration.Translations, log));

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
            Settings.OutputDirectory = ConfigureOutputDirectory(Settings.SourceDirectory);
            Template.TemplateContent = ConfigureTemplateContent();

            Stopwatch sw = new Stopwatch();
            sw.Start();
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
            }
            catch (Exception ex)
            {
                _log.Critical(ex);
#if DEBUG
                Debugger.Break();
#endif
            }
            finally
            {
                sw.Stop();
            }
            return sw.Elapsed;
        }
    }
}
