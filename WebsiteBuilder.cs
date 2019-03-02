//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework;
using BookGen.Utilities;
using System.IO;

namespace BookGen
{
    internal class WebsiteBuilder : Generator
    {
        public WebsiteBuilder(Config configuration) : base(configuration)
        {
            FillToc();
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyAssets());
            AddStep(new GeneratorSteps.CopyImagesDirectory());
            AddStep(new GeneratorSteps.CreateIndexHtml());
            AddStep(new GeneratorSteps.CreatePagesJS());
            AddStep(new GeneratorSteps.CreatePages());
            AddStep(new GeneratorSteps.CreateSubpageIndexes());
        }

        private void FillToc()
        {
            var tocContent = MarkdownUtils.Markdown2WebHTML(Settings.Toc.ReadFile());
            foreach (var file in Settings.TocContents.Files)
            {
                tocContent = tocContent.Replace(file, Settings.Configruation.HostName + Path.ChangeExtension(file, ".html"));
            }
            GeneratorContent.TableOfContents = tocContent;
        }
    }
}
