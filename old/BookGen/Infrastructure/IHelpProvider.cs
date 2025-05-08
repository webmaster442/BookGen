namespace BookGen.Infrastructure;

internal interface IHelpProvider
{
    IEnumerable<string> HelpEntries { get; }
    IEnumerable<string> GetCommandHelp(string cmd);
    void RegisterCallback(string commandName, Func<string> callback);
}