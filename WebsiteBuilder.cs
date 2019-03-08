//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework;

namespace BookGen
{
    internal class WebsiteBuilder : Generator
    {
        public WebsiteBuilder(Config configuration) : base(configuration)
        {
            MarkdownModifier.Config = configuration;

            AddStep(new GeneratorSteps.CreateTOCForWebsite());
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyAssets());
            AddStep(new GeneratorSteps.CopyImagesDirectory());
            AddStep(new GeneratorSteps.CreateIndexHtml());
            AddStep(new GeneratorSteps.CreatePagesJS());
            AddStep(new GeneratorSteps.CreatePages());
            AddStep(new GeneratorSteps.CreateSubpageIndexes());
            AddStep(new GeneratorSteps.GenerateSearchPage());
            //Note: Cache manifest needs to be last, because
            //it has to know about all generated content
            AddStep(new GeneratorSteps.CreateCacheManifest());
        }
    }
}
