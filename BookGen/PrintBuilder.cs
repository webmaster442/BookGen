//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework;
using Bookgen.Template.Properties;

namespace BookGen
{
    internal class PrintBuilder : Generator
    {
        public PrintBuilder(string workdir, Config configuration, ILog log, ShortCodeLoader loader) : base(workdir, configuration, log, loader)
        {
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyAssets(configuration.TargetPrint));
            AddStep(new GeneratorSteps.CopyImagesDirectory(false));
            AddStep(new GeneratorSteps.CreatePrintableHtml());
        }

        protected override string ConfigureTemplate()
        {
            return Resources.TemplatePrint;
        }
    }
}
