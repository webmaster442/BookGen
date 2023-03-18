using BookGen.Cli;
using BookGen.CommandArguments;
using BookGen.DomainServices.Markdown;

namespace BookGen.Commands
{
    internal class MdTableCommand : Command<MdTableArguments>
    {
        private readonly ILog _log;

        public MdTableCommand(ILog log)
        {
            _log = log;
        }

        public override int Execute(MdTableArguments arguments, string[] context)
        {
            string? content = WinClipboard.GetText();
            if (string.IsNullOrEmpty(content))
            {
                _log.Warning("Clipboard doesn't contain string data");
                return Constants.GeneralError;
            }

            if (MarkdownTableConverter.TryConvertToMarkdownTable(content, arguments.Delimiter, out string markdown))
            {
                _log.Info("Table formatted & copied to clipboard");
                WinClipboard.SetText(markdown);
                return Constants.Succes;
            }
            else
            {
                _log.Warning("Table format failed");
                return Constants.GeneralError;
            }
        }
    }
}
