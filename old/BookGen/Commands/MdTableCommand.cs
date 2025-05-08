//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.DomainServices.Markdown;

namespace BookGen.Commands;

[CommandName("mdtable")]
internal class MdTableCommand : Command<MdTableArguments>
{
    private readonly ILogger _log;

    public MdTableCommand(ILogger log)
    {
        _log = log;
    }

    public override int Execute(MdTableArguments arguments, string[] context)
    {
        string? content = TextCopy.ClipboardService.GetText();
        if (string.IsNullOrEmpty(content))
        {
            _log.LogWarning("Clipboard doesn't contain string data");
            return Constants.GeneralError;
        }

        if (MarkdownTableConverter.TryConvertToMarkdownTable(content, arguments.Delimiter, out string markdown))
        {
            _log.LogInformation("Table formatted & copied to clipboard");
            TextCopy.ClipboardService.SetText(markdown);
            return Constants.Succes;
        }
        else
        {
            _log.LogWarning("Table format failed");
            return Constants.GeneralError;
        }
    }
}
