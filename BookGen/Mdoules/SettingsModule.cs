//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookGen.Mdoules
{
    internal class SettingsModule : ModuleBase
    {
        private readonly AppSetting _settings;
        private readonly Dictionary<string, Type> _knownsettings;

        public SettingsModule(ProgramState currentState, AppSetting settings) : base(currentState)
        {
            _settings = settings;
            _knownsettings = FillKnownSettings();
        }

        private Dictionary<string, Type> FillKnownSettings()
        {
            return _settings.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(x => x.Name, x => x.PropertyType);
        }

        public override string ModuleCommand => "Settings";

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            var items = tokenizedArguments.GetValues().ToList();

            if (items.Count < 2) return false;

            if (string.Equals(items[1], "list", StringComparison.OrdinalIgnoreCase))
            {
                ListAvailableSettings();
                return true;
            }
            else if (string.Equals(items[1], "get", StringComparison.OrdinalIgnoreCase)
                     && items.Count == 3)
            {
                GetSetting(items[2]);
                return true;
            }
            else if (string.Equals(items[1], "set", StringComparison.OrdinalIgnoreCase)
                     && items.Count == 4)
            {
                SetSetting(items[2], items[3]);
                return true;
            }

            return false;
        }

        private void ListAvailableSettings()
        {
            Console.WriteLine("Available settings:");
            Console.WriteLine("{0,-20} - {1}", "Name", "Type");
            Console.WriteLine("-------------------");
            foreach (var item in _knownsettings)
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
                var value = prop?.GetValue(_settings)?.ToString() ?? "<null>";
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
                    var changed = Convert.ChangeType(value, prop.PropertyType);
                    prop?.SetValue(_settings, changed);
                }
                catch (Exception)
                {
                    Console.WriteLine("Can't convert {0} to type {1}", value, prop.PropertyType);
                }
                AppSettingHandler.SaveAppSettings(_settings);
            }
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(SettingsModule));
        }
    }
}
