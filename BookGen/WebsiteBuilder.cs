//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Template;
using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Framework;
using BookGen.Framework.Scripts;

namespace BookGen
{
    internal class WebsiteBuilder : Builder
    {
        private readonly GeneratorSteps.ExtractTemplateAssets _extractAssets;

        public WebsiteBuilder(string workdir, Config configuration, ILog log, CsharpScriptHandler scriptHandler) 
            : base(workdir, configuration, log, configuration.TargetWeb, scriptHandler)
        {
            _extractAssets = new GeneratorSteps.ExtractTemplateAssets();

            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(_extractAssets);
            AddStep(new GeneratorSteps.CopyAssets(configuration.TargetWeb));
            AddStep(new GeneratorSteps.CopyImagesDirectory(true));
            AddStep(new GeneratorSteps.CreateToCForWebsite());
            AddStep(new GeneratorSteps.CreatePagesJS());
            AddStep(new GeneratorSteps.CreateMetadata());
            AddStep(new GeneratorSteps.CreateIndexHtml());
            AddStep(new GeneratorSteps.CreatePages());
            AddStep(new GeneratorSteps.CreateSubpageIndexes());
            AddStep(new GeneratorSteps.GenerateSearchPage());
            AddStep(new GeneratorSteps.CreateSitemap());
        }

        protected override FsPath ConfigureOutputDirectory(FsPath workingDirectory)
        {
            return workingDirectory.Combine(Settings.Configuration.TargetWeb.OutPutDirectory);
        }

        protected override string ConfigureTemplateContent()
        {
            if (TemplateLoader.FallbackTemplateRequired(Settings.SourceDirectory, Settings.Configuration.TargetWeb))
            {
                _extractAssets.Assets = new (string content, string targetPath)[]
                {
                    (BuiltInTemplates.AssetPrismCss, "Assets\\prism.css"),
                    (BuiltInTemplates.AssetPrismJs, "Assets\\prism.js"),
                    (BuiltInTemplates.AssetBootstrapCSS, "Assets\\bootstrap.min.css"),
                    (BuiltInTemplates.AssetBootstrapJs, "Assets\\bootstrap.min.js"),
                    (BuiltInTemplates.AssetJqueryJs, "Assets\\jquery.min.js"),
                    (BuiltInTemplates.AssetPopperJs, "Assets\\popper.min.js"),
                    (BuiltInTemplates.AssetTurbolinksJs, "Assets\\turbolinks.js"),
                };
            }

            return TemplateLoader.LoadTemplate(Settings.SourceDirectory,
                                               Settings.Configuration.TargetWeb,
                                               _log,
                                               BuiltInTemplates.TemplateWeb);
        }
    }
}
