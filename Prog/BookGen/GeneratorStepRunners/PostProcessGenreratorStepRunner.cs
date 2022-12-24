//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Framework.Scripts;
using BookGen.Interfaces;

namespace BookGen.GeneratorStepRunners
{
    internal sealed class PostProcessGenreratorStepRunner : GeneratorStepRunner
    {
        public PostProcessGenreratorStepRunner(RuntimeSettings settings, ILog log, ShortCodeLoader shortCodeLoader, CsharpScriptHandler scriptHandler) 
            : base(settings, log, shortCodeLoader, scriptHandler)
        {
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.ImageProcessor());
            AddStep(new GeneratorSteps.CreatePages());
        }

        protected override FsPath ConfigureOutputDirectory(FsPath workingDirectory)
        {
            return workingDirectory.Combine(Settings.Configuration.TargetPostProcess.OutPutDirectory);
        }

        protected override string ConfigureTemplateContent()
        {
            return TemplateLoader.LoadTemplate(Settings.SourceDirectory,
                                               Settings.Configuration.TargetPostProcess,
                                               _log,
                                               "<!--{content}-->");
        }
    }
}
