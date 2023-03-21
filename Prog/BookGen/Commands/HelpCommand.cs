using BookGen.Gui;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("help")]
internal class HelpCommand : Command
{
    private readonly IHelpProvider _helpProvider;
    private readonly HashSet<string> _commandNames;

    public HelpCommand(IHelpProvider helpProvider, IModuleApi api)
    {
        _helpProvider = helpProvider;
        _commandNames = api.GetCommandNames().ToHashSet();
    }

    public override int Execute(string[] context)
    {
        if (context.Length == 0) 
        {
            HelpRenderer.RenderHelp(_helpProvider.GetCommandHelp("help"));
            return Constants.Succes;
        }

        string command = context[0].ToLower();
        if (!_commandNames.Contains(command))
        {
            Console.WriteLine("Unknown Command: {0}", command);
            return Constants.GeneralError;
        }

        HelpRenderer.RenderHelp(_helpProvider.GetCommandHelp(command));
        return Constants.Succes;

    }
}
