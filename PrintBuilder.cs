//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework;

namespace BookGen
{
    internal class PrintBuilder: Generator
    {
        public PrintBuilder(Config configuration) : base(configuration, Program.Log)
        {
            MarkdownPrintModifier.Configuration = configuration;
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyImagesDirectory());
            AddStep(new GeneratorSteps.CreatePrintableHtml());
        }
    }
}
