//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Reflection;

using BookGen.Cli.Annotations;
using BookGen.Cli.OpenCli.Draft;

namespace BookGen.Cli.OpenCli;

internal static class OpenCliDraftGenerator
{
    public static Document GenerateOpenCli(string appName,
                                           Version version,
                                           Type defaultCommand,
                                           IEnumerable<GlobalOptionParser> globalOptionParsers,
                                           IEnumerable<(Type commandType, Type? argumentType)> commandTypes,
                                           IEnumerable<string> branchCommands)
    {
        return new Document
        {
            Command = new Draft.Command
            {
                Name = appName,
                Description = GetDescription(defaultCommand),
                ExitCodes = GetExitCodes(defaultCommand),
                Options = GetGlobalOptions(globalOptionParsers),
            },
            Opencli = "0.1",
            Info = new CliInfo
            {
                Title = appName,
                Version = version.ToString(),
            },
            Conventions = new Conventions
            {
                GroupOptions = false,
                OptionSeparator = " ",
            },
            Commands = GenerateCommands(appName, commandTypes, branchCommands),
        };
    }

    private static List<Draft.Command> GenerateCommands(string appName, IEnumerable<(Type commandType, Type? argumentType)> commandTypes, IEnumerable<string> branchCommands)
    {
        List<Draft.Command> result = new();
        foreach ((Type commandType, Type? argumentType) commandType in commandTypes)
        {
            var name = GetCommandName(commandType.commandType);

            if (string.IsNullOrEmpty(name))
                continue;

            List<Argument>? arguments = GetArguments(commandType.argumentType);
            List<Option>? options = GetOptions(commandType.argumentType);

            result.Add(new Draft.Command
            {
                Name = name,
                Description = GetDescription(commandType.commandType),
                ExitCodes = GetExitCodes(commandType.commandType),
                Arguments = arguments,
                Options = options,
                Examples = GenerateExamples(appName, name, arguments, options),
            });
        }
        foreach (var branch in branchCommands)
        {
            result.Add(new Draft.Command
            {
                Name = branch,
                Description = $"Displays commands starting with: {branch}",
                Examples = new List<string>
                {
                    $"{appName} {branch}",
                },
                ExitCodes = new List<ExitCode>
                {
                    new ExitCode
                    {
                        Code = 0,
                        Description = "Success"
                    },
                },
            });
        }

        return result;
    }

    private static List<Option>? GetGlobalOptions(IEnumerable<GlobalOptionParser> globalOptionParsers)
    {
        var result = new List<Option>();
        foreach (GlobalOptionParser globalOptionParser in globalOptionParsers)
        {
            DescriptionAttribute? description = globalOptionParser.GetType().GetCustomAttribute<DescriptionAttribute>();
            result.Add(new Option
            {
                Name = globalOptionParser.ShortName,
                Aliases = [globalOptionParser.LongName],
                Description = description?.Description,
                OpenClRequired = false,
            });
        }
        return result.Count > 0 ? result : null;
    }

    private static List<Option>? GetOptions(Type? commandArgType)
    {
        if (commandArgType == null)
            return null;

        var options = new List<Option>();
        PropertyInfo[] properties = commandArgType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            SwitchAttribute? @switch = property.GetCustomAttribute<SwitchAttribute>();
            DescriptionAttribute? description = property.GetCustomAttribute<DescriptionAttribute>();
            if (@switch != null)
            {
                options.Add(new Option
                {
                    Name = @switch.ShortName,
                    Aliases = [@switch.LongName],
                    Description = description?.Description,
                    OpenClRequired = @switch.Required,
                    Metadata = MakeTypeMetadata(property.PropertyType),
                });
            }
        }

        return options.Count > 0 ? options : null;
    }

    private static List<Metadata> MakeTypeMetadata(Type propertyType)
    {
        return
        [
            new() {
                Name = "type",
                Value = propertyType.FullName
            }
        ];
    }

    private static List<Argument>? GetArguments(Type? commandArgType)
    {
        if (commandArgType == null)
            return null;

        var arguments = new List<Argument>();
        PropertyInfo[] properties = commandArgType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            ArgumentAttribute? argument = property.GetCustomAttribute<ArgumentAttribute>();
            DescriptionAttribute? description = property.GetCustomAttribute<DescriptionAttribute>();
            if (argument != null)
            {
                arguments.Add(new Argument
                {
                    Name = property.Name,
                    Description = description?.Description,
                    OpenClRequired = !argument.IsOptional,
                    Metadata = MakeTypeMetadata(property.PropertyType)
                });
            }
        }

        return arguments.Count > 0 ? arguments : null;
    }

    private static List<string>? GenerateExamples(string appName, string name, List<Argument>? arguments, List<Option>? options)
    {
        static string NeedsValue(List<Metadata>? metadata)
        {
            if (metadata == null)
                return string.Empty;

            Metadata? typeMetadata = metadata.FirstOrDefault(x => x.Name == "type");

            return typeMetadata?.Value?.ToString() == typeof(bool).FullName
                ? string.Empty
                : " <value>";
        }

        List<string> result = [$"{appName} {name}"];

        foreach (Argument argument in arguments?.OrderBy(x => x.OpenClRequired).ThenBy(x => x.Name) ?? Enumerable.Empty<Argument>())
        {
            string item = argument.OpenClRequired == true
                ? $"  <{argument.Name}>"
                : $"  [{argument.Name}]";

            result.Add(item);
        }

        foreach (Option option in options?.OrderBy(x => x.OpenClRequired).ThenBy(x => x.Name) ?? Enumerable.Empty<Option>())
        {
            string item = option.OpenClRequired == true
                ? $"  -{option.Name}, --{string.Join(' ', option.Aliases ?? new List<string>())}{NeedsValue(option.Metadata)}"
                : $"  [-{option.Name}, --{string.Join(' ', option.Aliases ?? new List<string>())}{NeedsValue(option.Metadata)}]";

            result.Add(item);
        }

        for (int i = 0; i < result.Count - 1; i++)
        {
            result[i] = $"{result[i]} \\";
        }

        return result.Count > 0 ? result : null;
    }

    private static string GetCommandName(Type commandType)
    {
        return commandType.GetCustomAttribute<CommandNameAttribute>()?.Name
            ?? throw new InvalidOperationException($"{commandType} doesn't have a {nameof(CommandNameAttribute)} attribute set");
    }

    private static List<ExitCode> GetExitCodes(Type commandType)
    {
        return commandType.GetCustomAttributes<ExitCodeAttribute>()
            .Select(x => new ExitCode
            {
                Code = x.ExitCode,
                Description = x.Description,
            })
            .ToList();
    }

    private static string? GetDescription(Type commandType)
        => commandType.GetCustomAttribute<DescriptionAttribute>()?.Description;
}
