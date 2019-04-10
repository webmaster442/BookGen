//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework;

namespace BookGen
{
    internal class PrintBuilder: Generator
    {
        public PrintBuilder(string workdir, Config configuration, ILog log) : base(workdir, configuration, log)
        {
            MarkdownPrintModifier.Configuration = configuration;
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyImagesDirectory(false));
            AddStep(new GeneratorSteps.CreatePrintableHtml());
        }
    }
}
