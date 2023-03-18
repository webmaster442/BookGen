//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

namespace BookGen.Commands;

[CommandName("settings")]
internal class SettingsCommand : Command
{
    private readonly Dictionary<string, Type> _knownsettings;
    private readonly AppSetting _settings;
    private readonly ILog _log;

    private Dictionary<string, Type> FillKnownSettings()
    {
        return _settings.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(x => x.Name, x => x.PropertyType);
    }

    public SettingsCommand(AppSetting settings, ILog log)
    {
        _settings = settings;
        _log = log;
        _knownsettings = FillKnownSettings();
    }

    public override int Execute(string[] context)
    {
        if (context.Length < 1) return Constants.ArgumentsError;

        if (string.Equals(context[0], "list", StringComparison.OrdinalIgnoreCase))
        {
            ListAvailableSettings();
            return Constants.Succes;
        }
        else if (string.Equals(context[0], "get", StringComparison.OrdinalIgnoreCase)
                 && context.Length == 2)
        {
            GetSetting(context[1]);
            return Constants.Succes;
        }
        else if (string.Equals(context[0], "set", StringComparison.OrdinalIgnoreCase)
                 && context.Length == 3)
        {
            SetSetting(context[1], context[2]);
            return Constants.Succes;
        }

        return Constants.GeneralError;
    }

    private void ListAvailableSettings()
    {
        Console.WriteLine("Available settings:");
        Console.WriteLine("{0,-20} - {1}", "Name", "Type");
        Console.WriteLine("-------------------");
        foreach (KeyValuePair<string, Type> item in _knownsettings)
        {
            Console.WriteLine("{0,-20} - {1}", item.Key, item.Value.Name);
        }
    }

    private PropertyInfo? GetProperty(string setting)
    {
        KeyValuePair<string, Type> item = _knownsettings.FirstOrDefault(x => x.Key.Equals(setting, StringComparison.OrdinalIgnoreCase));

        if (item.Key == null
            || item.Value == null)
        {
            Console.WriteLine("Setting not found: {0}", setting);
            return null;
        }

        return _settings.GetType().GetProperty(item.Key);
    }

    private void GetSetting(string setting)
    {
        PropertyInfo? prop = GetProperty(setting);
        if (prop != null)
        {
            string? value = prop.GetValue(_settings)?.ToString() ?? "<null>";
            Console.WriteLine(value);
        }
    }

    private void SetSetting(string setting, string value)
    {
        PropertyInfo? prop = GetProperty(setting);
        if (prop != null)
        {
            try
            {
                object? changed = Convert.ChangeType(value, prop.PropertyType);
                prop.SetValue(_settings, changed);
            }
            catch (Exception)
            {
                _log.Warning("Can't convert {0} to type {1}", value, prop.PropertyType);
            }
            AppSettingHandler.SaveAppSettings(_settings);
        }
    }
}