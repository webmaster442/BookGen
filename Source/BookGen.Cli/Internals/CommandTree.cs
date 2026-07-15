//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace BookGen.Cli.Internals;

internal sealed class CommandTree
{
    private readonly Dictionary<string, Type> _commands;

    public CommandTree()
    {
        _commands = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
    }

    private bool TryGetBranchNames(string name, out string[] branches)
    {
        string[] parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
        {
            branches = Array.Empty<string>();
            return false;
        }
        var items = new List<string>();
        for (int i = 1; i < parts.Length; i++)
        {
            string branch = string.Join(' ', parts, 0, i);
            items.Add(branch);
        }
        branches = [.. items];
        return branches.Length > 0;
    }

    public Type GetCommandByName(string name)
    {
        return _commands.TryGetValue(name, out Type? commandType)
            ? commandType
            : throw new KeyNotFoundException($"Command '{name}' not found.");
    }

    public void Add(string name, Type type)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Command name cannot be null or whitespace.", nameof(name));
        }
        if (TryGetBranchNames(name, out string[] branches))
        {
            foreach (var branch in branches)
            {
                if (!_commands.ContainsKey(branch))
                {
                    _commands[branch] = typeof(BranchCommand);
                }
            }
        }
        _commands[name] = type;
    }

    public bool ContainsCommand(string name)
        => _commands.ContainsKey(name);

    public IEnumerable<string> CommandNames
        => _commands.Keys;

    public IEnumerable<Type> CommandTypes
        => _commands.Values;

    public bool TryGetCommand(string name, [NotNullWhen(true)] out Type? commandType)
        => _commands.TryGetValue(name, out commandType);

    public Type GetCommand(string name)
        => _commands[name];
}
