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

        public ArgumentsBase Fill(string[] args)
        {
            var arguments = Activator.CreateInstance(_argumentType)
                ?? throw new InvalidOperationException();

            var argBag = new ArgumentBag(args);

            foreach (var property in _properities)
            {
                var switchParams = property.GetCustomAttribute<SwitchAttribute>();
                var argParams = property.GetCustomAttribute<ArgumentAttribute>();

                if (switchParams != null && argParams != null)
                    throw new InvalidOperationException($"Invalid annotation found in type {_argumentType.FullName} on propery {property.Name}. Both Switch and Argument attributes are present");

                if (switchParams != null)
                {
                    if (property.PropertyType == typeof(bool))
                    {
                        property.SetValue(arguments, argBag.GetSwitch(switchParams));
                    }
                    else
                    {
                        var value = ValueConverter.Convert(argBag.GetSwitchValue(switchParams), property.PropertyType);
                        property.SetValue(arguments, value);
                    }
                }
                else if (argParams != null)
                {
                    var value = ValueConverter.Convert(argBag.GetArgument(argParams), property.PropertyType);
                    property.SetValue(arguments, value);
                }
                else
                {
                    continue;
                }

            }

            return (ArgumentsBase)arguments;
        }
    }
}
