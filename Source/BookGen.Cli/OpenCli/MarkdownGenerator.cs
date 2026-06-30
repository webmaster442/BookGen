using System.Text;

using BookGen.Cli.OpenCli.Draft;

namespace BookGen.Cli.OpenCli;

public static class MarkdownGenerator
{
    public static string GenerateMarkdown(this Document document)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# {document.Info.Title}");
        sb.AppendLine();
        sb.AppendLine(document.Info.Description);
        sb.AppendLine();
        if (document.Commands != null)
        {
            foreach (Draft.Command command in document.Commands)
            {
                sb.AppendLine($"## {command.Name}");
                sb.AppendLine();
                sb.AppendLine(command.Description);
                sb.AppendLine();
                if (command.Options?.Count > 0)
                {
                    sb.AppendLine("### Options");
                    sb.AppendLine();
                    foreach (var option in command.Options)
                    {
                        sb.AppendLine($"- **{option.Name}**: {option.Description}");
                    }
                    sb.AppendLine();
                }
            }
        }
        return sb.ToString();
    }
}
