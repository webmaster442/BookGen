//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Framework.Scripts;
using BookGen.Interfaces;
using BookGen.Resources;

namespace BookGen
{
    internal class WebsiteGeneratorStepRunner : GeneratorStepRunner
    {
        public WebsiteGeneratorStepRunner(RuntimeSettings settings, ILog log, ShortCodeLoader loader, CsharpScriptHandler scriptHandler)
            : base(settings, log, loader, scriptHandler)
        {
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(CreateAssets());
            AddStep(new GeneratorSteps.CopyAssets(settings.Configuration.TargetWeb));
            AddStep(new GeneratorSteps.ImageProcessor());
            AddStep(new GeneratorSteps.CreateToCForWebsite());
            AddStep(new GeneratorSteps.CreatePagesJS());
            AddStep(new GeneratorSteps.CreateMetadata());
            AddStep(new GeneratorSteps.CreateIndexHtml());
            AddStep(new GeneratorSteps.CreatePages());
            AddStep(new GeneratorSteps.CreateSubpageIndexes());
            AddStep(new GeneratorSteps.GenerateSearchPage());
            AddStep(new GeneratorSteps.CreateSitemap());
        }

        private IGeneratorStep CreateAssets()
        {
            var step = new GeneratorSteps.ExtractTemplateAssets();
            if (TemplateLoader.FallbackTemplateRequired(Settings.SourceDirectory, Settings.Configuration.TargetWeb))
            {
                step.Assets = new (KnownFile file, string targetPath)[]
                {
                    (KnownFile.PrismCss, "Assets"),
                    (KnownFile.PrismJs, "Assets"),
                    (KnownFile.BootstrapMinCss, "Assets"),
                    (KnownFile.BootstrapMinJs, "Assets"),
                    (KnownFile.JqueryMinJs, "Assets"),
                    (KnownFile.PopperMinJs, "Assets"),
                    (KnownFile.TurbolinksJs, "Assets"),
                };
            }
            return step;
        }

        protected override FsPath ConfigureOutputDirectory(FsPath workingDirectory)
        {
            return workingDirectory.Combine(Settings.Configuration.TargetWeb.OutPutDirectory);
        }

        protected override string ConfigureTemplateContent()
        {
            return TemplateLoader.LoadTemplate(Settings.SourceDirectory,
                                               Settings.Configuration.TargetWeb,
                                               _log,
                                               ResourceHandler.GetFile(KnownFile.TemplateWebHtml));
        }
    }
}
