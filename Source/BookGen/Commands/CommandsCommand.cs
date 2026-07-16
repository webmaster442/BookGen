using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("commands")]
[Description("Lists all available commands in a hierarchical structure")]
[ExitCode(ExitCodes.Success, "The command executed successfully.")]
internal sealed class CommandsCommand(ICommandRunnerProxy commandRunnerProxy) : Command
{
    private Dictionary<string, List<string>> BuildFullTree()
    {
        Dictionary<string, List<string>> tree = new();

        foreach (var cmd in commandRunnerProxy.CommandNames)
        {
            if (string.IsNullOrWhiteSpace(cmd))
            {
                continue;
            }

            string[] parts = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                string toAdd = parts[1];
                if (tree.TryGetValue(parts[0], out List<string>? value))
                {
                    value.Add(toAdd);
                }
                else
                {
                    tree[parts[0]] = [toAdd];
                }
            }
            else if (parts.Length == 1)
            {
                if (!tree.ContainsKey(parts[0]))
                {
                    string toAdd = parts[0];
                    tree[toAdd] = new List<string>();
                }
            }
            else
            {
                throw new InvalidOperationException("Too many nestings");
            }
        }
        return tree;
    }

    public override int Execute(IReadOnlyList<string> context)
    {
        AnsiConsole.MarkupLine("[green]Available commands: [/]");
        AnsiConsole.WriteLine();
        Dictionary<string, List<string>> treeData = BuildFullTree();
        Dictionary<string, string?> commandDescriptions = commandRunnerProxy.GetOpenCliDocs().Commands?
            .DistinctBy(x => x.Name)
            .ToDictionary(x => x.Name, x => x.Description) ?? new Dictionary<string, string?>();

        var tree = new Spectre.Console.Tree("BookGen");
        foreach (var item in treeData)
        {
            string nodeName = $"[bold green]{item.Key.EscapeMarkup()}[/]\r\n[italic]{commandDescriptions.GetValueOrDefault(item.Key)?.EscapeMarkup()}[/]";
            TreeNode node = tree.AddNode(nodeName);
            if (item.Value.Count > 0)
            {
                foreach (var subItem in item.Value)
                {
                    string subNodeName = $"[bold green]{subItem.EscapeMarkup()}[/]\r\n[italic]{commandDescriptions.GetValueOrDefault($"{item.Key} {subItem}")?.EscapeMarkup()}[/]";
                    node.AddNode(subNodeName);
                }
            }
        }
        AnsiConsole.Write(tree);

        return ExitCodes.Success;
    }
}
