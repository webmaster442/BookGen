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
        _commandNames = runnerProxy.CommandNames.Select(x => $"{ProgramName} {x}").Order().ToArray();
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
                commandName = candidate[ProgramName.Length..].Trim();
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
            IEnumerable<string> candidates = ShellAutoCompleteFilter.FilterCommandNames(_commandNames, context[1], index, _comparison);

            if (!candidates.Any()
                && TryGetCommandName(context[1], out string? commandName))
            {
                IOrderedEnumerable<string> items = _commandRunner.GetAutoCompleteItems(commandName).Order();
                candidates = ShellAutoCompleteFilter.FilterSwitchesAndArgs(items, context[1], index, _comparison);
            }

            //var json = System.Text.Json.JsonSerializer.Serialize(new
            //{
            //    i = index,
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
#pragma warning disable Spectre1000
                // Tests use console redirect to capture output, so
                // we need to use Console.WriteLine here instead of AnsiConsole.WriteLine
                Console.WriteLine(candidate);
#pragma warning restore Spectre1000
            }
        }

        return 0;
    }
}
