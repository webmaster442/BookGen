//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Cli;
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
            var commands = _api
                .GetCommandNames()
                .Where(cmd => cmd.StartsWith(words[0], StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (commands.Length > 1)
            {
                return commands;
            }
            else if (!string.IsNullOrEmpty(commands[0]))
            {
                if (!string.Equals(words[0], commands[0], StringComparison.OrdinalIgnoreCase))
                    return commands;

                string[] items = _api.GetAutoCompleteItems(commands[0]);

                if (words.Length <= 1)
                    return items;

                var candidate = items.Where(arg => arg.StartsWith(words.Last(), StringComparison.OrdinalIgnoreCase));

                if (candidate.Any())
                    return candidate;
                else
                    return ProgramConfigurator.GeneralArguments.Where(c => c.StartsWith(words.Last(), StringComparison.OrdinalIgnoreCase));

            }
        }

        return new string[] { Constants.ProgramName };
    }
}
