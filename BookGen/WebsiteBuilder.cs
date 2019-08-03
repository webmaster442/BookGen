//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Template;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework;

namespace BookGen
{
    internal class WebsiteBuilder : Generator
    {
        private readonly GeneratorSteps.ExtractTemplateAssets _extractAssets;

        public WebsiteBuilder(string workdir, Config configuration, ILog log) : base(workdir, configuration, log)
        {
            _extractAssets = new GeneratorSteps.ExtractTemplateAssets();

            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(_extractAssets);
            AddStep(new GeneratorSteps.CopyAssets(configuration.TargetWeb));
            AddStep(new GeneratorSteps.CopyImagesDirectory(true));
            AddStep(new GeneratorSteps.CreateToCForWebsite());
            AddStep(new GeneratorSteps.CreateMetadata());
            AddStep(new GeneratorSteps.CreateIndexHtml());
            AddStep(new GeneratorSteps.CreatePagesJS());
            AddStep(new GeneratorSteps.CreatePages());
            AddStep(new GeneratorSteps.CreateSubpageIndexes());
            AddStep(new GeneratorSteps.GenerateSearchPage());
            AddStep(new GeneratorSteps.CreateSitemap());
        }

        protected override FsPath ConfigureOutputDirectory(FsPath workingDirectory)
        {
            return workingDirectory.Combine(Settings.Configuration.TargetWeb.OutPutDirectory);
        }

        protected override string ConfigureTemplate()
        {
            if (string.IsNullOrEmpty(Settings.Configuration.TargetWeb.TemplateFile))
            {
                _extractAssets.Assets = new (string content, string targetPath)[]
                {
                    (BuiltInTemplates.AssetPrismCss, "Assets\\prism.css"),
                    (BuiltInTemplates.AssetPrismJs, "Assets\\prism.js"),
                };
                return BuiltInTemplates.TemplateWeb;
            }
            return Settings.Configuration.TargetWeb.TemplateFile;
        }
    }
}
