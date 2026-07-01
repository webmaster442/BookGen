//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using Bookgen.Lib.AppSettings;

using BookGen.Cli;
using BookGen.Cli.Annotations;

using Microsoft.Extensions.Logging;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("config")]
[Description("Get or set the application settings. Without any arguments, the command will display the current settings.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
[ExitCode(ExitCodes.GeneralError, "An error occurred while executing the command.")]
internal sealed class ConfigCommand : Command<ConfigCommand.ConfigCommandSettings>
{
    public class ConfigCommandSettings : ArgumentsBase
    {
        [Argument(0, IsOptional = true)]
        [Description("The setting to be configured.")]
        public string Setting { get; set; } = string.Empty;

        [Argument(1, IsOptional = true)]
        [Description("The value to set for the specified setting.")]
        public string Value { get; set; } = string.Empty;
    }

    private readonly ILogger _logger;
    private readonly IAppSettings _appSettings;

    public ConfigCommand(ILogger logger, IAppSettings appSettings)
    {
        _logger = logger;
        _appSettings = appSettings;
    }

    public override int Execute(ConfigCommandSettings arguments, IReadOnlyList<string> context)
    {
        if (string.IsNullOrEmpty(arguments.Setting)
            && string.IsNullOrEmpty(arguments.Value))
        {
            PrintCurrentSettings();
            return ExitCodes.Success;
        }

        if (!_appSettings.KnownSettings.Any(x => x.setting == arguments.Setting))
        {
            _logger.LogError("Unknown setting: {Setting}", arguments.Setting);
            return ExitCodes.GeneralError;
        }

        _appSettings.Set(arguments.Setting, arguments.Value);

        if (!_appSettings.IsSettingValid(arguments.Setting, out IReadOnlyList<string> issues))
        {
            foreach (var issue in issues)
            {
                _logger.LogError(issue);
            }
            return ExitCodes.GeneralError;
        }

        _appSettings.Save();
        return ExitCodes.Success;
    }

    private void PrintCurrentSettings()
    {
        var table = new Table();
        table.AddColumns("Setting", "Type", "Is Valid?", "Value");
        foreach ((string setting, Type type) setting in _appSettings.KnownSettings)
        {
            object? value = _appSettings.Get(setting.setting);
            bool isValid = _appSettings.IsSettingValid(setting.setting, out _);
            table.AddRow(setting.setting, setting.type.Name, isValid ? "Yes" : "No", value?.ToString() ?? "null");
        }
        AnsiConsole.Write(table);
    }
}
