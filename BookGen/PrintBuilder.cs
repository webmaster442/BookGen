//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Resources;
using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Framework;
using BookGen.Framework.Scripts;

namespace BookGen
{
    internal class PrintBuilder : Builder
    {
        public PrintBuilder(string workdir, Config configuration, ILog log, CsharpScriptHandler scriptHandler) 
            : base(workdir, configuration, log, configuration.TargetPrint, scriptHandler)
        {
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyAssets(configuration.TargetPrint));
            AddStep(new GeneratorSteps.ImageProcessor());
            AddStep(new GeneratorSteps.CreatePrintableHtml());
        }

        protected override FsPath ConfigureOutputDirectory(FsPath workingDirectory)
        {
            return workingDirectory.Combine(Settings.Configuration.TargetPrint.OutPutDirectory);
        }

        protected override string ConfigureTemplateContent()
        {
            return TemplateLoader.LoadTemplate(Settings.SourceDirectory,
                                               Settings.Configuration.TargetPrint,
                                               _log,
                                               ResourceHandler.GetFile(KnownFile.TemplatePrintHtml));
        }
    }
}
