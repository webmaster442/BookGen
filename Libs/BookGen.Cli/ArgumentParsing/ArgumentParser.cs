//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;
using System.Text.Json;

using BookGen.Api;
using BookGen.Cli.Annotations;

namespace BookGen.Cli.ArgumentParsing;

internal class ArgumentParser
{
    private readonly PropertyInfo[] _properities;
    private readonly Type _argumentType;
    private readonly ILog _log;

    public ArgumentParser(Type argumentType, ILog log)
    {
        _properities = argumentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        _argumentType = argumentType;
        _log = log;
    }

    public ArgumentsBase Fill(IReadOnlyList<string> args)
    {
        var argumentsClass = Activator.CreateInstance(_argumentType)
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
                    property.SetValue(argumentsClass, argBag.GetSwitch(switchAttribute));
                }
                else if (property.PropertyType.IsArray)
                {
                    string[] values = argBag.GetSwitchValues(switchAttribute);
                    var elementType = property.PropertyType.GetElementType();
                    if (elementType != null)
                    {
                        var finalValues = values
                            .Select(v => ValueConverter.Convert(v, elementType))
                            .ToArray();

                        var array = Array.CreateInstance(elementType, finalValues.Length);
                        Array.Copy(finalValues, array, finalValues.Length);

                        property.SetValue(argumentsClass, array);
                    }
                }
                else
                {
                    var value = ValueConverter.Convert(argBag.GetSwitchValue(switchAttribute), property.PropertyType);
                    if (value != null)
                    {
                        property.SetValue(argumentsClass, value);
                    }
                }
            }
            else if (argumentAttribute != null)
            {
                if (property.PropertyType.IsArray)
                    throw new InvalidOperationException("Array properties are not supported for arguments");

                var argumentValue = argBag.GetArgument(argumentAttribute);

                if (argumentValue == null)
                {
                    if (!argumentAttribute.IsOptional)
                        throw new InvalidOperationException("Missing Arguments");

                    continue;
                }

                var value = ValueConverter.Convert(argumentValue, property.PropertyType);
                property.SetValue(argumentsClass, value);
            }

        }

        var notProcessesd = string.Join(' ', argBag.GetNotProcessed());
        if (!string.IsNullOrEmpty(notProcessesd))
            _log.Warning("Not processed arguments: {0}", notProcessesd);

        return (ArgumentsBase)argumentsClass;
    }

    internal ArgumentsBase LoadArgsFromJson(string argsJson)
    {
        KeyValuePair<string, string>[] arguments = JsonSerializer.Deserialize<KeyValuePair<string, string>[]>(File.ReadAllText(argsJson))
            ?? throw new InvalidOperationException($"Failed to deserialize {argsJson}");

        List<string> toParse = new();
        List<string> values = new();
        foreach (var argument in arguments)
        {
            if (string.IsNullOrEmpty(argument.Key))
            {
                values.Add(argument.Value);
            }
            else
            {
                toParse.Add(argument.Key);
                if (!string.IsNullOrEmpty(argument.Value))
                    values.Add(argument.Value);
            }
        }

        toParse.AddRange(values);

        return Fill(toParse);
    }
}
