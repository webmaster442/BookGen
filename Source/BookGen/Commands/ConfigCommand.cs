using System.Reflection;

using Bookgen.Lib.AppSettings;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Terminal;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("config")]
internal class ConfigCommand : Command
{
    private readonly ILogger _logger;

    public ConfigCommand(ILogger logger)
    {
        _logger = logger;
    }

    public override int Execute(IReadOnlyList<string> context)
    {
        BookGenAppSettings appSettings = new();

        var data = typeof(BookGenAppSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (context.Count == 0)
            return DisplayConfig(data, appSettings);

        if (context.Count == 1)
            return DisplaySpecific(data, appSettings, context[0]);

        appSettings.Set(context[1], context[0]);
        appSettings.Save();

        return ExitCodes.Succes;
    }

    private int DisplaySpecific(PropertyInfo[] data, BookGenAppSettings appSettings, string property)
    {
        string[] headers = ["Name", "Type", "Value"];
        var tableData = data.Where(x => x.Name == property).ToArray();

        if (tableData.Length != 0)
            return DisplayConfig(tableData, appSettings);

        _logger.LogError("Setting not found: {setting}", property);
        return ExitCodes.ArgumentsError;
    }

    private static int DisplayConfig(PropertyInfo[] data, BookGenAppSettings appSettings)
    {
        string[] headers = ["Name", "Type", "Value"];
        var tableData = data.Select(x => new string[] 
        {
            x.Name,
            x.PropertyType.ToString(), 
            x.GetValue(appSettings)?.ToString() ?? "<null>"
        });

        Terminal.Table(headers, tableData);

        return ExitCodes.Succes;
    }
}
