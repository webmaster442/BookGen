//-----------------------------------------------------------------------------
// (c) 2022-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.GeneratorSteps.Print;

internal sealed class CreateInlinedXhtml : IGeneratorStep
{
    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        if (settings.CurrentBuildConfig.ImageOptions.SvgPassthru)
        {
            log.LogInformation("Skipping xhtml generation, because svg passthrough is enabled...");
            return;
        }

        log.LogInformation("Creating XHTML version of render...");

        log.LogDebug("Reading generated html5...");
        FsPath source = settings.OutputDirectory.Combine("print.html");

        string content = source.ReadFile(log);

        log.LogDebug("Inlining CSS...");
        var htmlTidy = new HtmlTidy(log);

        var xhtml = htmlTidy.HtmlToXhtml(content);

        var result = PreMailer.Net.PreMailer.MoveCssInline(xhtml, removeStyleElements: true);

        log.LogInformation("Writing target file...");
        FsPath target = settings.OutputDirectory.Combine("print_xhtml.html");
        target.WriteFile(log, result.Html);
    }
}
