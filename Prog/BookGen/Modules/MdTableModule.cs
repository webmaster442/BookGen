//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Utilities;

namespace BookGen.Modules
{
    internal class MdTableModule : ModuleWithState
    {
        public MdTableModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "MdTable";

        public override AutoCompleteItem AutoCompleteInfo => new AutoCompleteItem(ModuleCommand, "-d", "--delimiter");

        public override SupportedOs SupportedOs => SupportedOs.Windows;

        public override ModuleRunResult Execute(string[] arguments)
        {
            var args = new MdTableArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            var content = WinClipboard.GetText();
            if (string.IsNullOrEmpty(content))
            {
                CurrentState.Log.Warning("Clipboard doesn't contain string data");
                return ModuleRunResult.GeneralError;
            }

            if (MarkdownTableConverter.TryConvertToMarkdownTable(content, args.Delimiter, out string markdown))
            {
                CurrentState.Log.Info("Table formatted & copied to clipboard");
                WinClipboard.SetText(markdown);
                return ModuleRunResult.Succes;
            }
            else
            {
                CurrentState.Log.Warning("Table format failed");
                return ModuleRunResult.GeneralError;
            }
        }
    }
}
