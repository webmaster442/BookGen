//-----------------------------------------------------------------------------
// (c) 2022-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.GeneratorSteps.Print;

internal sealed class CreateInlinedXhtml : IGeneratorStep
{
    public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
    {
        log.Info("Creating XHTML version of render...");

        log.Detail("Reading generated html5...");
        FsPath source = settings.OutputDirectory.Combine("print.html");

        string content = source.ReadFile(log);

        log.Detail("Inlining CSS...");
        var htmlTidy = new HtmlTidy();

        var xhtml = htmlTidy.HtmlToXhtml(content);

        var result = PreMailer.Net.PreMailer.MoveCssInline(xhtml, removeStyleElements: true);

        log.Info("Writing target file...");
        FsPath target = settings.OutputDirectory.Combine("print_xhtml.html");
        target.WriteFile(log, result.Html);
    }
}
