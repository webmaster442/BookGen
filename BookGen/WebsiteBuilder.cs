//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Template.Properties;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework;

namespace BookGen
{
    internal class WebsiteBuilder : Generator
    {
        private (string content, string targetPath)[] _templateAssets;

        public WebsiteBuilder(string workdir, Config configuration, ILog log, ShortCodeLoader loader) : base(workdir, configuration, log, loader)
        {
            _templateAssets = new (string content, string targetPath)[0];

            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.ExtractTemplateAssets(_templateAssets));
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
            return workingDirectory.Combine(Configuration.TargetWeb.OutPutDirectory);
        }

        protected override string ConfigureTemplate()
        {
            if (!string.IsNullOrEmpty(Configuration.TargetWeb.TemplateFile))
            {
                _templateAssets = new (string content, string targetPath)[]
                {
                    (Resources.PrismCss, "Assets\\prism.css"),
                    (Resources.PrismJs, "Assets\\prism.js"),
                };
                return Resources.TemplateWeb;

            }
            return Configuration.TargetWeb.TemplateFile;
        }
    }
}
