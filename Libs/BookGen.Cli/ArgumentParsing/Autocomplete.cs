﻿using BookGen.Cli.Annotations;
using System.Reflection;

namespace BookGen.Cli.ArgumentParsing
{
    internal static class Autocomplete
    {
        public static IEnumerable<string> GetInfo(Type t)
        {
            var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var sw = property.GetCustomAttribute<SwitchAttribute>();
                if (sw != null)
                {
                    yield return $"-{sw.ShortName}";
                    yield return $"--{sw.LongName}";
                }
            }


        }
    }
}
