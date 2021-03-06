﻿//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Domain;
using BookGen.Framework;
using BookGen.Framework.Scripts;
using BookGen.Resources;

namespace BookGen
{
    internal class EpubBuilder : Builder
    {
        public EpubBuilder(RuntimeSettings settings, ILog log, CsharpScriptHandler scriptHandler)
            : base(settings, log, scriptHandler)
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
