using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("subcommands")]
internal class SubCommandsCommand : Command
{
    private readonly IEnumerable<string> _commands;

    public SubCommandsCommand(IModuleApi api)
    {
        _commands = api.GetCommandNames();
    }

    public override int Execute(string[] context)
    {
        Console.WriteLine("Available sub commands: \r\n");
        foreach (var command in _commands)
        {
            Console.WriteLine(command);
        }
        return Constants.Succes;
    }
}
