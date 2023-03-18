using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("shell")]
internal class ShellCommand : Command
{
    private readonly IModuleApi _api;

    public ShellCommand(IModuleApi api)
    {
        _api = api;
    }

    public override int Execute(string[] context)
    {
        foreach (string? item in DoComplete(context))
        {
            Console.WriteLine(item);
        }
        return Constants.Succes;
    }

    internal IEnumerable<string> DoComplete(string[] args)
    {
        if (args.Length == 0)
            return _api.GetCommandNames();

        string request = args[0] ?? "";

        if (request.StartsWith(Constants.ProgramName, StringComparison.OrdinalIgnoreCase))
        {
            request = request[Constants.ProgramName.Length..];
        }
        string[] words = request.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (words.Length > 0)
        {
            var command = _api.GetCommandNames().FirstOrDefault(cmd => cmd.StartsWith(words[0], StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(command))
            {
                if (words.Length > 1)
                {
                    IEnumerable<string>? candidate = _api.GetAutoCompleteItems(command).Where(arg => arg.StartsWith(words.Last(), StringComparison.OrdinalIgnoreCase));

                    if (candidate.Any())
                        return candidate;
                    else
                        return ProgramConfigurator.GeneralArguments.Where(c => c.StartsWith(words.Last(), StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    return new string[] { command };
                }
            }
        }

        return new string[] { Constants.ProgramName };
    }
}
