//-----------------------------------------------------------------------------
// (c) 2022-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
namespace BookGen.GeneratorStepRunners;

internal sealed class PostProcessGenreratorStepRunner : GeneratorStepRunner
{
    public PostProcessGenreratorStepRunner(RuntimeSettings settings, ILogger log, IAppSetting appSetting, ProgramInfo programInfo)
        : base(settings, log, appSetting, programInfo)
    {
        AddStep(new GeneratorSteps.CreateOutputDirectory());
        AddStep(new GeneratorSteps.ImageProcessor());
        AddStep(new GeneratorSteps.CreatePages());
    }

    protected override FsPath ConfigureOutputDirectory(FsPath workingDirectory)
    {
        return workingDirectory.Combine(Settings.Configuration.TargetPostProcess.OutPutDirectory);
    }

    protected override string ConfigureTemplateContent()
    {
        return TemplateLoader.LoadTemplate(Settings.SourceDirectory,
                                           Settings.Configuration.TargetPostProcess,
                                           _log,
                                           "<!--{content}-->");
    }
}
