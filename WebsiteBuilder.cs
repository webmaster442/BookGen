//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework;
using Newtonsoft.Json;
using BookGen.Utilities;
using System.Collections.Generic;

namespace BookGen
{
    internal class WebsiteBuilder : Generator
    {
        public WebsiteBuilder(Config configuration, FsPath menuPath) : base(configuration)
        {
            MarkdownModifier.Config = configuration;

            if (menuPath.IsExisting)
            {
                var menuItems = JsonConvert.DeserializeObject<List<HeaderMenuItem>>(menuPath.ReadFile());
                AddStep(new GeneratorSteps.CreateBootstrapMenuStructure(menuItems));
                AddStep(new GeneratorSteps.CreateAdditionalPages(menuItems));
            }

            AddStep(new GeneratorSteps.CreateTOCForWebsite());
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyAssets());
            AddStep(new GeneratorSteps.CopyImagesDirectory());
            AddStep(new GeneratorSteps.CreateIndexHtml());
            AddStep(new GeneratorSteps.CreatePagesJS());
            AddStep(new GeneratorSteps.CreatePages());
            AddStep(new GeneratorSteps.CreateSubpageIndexes());
            AddStep(new GeneratorSteps.GenerateSearchPage());
            AddStep(new GeneratorSteps.CreateSitemap());
        }
    }
}
