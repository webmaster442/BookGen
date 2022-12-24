//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

namespace BookGen.GeneratorSteps.Print
{
    internal sealed class CreateInlinedXhtml : IGeneratorStep
    {
        public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
        {
            log.Info("Reading generated html5...");
            FsPath source = settings.OutputDirectory.Combine("print.html");

            string content = source.ReadFile(log);

            log.Info("Inlining CSS...");
            var htmlTidy = new HtmlTidy();

            var xhtml = htmlTidy.HtmlToXhtml(content);
            
            var result = PreMailer.Net.PreMailer.MoveCssInline(xhtml, removeStyleElements: true);



            log.Info("Writing target file...");
            FsPath target = settings.OutputDirectory.Combine("print_xhtml.html");
            target.WriteFile(log, result.Html);
        }
    }
}
