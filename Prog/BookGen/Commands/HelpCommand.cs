using BookGen.Gui;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("help")]
internal class HelpCommand : Command
{
    private readonly IHelpProvider _helpProvider;
    private readonly IModuleApi _api;

    public HelpCommand(IHelpProvider helpProvider, IModuleApi api)
    {
        _helpProvider = helpProvider;
        _api = api;
    }

    public override int Execute(string[] context)
    {
        if (context.Length == 0) 
        {
            HelpRenderer.RenderHelp(_helpProvider.GetCommandHelp("help"));
            return Constants.Succes;
        }

        return Constants.Succes;

    }
}
