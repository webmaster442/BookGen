using System.Diagnostics;
using System.Reflection;

using BookGen.Cli.Annotations;

namespace BookGen.Cli.Internals;

internal static class Extensions
{
    public static string GetCommandName(this Type command)
    {
        CommandNameAttribute? nameAttribure = command.GetCustomAttribute<CommandNameAttribute>();
        return nameAttribure?.Name
            ?? throw new InvalidOperationException($"Command {command.FullName} is missing a {nameof(CommandNameAttribute)}");
    }


    public static Type? GetArgumentType(this Type command)
    {
        Type originalType = command;

        // Walk up the inheritance hierarchy to find Command<T> or AsyncCommand<T>
        while (command != null && command != typeof(object))
        {
            if (command.IsGenericType)
            {
                Type baseGeneric = command.GetGenericTypeDefinition();

                if (baseGeneric == typeof(Command<>) || baseGeneric == typeof(AsyncCommand<>))
                {
                    // Get the concrete TArguments
                    Type tArguments = command.GetGenericArguments()[0];
#if DEBUG
                    Debug.WriteLine($"Via generics: {tArguments.FullName}");
#endif
                    return tArguments;
                }
            }
            if (command.BaseType == null)
            {
                break;
            }
            command = command.BaseType;
        }

        MethodInfo? method = originalType.GetMethod(nameof(AsyncCommand.ExecuteAsync))
                     ?? originalType.GetMethod(nameof(Command.Execute));

        if (method == null)
            throw new InvalidOperationException($"Command {originalType.FullName} is missing Exetutable method");

        Type? parameter = method
            ?.GetParameters()
            .FirstOrDefault(p => p.ParameterType.IsAssignableTo(typeof(ArgumentsBase)))
            ?.ParameterType;

#if DEBUG
        Debug.WriteLine($"Via methodinfo: {parameter?.FullName}");
#endif

        return parameter;
    }
}
