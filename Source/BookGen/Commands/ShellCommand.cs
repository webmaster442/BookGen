//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("shell")]
internal class ShellCommand : Command
{
    private readonly ICommandRunnerProxy _commandNameProider;

    private const string ProgramName = "BookGen";

    public ShellCommand(ICommandRunnerProxy commandNameProvider)
    {
        _commandNameProider = commandNameProvider;
    }

    public override int Execute(string[] context)
    {
        foreach (string? item in DoComplete(context))
        {
            AnsiConsole.WriteLine(item);
        }
        return ExitCodes.Succes;
    }

    internal IEnumerable<string> DoComplete(string[] args)
    {
        if (args.Length == 0)
            return _commandNameProider.CommandNames;

        string request = args[0] ?? "";

        if (request.StartsWith(ProgramName, StringComparison.OrdinalIgnoreCase))
        {
            request = request[ProgramName.Length..];
        }
        string[] words = request.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (words.Length > 0)
        {
            var commands = _commandNameProider
                .CommandNames
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

                string[] items = _commandNameProider.GetAutoCompleteItems(commands[0]);

                if (words.Length <= 1)
                    return items;

                var candidate = items.Where(arg => arg.StartsWith(words.Last(), StringComparison.OrdinalIgnoreCase));

                if (candidate.Any())
                    return candidate;
                else
                    return ProgramConfigurator.GeneralArguments.Where(c => c.StartsWith(words.Last(), StringComparison.OrdinalIgnoreCase));

            }
        }

        return new string[] { ProgramName };
    }
}
