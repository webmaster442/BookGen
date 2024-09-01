//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;

namespace BookGen.GeneratorStepRunners;

internal sealed class WordpressGeneratorStepRunner : GeneratorStepRunner
{
    public WordpressGeneratorStepRunner(RuntimeSettings settings, ILogger log, IAppSetting appSetting)
        : base(settings, log, appSetting)
    {
        var session = new GeneratorSteps.Wordpress.Session();
        AddStep(new GeneratorSteps.CreateOutputDirectory());
        AddStep(new GeneratorSteps.ImageProcessor());
        AddStep(new GeneratorSteps.Wordpress.CreateWpChannel(session));
        AddStep(new GeneratorSteps.Wordpress.CreateWpPages(session));
        AddStep(new GeneratorSteps.Wordpress.WriteExportXmlFile(session));
    }

    protected override FsPath ConfigureOutputDirectory(FsPath workingDirectory)
    {
        return workingDirectory.Combine(Settings.Configuration.TargetWordpress.OutPutDirectory);
    }

    protected override string ConfigureTemplateContent()
    {
        return "{{content}}";
    }
}
