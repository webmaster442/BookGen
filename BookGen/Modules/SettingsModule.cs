//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Domain.Shell;
using BookGen.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookGen.Modules
{
    internal class SettingsModule : ModuleWithState
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

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "get",
                                            "list",
                                            "set");
            }
        }

        public override bool Execute(string[] arguments)
        {
            if (arguments.Length < 2) return false;

            if (string.Equals(arguments[1], "list", StringComparison.OrdinalIgnoreCase))
            {
                ListAvailableSettings();
                return true;
            }
            else if (string.Equals(arguments[1], "get", StringComparison.OrdinalIgnoreCase)
                     && arguments.Length == 3)
            {
                GetSetting(arguments[2]);
                return true;
            }
            else if (string.Equals(arguments[1], "set", StringComparison.OrdinalIgnoreCase)
                     && arguments.Length == 4)
            {
                SetSetting(arguments[2], arguments[3]);
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
    }
}
