//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework;

namespace BookGen
{
    internal class EpubBuilder : Generator
    {
        public EpubBuilder(string workdir, Config configuration, ILog log) : base(workdir, configuration, log)
        {
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyImagesDirectory(true, true));
            AddStep(new GeneratorSteps.CreateEpubStructure());
            AddStep(new GeneratorSteps.CreateEpubPages());
            AddStep(new GeneratorSteps.CreateEpubToc());
        }
    }
}
