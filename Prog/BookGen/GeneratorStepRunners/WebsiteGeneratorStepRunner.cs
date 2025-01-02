//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Resources;

namespace BookGen.GeneratorStepRunners;

internal sealed class WebsiteGeneratorStepRunner : GeneratorStepRunner
{
    public WebsiteGeneratorStepRunner(RuntimeSettings settings, ILogger log, IAppSetting appSetting, ProgramInfo programInfo)
        : base(settings, log, appSetting, programInfo)
    {
        AddStep(new GeneratorSteps.CreateOutputDirectory());
        AddStep(CreateAssets());
        AddStep(new GeneratorSteps.CopyAssets(settings.Configuration.TargetWeb));
        AddStep(new GeneratorSteps.ImageProcessor());
        AddStep(new GeneratorSteps.CreateToCForWebsite());
        AddStep(new GeneratorSteps.CreatePagesJS());
        AddStep(new GeneratorSteps.CreateMetadata());
        AddStep(new GeneratorSteps.CreateIndexHtml());
        AddStep(new GeneratorSteps.CreatePages());
        AddStep(new GeneratorSteps.CreateSubpageIndexes());
        AddStep(new GeneratorSteps.GenerateSearchPage());
        AddStep(new GeneratorSteps.CreateSitemap());
    }

    private IGeneratorStep CreateAssets()
    {
        var step = new GeneratorSteps.ExtractTemplateAssets();
        if (TemplateLoader.FallbackTemplateRequired(Settings.SourceDirectory, Settings.Configuration.TargetWeb))
        {
            step.Assets = new (KnownFile file, string targetPath)[]
            {
                (KnownFile.PrismCss, "Assets"),
                (KnownFile.PrismJs, "Assets"),
                (KnownFile.BootstrapMinCss, "Assets"),
                (KnownFile.BootstrapMinJs, "Assets"),
                (KnownFile.JqueryMinJs, "Assets"),
                (KnownFile.PopperMinJs, "Assets"),
            };
        }
        return step;
    }

    protected override FsPath ConfigureOutputDirectory(FsPath workingDirectory)
    {
        return workingDirectory.Combine(Settings.Configuration.TargetWeb.OutPutDirectory);
    }

    protected override string ConfigureTemplateContent()
    {
        return TemplateLoader.LoadTemplate(Settings.SourceDirectory,
                                           Settings.Configuration.TargetWeb,
                                           _log,
                                           ResourceHandler.GetFile(KnownFile.TemplateWebHtml));
    }
}
