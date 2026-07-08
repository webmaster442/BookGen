//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using BookGen.Cli.OpenCli.Draft;

namespace BookGen.Cli.OpenCli;

public static class MarkdownGenerator
{
    public static string GenerateMarkdown(Draft.Command command, int level = 1)
    {
        static string LevelPrefix(int level) => new('#', level);

        StringBuilder result = new();
        result
            .AppendLine($"{LevelPrefix(level)} {command.Name}")
            .AppendLine()
            .AppendLine(command.Description)
            .AppendLine();

        if (command.Examples?.Count > 0)
        {
            result.AppendLine("```sh");
            foreach (string example in command.Examples)
            {
                result.AppendLine(example);
            }
            result
                .AppendLine("```")
                .AppendLine();
        }

        if (command.Arguments?.Count > 0)
        {
            result
                .AppendLine($"{LevelPrefix(level + 1)} Arguments")
                .AppendLine();

            foreach (Argument argument in command.Arguments.OrderBy(a => a.OpenClRequired).ThenBy(a => a.Name))
            {
                result
                    .AppendLine($"* `{argument.Name}`")
                    .AppendLine(RequiredOrNot(argument.OpenClRequired, "argument"))
                    .AppendLine($"  {argument.Description}");
            }

            result.AppendLine();
        }

        if (command.Options?.Count > 0)
        {
            result
                .AppendLine($"{LevelPrefix(level + 1)} Options")
                .AppendLine();

            foreach (Option option in command.Options.OrderBy(o => o.OpenClRequired).ThenBy(o => o.Name))
            {
                result
                    .AppendLine($"* -`{option.Name}`, `--{string.Join(' ', option.Aliases ?? new List<string>())}`")
                    .AppendLine()
                    .Append("  ")
                    .AppendLine(RequiredOrNot(option.OpenClRequired, "option"))
                    .AppendLine()
                    .Append("  ")
                    .AppendLine(option.Description)
                    .AppendLine();
            }

            result.AppendLine();
        }

        result
            .AppendLine($"{LevelPrefix(level + 1)} Exit codes")
            .AppendLine();

        foreach (ExitCode exitCode in command.ExitCodes ?? new List<ExitCode>())
        {
            result
                .AppendLine($"* `{exitCode.Code}` - {exitCode.Description}");
        }

        result.AppendLine();

        return result.ToString();
    }

    private static string? RequiredOrNot(bool? required, string type)
    {
        if (required == null)
        {
            return null;
        }
        return required == true ? $"**Required {type}**" : $"**Optional {type}**";
    }
}
