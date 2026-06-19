//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Confighandling;

using BookGen.Cli;
using BookGen.Cli.Annotations;

using Microsoft.Extensions.Logging;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("config")]
internal sealed class ConfigCommand : Command<ConfigCommand.ConfigCommandSettings>
{
    public class ConfigCommandSettings : ArgumentsBase
    {
        [Argument(0, IsOptional = true)]
        public string Setting { get; set; } = string.Empty;

        [Argument(1, IsOptional = true)]
        public string Value { get; set; } = string.Empty;
    }

    private readonly ILogger _logger;
    private readonly IAppSettingsAccessor _appSettings;

    public ConfigCommand(ILogger logger, IAppSettingsAccessor appSettings)
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
