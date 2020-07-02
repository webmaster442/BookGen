//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace BookGen.Ui.ArgumentParser
{
    public static class ArgumentParser
    {
        private static readonly Dictionary<SwitchAttribute, PropertyInfo> _properties = new Dictionary<SwitchAttribute, PropertyInfo>();
        private static readonly List<string> _files = new List<string>();
        private static int _filled = 0;
        private static int _required = 0;

        public static bool ParseArguments<T>(string[] args, T targetClass) where T: ArgumentsBase
        {
            _properties.Clear();
            _filled = 0;
            Type tType = typeof(T);


            Inialize(tType);
            WalkArgsAndFillClass(args, ref targetClass);

            return
                _filled == _required
                && targetClass.Validate();
        }

        private static SwitchAttribute? GetSwitchAttrubute(PropertyInfo property)
        {
            return property
                .GetCustomAttributes()
                .FirstOrDefault(p => p is SwitchAttribute) as SwitchAttribute;
        }

        private static void Inialize(Type tType)
        {
            PropertyInfo[]? props = tType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in props)
            {
                SwitchAttribute? sw = GetSwitchAttrubute(property);
                if (sw != null)
                {
                    if (sw.Required) ++_required;
                    _properties.Add(sw, property);
                }
            }
        }

        private static string ParseSwitch(string current)
        {
            if (current.StartsWith("--"))
                return current.Substring(2);
            else if (current.StartsWith("-"))
                return current.Substring(1);
            else
                return current;
        }

        private static void WalkArgsAndFillClass<T>(string[] args, ref T targetClass) where T : ArgumentsBase
        {
            int i = 0;
            bool nextIsswitch, currentIsSwitch;
            while (i < args.Length)
            {
                string current = args[i].ToLower();
                string next = (i + 1) < args.Length ? args[i + 1].ToLower() : string.Empty;
                nextIsswitch = next.StartsWith("-");
                currentIsSwitch = current.StartsWith("-");

                if (currentIsSwitch)
                {
                    if (nextIsswitch)
                    {
                        ++i;
                        //standalone switch
                        SetSwitch(ParseSwitch(current), ref targetClass);
                    }
                    else
                    {
                        i += 2;
                        SetSwitchWithValue(ParseSwitch(current), next, ref targetClass);
                    }
                }
                else
                {
                    ++i;
                    _files.Add(current);
                }
            }
            targetClass.Files = _files.ToArray();
        }

        private static void SetSwitchWithValue<T>(string key, string value, ref T targetClass) where T : ArgumentsBase
        {
            PropertyInfo? prop = _properties
                .Where(s => s.Key.ShortName == key || s.Key.LongName == key)
                .Select(s => s.Value)
                .FirstOrDefault();

            if (prop != null)
            {
                if (prop.PropertyType.IsEnum &&
                    Enum.TryParse(prop.PropertyType, value, out object? parsed))
                {
                    prop.SetValue(targetClass, parsed);
                    ++_filled;
                }
                else
                {
                    try
                    {
                        object converted = Convert.ChangeType(value, prop.PropertyType, CultureInfo.InvariantCulture);
                        prop.SetValue(targetClass, converted);
                        ++_filled;
                    }
                    catch (Exception ex) 
                        when (ex is InvalidCastException
                             || ex is FormatException
                             || ex is OverflowException
                             || ex is ArgumentException)
                    {
                        --_filled;
                    }
                }

            }
            else
            {
                --_filled;
            }
        }

        private static void SetSwitch<T>(string key, ref T targetClass) where T : ArgumentsBase
        {
            PropertyInfo? prop = _properties
                .Where(s => s.Key.ShortName == key || s.Key.LongName == key)
                .Select(s => s.Value)
                .FirstOrDefault();

            if (prop != null && prop.PropertyType == typeof(bool))
            {
                prop.SetValue(targetClass, true);
                ++_filled;
            }
            else
            {
                --_filled;
            }
        }
    }
}
