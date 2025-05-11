//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;

namespace BookGen.Commands;

[CommandName("html2png")]
internal sealed class Html2PngCommand : AsyncCommand<Html2PngArguments>
{
    private readonly BrowserInteract _browser;

    public override SupportedOs SupportedOs => SupportedOs.Windows;

    public Html2PngCommand(ILogger log)
    {
        _browser = new BrowserInteract(log);
    }

    public override async Task<int> Execute(Html2PngArguments arguments, string[] context)
    {
        bool result = await _browser.Html2Png(arguments.InputFile.ToString(),
                                              arguments.OutputFile.ToString(),
                                              arguments.Width,
                                              arguments.Height);

        return result ? Constants.Succes : Constants.GeneralError;
    }
}