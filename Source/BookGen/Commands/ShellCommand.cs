//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Buffers;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("shell")]
[Description("Autocompleter command, that is used by Powershell.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class ShellCommand : Command
{
    private readonly ICommandRunnerProxy _commandRunner;

    private const string ProgramName = "BookGen";
    private readonly string[] _commandNames;
    private readonly StringComparison _comparison;

    public ShellCommand(ICommandRunnerProxy runnerProxy)
    {
        _commandRunner = runnerProxy;
        _commandNames = runnerProxy.CommandNames.Select(x => $"{ProgramName} {x}").ToArray();
        _comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
    }

    internal bool TryGetCommandName(string input, [NotNullWhen(true)] out string? commandName)
    {
        commandName = null;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        foreach (string candidate in _commandNames.OrderByDescending(x => x.Length))
        {
            if (input.Equals(candidate, _comparison)
                || input.StartsWith(candidate + " ", _comparison ))
            {
                commandName = candidate.Substring(ProgramName.Length).Trim();
                return true;
            }
        }

        return false;
    }

    public override int Execute(IReadOnlyList<string> context)
    {
        if (context.Count == 2
            && int.TryParse(context[0], out int index)
            && !string.IsNullOrEmpty(context[1]))
        {
            IEnumerable<string> candidates = ShellAutoCompleteFilter.DoFilter(_commandNames, context[1], index, _comparison);

            if (!candidates.Any() 
                && TryGetCommandName(context[1], out string? commandName))
            {
                candidates = _commandRunner.GetAutoCompleteItems(commandName);
            }

            //var json = System.Text.Json.JsonSerializer.Serialize(new
            //{
            //    i = index,
            //    commandName = commandName ?? "no command name",
            //    context = context,
            //    candidates = candidates.ToArray(),
            //},
            //new System.Text.Json.JsonSerializerOptions
            //{
            //    WriteIndented = true,
            //});
            //File.WriteAllText($"complete-{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.json", json);

            foreach (var candidate in candidates)
            {
                AnsiConsole.WriteLine(candidate);
            }
        }

        return 0;
    }
}
