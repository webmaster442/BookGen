//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

using BookGen.Cli.Annotations;

using Microsoft.Extensions.Logging;

namespace BookGen.Cli.ArgumentParsing;

internal class ArgumentParser
{
    private readonly List<PropertyInfo> _argumentPropertyInfos;
    private readonly List<PropertyInfo> _switchPropertyInfos;

    private readonly Type _argumentType;
    private readonly ILogger _log;

    public ArgumentParser(Type argumentType, ILogger log)
    {
        _argumentPropertyInfos = new List<PropertyInfo>();
        _switchPropertyInfos = new List<PropertyInfo>();
        OrganizeProperties(argumentType.GetProperties(BindingFlags.Public | BindingFlags.Instance));
        _argumentType = argumentType;
        _log = log;
    }

    private void OrganizeProperties(PropertyInfo[] propertyInfos)
    {
        foreach (var propertyInfo in propertyInfos)
        {
            var switchAttribute = propertyInfo.GetCustomAttribute<SwitchAttribute>();
            var argumentAttribute = propertyInfo.GetCustomAttribute<ArgumentAttribute>();

            if (switchAttribute != null && argumentAttribute != null)
            {
                throw new InvalidOperationException($"Invalid annotation found in type {_argumentType.FullName} on propery {propertyInfo.Name}. Both Switch and Argument attributes are present");
            }
           
            if (switchAttribute != null)
            {
                _switchPropertyInfos.Add(propertyInfo);
            }
            else if (argumentAttribute != null)
            {
                _argumentPropertyInfos.Add(propertyInfo);
            }
        }
    }

    public ArgumentsBase Fill(IReadOnlyList<string> args)
    {
        var argumentsClass = Activator.CreateInstance(_argumentType)
            ?? throw new InvalidOperationException();

        var argBag = new ArgumentBag(args);

        HandleSwitchProperties(argumentsClass, argBag);
        HandleArgumentProperties(argumentsClass, argBag);

        var notProcessesd = string.Join(' ', argBag.GetNotProcessed());
        if (!string.IsNullOrEmpty(notProcessesd))
            _log.LogWarning("Not processed arguments: {notprocessed}", notProcessesd);

        return (ArgumentsBase)argumentsClass;
    }

    private void HandleSwitchProperties(object argumentsClass, ArgumentBag argBag)
    {
        foreach (var property in _switchPropertyInfos)
        {
            var switchAttribute = property.GetCustomAttribute<SwitchAttribute>()
                ?? throw new System.Diagnostics.UnreachableException();
            
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
    }

    private void HandleArgumentProperties(object argumentsClass, ArgumentBag argBag)
    {
        foreach (var property in _argumentPropertyInfos)
        {
            var argumentAttribute = property.GetCustomAttribute<ArgumentAttribute>()
                ?? throw new System.Diagnostics.UnreachableException();

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
}
