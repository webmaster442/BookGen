//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Template;
using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Framework;
using BookGen.Framework.Scripts;

namespace BookGen
{
    internal class EpubBuilder : Builder
    {
        public EpubBuilder(string workdir, Config configuration, ILog log, ScriptHandler scriptHander)
            : base(workdir, configuration, log, configuration.TargetEpub, scriptHander)
        {
            var session = new GeneratorSteps.Epub.EpubSession();

            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyAssets(configuration.TargetEpub));
            AddStep(new GeneratorSteps.CopyImagesDirectory(true, true));
            AddStep(new GeneratorSteps.Epub.CreateEpubStructure());
            AddStep(new GeneratorSteps.Epub.CreateEpubPages(session));
            AddStep(new GeneratorSteps.Epub.CreateEpubToc());
            AddStep(new GeneratorSteps.Epub.CreatePackageOpf(session));
            AddStep(new GeneratorSteps.Epub.CreateEpubPack());
        }

        protected override FsPath ConfigureOutputDirectory(FsPath workingDirectory)
        {
            return workingDirectory.Combine(Settings.Configuration.TargetEpub.OutPutDirectory);
        }

        protected override string ConfigureTemplateContent()
        {
            return TemplateLoader.LoadTemplate(Settings.SourceDirectory, 
                                               Settings.Configuration.TargetEpub,
                                               _log, 
                                               BuiltInTemplates.Epub);
        }
    }
}
