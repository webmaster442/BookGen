using BookGen.Cli.Annotations;
using System.Reflection;

namespace BookGen.Cli.ArgumentParsing
{
    internal class ArgumentParser
    {
        private PropertyInfo[] _properities;
        private readonly Type _argumentType;

        public ArgumentParser(Type argumentType)
        {
            _properities = argumentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _argumentType = argumentType;
        }

        private static string? GetSwitchValue(string[] args, SwitchAttribute @switch)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == $"-{@switch.ShortName}"
                    || args[i] == $"--{@switch.LongName}")
                {
                    int nextIndex = i + 1;
                    if (nextIndex < args.Length)
                    {
                        return args[nextIndex];
                    }
                }
            }
            return null;
        }

        private static bool GetSwitch(string[] args, SwitchAttribute @switch)
        {
            return args.Contains($"-{@switch.ShortName}")
                || args.Contains($"--{@switch.LongName}");
        }

        public ArgumentsBase Fill(string[] args)
        {
            var aguments = Activator.CreateInstance(_argumentType)
                ?? throw new InvalidOperationException();

            foreach (var property in _properities)
            {
                var switchParams = property.GetCustomAttribute<SwitchAttribute>();
                if (switchParams == null)
                {
                    continue;
                }

                if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(aguments, GetSwitch(args, switchParams));
                }
                else
                {
                    var value = ValueConverter.Convert(GetSwitchValue(args, switchParams), property.PropertyType);
                    property.SetValue(aguments, value);
                }

            }

            return (ArgumentsBase)aguments;
        }
    }
}
