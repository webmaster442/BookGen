//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Domain;
using BookGen.Framework.Scripts;
using BookGen.Gui;
using System.Diagnostics;

namespace BookGen.Framework
{
    internal abstract class Builder
    {
        private readonly List<IGeneratorStep> _steps;

        protected readonly ILog _log;

        protected RuntimeSettings Settings { get; }

        protected readonly ShortCodeLoader _loader;
        private readonly CsharpScriptHandler _scriptHandler;
        private readonly StaticTemplateContent _staticContent;
        protected Builder(RuntimeSettings settings,
                          ILog log,
                          ShortCodeLoader shortCodeLoader,
                          CsharpScriptHandler scriptHandler)
        {
            Settings = settings;

            _staticContent = new StaticTemplateContent();
            _loader = shortCodeLoader;
            _loader.LoadAll();

            _scriptHandler = scriptHandler;
            scriptHandler.SetHostFromRuntimeSettings(Settings);

            _steps = new List<IGeneratorStep>();
            _log = log;
        }

        private TemplateProcessor CreateTemplateProcessor()
        {
            return new TemplateProcessor(Settings.Configuration,
                                         new ShortCodeParser(_loader.Imports,
                                                             _scriptHandler,
                                                             Settings.Configuration.Translations,
                                                             _log),
                                         _staticContent);
        }

        protected abstract string ConfigureTemplateContent();

        protected abstract FsPath ConfigureOutputDirectory(FsPath workingDirectory);

        public void AddStep(IGeneratorStep step)
        {
            _steps.Add(step);
        }

        public TimeSpan Run()
        {
            Settings.OutputDirectory = ConfigureOutputDirectory(Settings.SourceDirectory);
            Stopwatch sw = new Stopwatch();
            ConsoleProgressbar progressbar = new(0, _steps.Count);
            sw.Start();
            string stepName = string.Empty;
            try
            {
                progressbar.SwitchBuffers();
                int stepCounter = 1;
                foreach (var step in _steps)
                {
                    stepName = step.GetType().Name;
                    switch (step)
                    {
                        case ITemplatedStep templated:
                            var instance = CreateTemplateProcessor();
                            templated.Content = instance;
                            templated.Template = instance;
                            templated.Template.TemplateContent = ConfigureTemplateContent();
                            break;
                        case IGeneratorContentFillStep contentFill:
                            contentFill.Content = _staticContent;
                            break;
                    }

                    progressbar.Report(stepCounter, "Step {0} of {1}", stepCounter, _steps.Count);
                    step.RunStep(Settings, _log);
                    ++stepCounter;
                }
                progressbar.SwitchBuffers();
            }
            catch (Exception ex)
            {
                progressbar.SwitchBuffers();
                _log.Critical("Critical exception while running: {0}", stepName);
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
