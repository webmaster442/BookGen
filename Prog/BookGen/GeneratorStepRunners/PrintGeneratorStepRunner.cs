//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Framework.Scripts;
using BookGen.Resources;

namespace BookGen.GeneratorStepRunners;

internal sealed class PrintGeneratorStepRunner : GeneratorStepRunner
{
    public PrintGeneratorStepRunner(RuntimeSettings settings, ILog log, ShortCodeLoader loader, CsharpScriptHandler scriptHandler)
        : base(settings, log, loader, scriptHandler)
    {
        AddStep(new GeneratorSteps.CreateOutputDirectory());
        AddStep(new GeneratorSteps.CopyAssets(settings.Configuration.TargetPrint));
        AddStep(new GeneratorSteps.ImageProcessor());
        AddStep(new GeneratorSteps.Print.CreatePrintableHtml());
        AddStep(new GeneratorSteps.Print.CreateInlinedXhtml());
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
