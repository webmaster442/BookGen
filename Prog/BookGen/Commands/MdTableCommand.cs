//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.DomainServices.Markdown;
using BookGen.Native;

namespace BookGen.Commands;

[CommandName("mdtable")]
internal class MdTableCommand : Command<MdTableArguments>
{
    private readonly ILog _log;

    public MdTableCommand(ILog log)
    {
        _log = log;
    }

    public override int Execute(MdTableArguments arguments, string[] context)
    {
        IClipboard clipboard = NativeFactory.GetPlatformClipboard();

        string? content = clipboard.GetText();
        if (string.IsNullOrEmpty(content))
        {
            _log.Warning("Clipboard doesn't contain string data");
            return Constants.GeneralError;
        }

        if (MarkdownTableConverter.TryConvertToMarkdownTable(content, arguments.Delimiter, out string markdown))
        {
            _log.Info("Table formatted & copied to clipboard");
            clipboard.SetText(markdown);
            return Constants.Succes;
        }
        else
        {
            _log.Warning("Table format failed");
            return Constants.GeneralError;
        }
    }
}
