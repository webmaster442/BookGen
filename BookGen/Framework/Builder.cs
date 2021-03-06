﻿//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
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

        protected readonly ShortCodeLoader _loader;
        private readonly CsharpScriptHandler _scriptHandler;
        private readonly StaticTemplateContent _staticContent;
        protected Builder(RuntimeSettings settings,
                          ILog log,
                          CsharpScriptHandler scriptHandler)
        {
            Settings = settings;

            _staticContent = new StaticTemplateContent();
            _loader = new ShortCodeLoader(log, Settings, Program.AppSetting);
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
            sw.Start();
            try
            {
                int stepCounter = 1;
                foreach (var step in _steps)
                {
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

                    _log.Info("Step {0} of {1}", stepCounter, _steps.Count);
                    step.RunStep(Settings, _log);
                    ++stepCounter;
                }
            }
            catch (Exception ex)
            {
                _log.Critical(ex);

#if TESTBUILD
                if (Program.IsTesting)
                {
                    Program.ErrorHappened = true;
                    Program.ErrorText = ex.Message;
                }
#endif
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
