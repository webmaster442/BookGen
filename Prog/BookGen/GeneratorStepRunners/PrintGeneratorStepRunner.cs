//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Resources;

namespace BookGen.GeneratorStepRunners;

internal sealed class PrintGeneratorStepRunner : GeneratorStepRunner
{
    public PrintGeneratorStepRunner(RuntimeSettings settings, ILog log, ShortCodeLoader loader)
        : base(settings, log, loader)
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
