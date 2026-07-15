using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure;
using BookGen.Infrastructure.Terminal;

using Spectre.Console;

namespace BookGen;

[CommandName("default")]
internal sealed class DefaultCommand : Command
{
    private readonly ICommandRunnerProxy _commandRunnerProxy;

    public DefaultCommand(ICommandRunnerProxy commandRunnerProxy)
    {
        _commandRunnerProxy = commandRunnerProxy;
    }

    private Dictionary<string, List<string>> BuildFullTree()
    {
        Dictionary<string, List<string>> tree = new();

        foreach (var cmd in _commandRunnerProxy.CommandNames)
        {
            if (string.IsNullOrWhiteSpace(cmd))
            {
                continue;
            }

            string[] parts = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                if (tree.TryGetValue(parts[0], out List<string>? value))
                {
                    value.Add(parts[1]);
                }
                else
                {
                    tree[parts[0]] = [parts[1]];
                }
            }
            else if (parts.Length == 1)
            {
                if (!tree.ContainsKey(parts[0]))
                {
                    tree[parts[0]] = new List<string>();
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
        Dictionary<string, List<string>> tree = BuildFullTree();

        string text = Embedded.ReadEmbeddedResource("BookGen.Resources.DefaultCommand.md");

        AnsiConsole.MarkupLine(text);
        Terminal.Tree("BookGen", tree);
        return ExitCodes.Success;
    }

}
