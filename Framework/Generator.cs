﻿//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.GeneratorSteps;
using BookGen.Utilities;
using NLog;
using System;
using System.Collections.Generic;

namespace BookGen.Framework
{
    internal abstract class Generator
    {
        private readonly List<IGeneratorStep> _steps;
        private readonly ILogger _log;

        protected GeneratorSettings Settings { get; }
        protected Template Template { get; }
        protected GeneratorContent GeneratorContent { get; }

        public Generator(Config configuration, ILogger log)
        {
            Settings = new GeneratorSettings
            {
                SourceDirectory = new FsPath(Environment.CurrentDirectory),
                OutputDirectory = configuration.OutputDir.ToPath(),
                ImageDirectory = configuration.ImageDir.ToPath(),
                Toc = configuration.TOCFile.ToPath(),
                Configruation = configuration,
                TocContents = MarkdownUtils.ParseToc(configuration.TOCFile.ToPath().ReadFile())
            };
            Template = new Template(configuration.Template.ToPath());
            GeneratorContent = new GeneratorContent(configuration);
            _steps = new List<IGeneratorStep>();
            _log = log;
        }

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

        public void Run()
        {
            try
            {
                int stepCounter = 1;
                foreach (var step in _steps)
                {
                    Console.Write("Step {0} of {1}: ", stepCounter, _steps.Count);
                    step.RunStep(Settings, _log);
                    ++stepCounter;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in step. Aborting generation");
                _log.Fatal(ex);
            }
        }
    }
}
