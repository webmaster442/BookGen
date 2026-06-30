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
                                           IEnumerable<(Type commandType, Type? argumentType)> commandTypes)
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
            Commands = GenerateCommands(commandTypes),
        };
    }

    private static List<Draft.Command> GenerateCommands(IEnumerable<(Type commandType, Type? argumentType)> commandTypes)
    {
        List<Draft.Command> result = new();
        foreach (var commandType in commandTypes)
        {
            result.Add(new Draft.Command
            {
                Name = GetCommandName(commandType.commandType),
                Description = GetDescription(commandType.commandType),
                ExitCodes = GetExitCodes(commandType.commandType),
                Arguments = GetArguments(commandType.argumentType),
                Options = GetOptions(commandType.argumentType),
            });
        }
        return result;
    }

    private static List<Option>? GetGlobalOptions(IEnumerable<GlobalOptionParser> globalOptionParsers)
    {
        List<Option> result = new List<Option>();
        foreach (var globalOptionParser in globalOptionParsers)
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
                    OpenClRequired = @switch.Required
                });
            }
        }
        
        return options.Count > 0 ? options : null;
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
                    OpenClRequired = !argument.IsOptional
                });
            }
        }

        return arguments.Count > 0 ? arguments : null;
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
