//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

using BookGen.Cli.Annotations;

namespace BookGen.Cli.ArgumentParsing;

internal class ArgumentParser
{
    private readonly PropertyInfo[] _properities;
    private readonly Type _argumentType;

    public ArgumentParser(Type argumentType)
    {
        _properities = argumentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        _argumentType = argumentType;
    }

    public ArgumentsBase Fill(IReadOnlyList<string> args)
    {
        var arguments = Activator.CreateInstance(_argumentType)
            ?? throw new InvalidOperationException();

        var argBag = new ArgumentBag(args);

        foreach (var property in _properities)
        {
            var switchAttribute = property.GetCustomAttribute<SwitchAttribute>();
            var argumentAttribute = property.GetCustomAttribute<ArgumentAttribute>();

            if (switchAttribute != null && argumentAttribute != null)
                throw new InvalidOperationException($"Invalid annotation found in type {_argumentType.FullName} on propery {property.Name}. Both Switch and Argument attributes are present");

            if (switchAttribute != null)
            {
                if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(arguments, argBag.GetSwitch(switchAttribute));
                }
                else
                {
                    var value = ValueConverter.Convert(argBag.GetSwitchValue(switchAttribute), property.PropertyType);
                    if (value != null)
                    {
                        property.SetValue(arguments, value);
                    }
                }
            }
            else if (argumentAttribute != null)
            {
                var argumentValue = argBag.GetArgument(argumentAttribute);

                if (argumentValue == null)
                {
                    if (!argumentAttribute.IsOptional)
                        throw new InvalidOperationException("Missing Arguments");

                    continue;
                }

                var value = ValueConverter.Convert(argumentValue, property.PropertyType);
                property.SetValue(arguments, value);
            }

        }

        return (ArgumentsBase)arguments;
    }
}
