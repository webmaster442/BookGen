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
    internal class EpubBuilder : Generator
    {
        public EpubBuilder(string workdir, Config configuration, ILog log, ShortCodeLoader loader) : base(workdir, configuration, log, loader)
        {
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyAssets(configuration.TargetEpub));
            AddStep(new GeneratorSteps.CopyImagesDirectory(true, true));
            AddStep(new GeneratorSteps.CreateEpubStructure());
            AddStep(new GeneratorSteps.CreateEpubPages());
            AddStep(new GeneratorSteps.CreateEpubToc());
            AddStep(new GeneratorSteps.CreateEpubContent());
            AddStep(new GeneratorSteps.CreateEpubPack());
        }

        protected override FsPath ConfigureOutputDirectory(FsPath workingDirectory)
        {
            return workingDirectory.Combine(Settings.Configuration.TargetEpub.OutPutDirectory);
        }

        protected override string ConfigureTemplate()
        {
            return Resources.TemplateEpub;
        }
    }
}
