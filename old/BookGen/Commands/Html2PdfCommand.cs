//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;

namespace BookGen.Commands;

[CommandName("html2pdf")]
internal sealed class Html2PdfCommand : AsyncCommand<Html2PdfArguments>
{
    private readonly BrowserInteract _browser;

    public override SupportedOs SupportedOs => SupportedOs.Windows;

    public Html2PdfCommand(ILogger log)
    {
        _browser = new BrowserInteract(log);
    }

    public override async Task<int> Execute(Html2PdfArguments arguments, string[] context)
    {
        bool result = await _browser.Html2Pdf(arguments.InputFile.ToString(),
                                              arguments.OutputFile.ToString());

        return result ? Constants.Succes : Constants.GeneralError;
    }
}
