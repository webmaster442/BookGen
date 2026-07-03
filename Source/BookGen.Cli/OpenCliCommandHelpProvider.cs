using System.Text;

using BookGen.Cli.OpenCli.Draft;

namespace BookGen.Cli;

public class OpenCliCommandHelpProvider : ICommandHelpProvider
{
    private Document _document;

    public OpenCliCommandHelpProvider()
    {
        _document = new Document
        {
            Info = new CliInfo
            {
                Title = string.Empty,
                Version = string.Empty,
            },
            Command = new OpenCli.Draft.Command
            {
                Name = string.Empty,
            },
            Opencli = "0.1"
        };
    }

    public void CommandsChanged(Document openCliDocument)
    {
        _document = openCliDocument;
    }

    public virtual string GetHelp(string commandName)
    {
        var command = _document.Commands?.FirstOrDefault(c => c.Name == commandName);
        if (command == null)
            return $"** No Help found for: {commandName} **";

        StringBuilder result = new();
        result
            .AppendLine($"# {command.Name}")
            .AppendLine()
            .AppendLine(command.Description);

        if (command.Arguments?.Count > 0)
        {
            result
                .AppendLine("## Arguments")
                .AppendLine();

            foreach (var argument in command.Arguments)
            {
                result
                    .AppendLine($"* {argument.Name}")
                    .AppendLine($"  {argument.Description}");
            }
        }

        if (command.Options?.Count > 0)
        {
            result
                .AppendLine("## Options")
                .AppendLine();

            foreach (var option in command.Options)
            {
                result
                    .AppendLine($"* -`{option.Name}`, `--{string.Join(' ', option.Aliases ?? new List<string>())}`")
                    .AppendLine($"  {option.Description}");
            }
        }

        return result.ToString();
    }
}

