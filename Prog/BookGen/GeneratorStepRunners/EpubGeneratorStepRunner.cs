//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Framework.Scripts;
using BookGen.Interfaces;
using BookGen.Resources;

namespace BookGen
{
    internal sealed class EpubGeneratorStepRunner : GeneratorStepRunner
    {
        public EpubGeneratorStepRunner(RuntimeSettings settings, ILog log, ShortCodeLoader loader, CsharpScriptHandler scriptHandler)
            : base(settings, log, loader, scriptHandler)
        {
            var session = new GeneratorSteps.Epub.EpubSession();

            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyAssets(settings.Configuration.TargetEpub));
            AddStep(new GeneratorSteps.ImageProcessor());
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
                                               ResourceHandler.GetFile(KnownFile.TemplateEpubHtml));
        }
    }
}
