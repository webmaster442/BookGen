//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Resources;

namespace BookGen.Commands;

[CommandName("new")]
internal class NewCommand : Command<NewArguments>
{
    private readonly NewFiles _newFiles;

    public NewCommand()
    {
        _newFiles = new NewFiles();
    }

    public override int Execute(NewArguments arguments, string[] context)
    {
        if (!arguments.TemplateSelected)
        {
            Console.Write(_newFiles.GetHelp());
            return Constants.Succes;
        }

        if (_newFiles.TryGetFile(arguments.TemplateName, out string content))
        {
            File.WriteAllText(arguments.Output, content);
            return Constants.Succes;
        }
        else
        {
            Console.WriteLine($"Template {arguments.TemplateName} not found.");
            return Constants.GeneralError;
        }
    }
}
