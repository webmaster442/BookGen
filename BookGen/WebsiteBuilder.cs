//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Template.Properties;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework;

namespace BookGen
{
    internal class WebsiteBuilder : Generator
    {
        public WebsiteBuilder(string workdir, Config configuration, ILog log, ShortCodeLoader loader) : base(workdir, configuration, log, loader)
        {
            AddStep(new GeneratorSteps.CreateOutputDirectory());
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

        protected override string ConfigureTemplate()
        {
            return Resources.TemplateEpub;
        }
    }
}
