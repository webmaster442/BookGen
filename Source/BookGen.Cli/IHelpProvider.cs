namespace BookGen.Cli;

public interface ICommandHelpProvider
{
    string GetHelp(string commandName, Type argumentType);
}
