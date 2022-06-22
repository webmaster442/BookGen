//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using WpLoad.Commands;
using WpLoad.Domain;
using WpLoad.Infrastructure;

namespace WpLoad
{
    public static class Program
    {
        public static async Task<int> Main(string[] arguments)
        {
            var log = new ConsoleLog();
            try
            {
                IEnumerable<Type>? commandTypes = Assembly
                    .GetAssembly(typeof(ICommandBase))
                    ?.GetTypes()
                    .Where(x => x.IsAssignableTo(typeof(ICommandBase))
                    && !x.IsInterface)
                    ?? Enumerable.Empty<Type>();

                IEnumerable<ICommandBase?>? commands = commandTypes.Select(x => Activator.CreateInstance(x) as ICommandBase);

                if (arguments.Length > 0
                    && TryParseSubCommand(arguments, commands, out ICommandBase? cmd, out string[] parameters))
                {
                    if (cmd is IAsyncCommand asyncCommand)
                    {
                        ExitCode code = await asyncCommand.Execute(log, parameters);
                        return PrintHelpIfNeededAndExit(log, code);
                    }
                    else if (cmd is ICommand command)
                    {
                        ExitCode code = command.Execute(log, parameters);
                        return PrintHelpIfNeededAndExit(log, code);
                    }
                    else
                        throw new InvalidOperationException($"Don't know what to do with: {cmd.GetType()}");

                }
                else
                {
                    return PrintHelpIfNeededAndExit(log, ExitCode.BadParameters);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return (int)ExitCode.Crash;
            }
        }

        private static int PrintHelpIfNeededAndExit(ConsoleLog log, ExitCode code)
        {
            if (code == ExitCode.BadParameters)
                new Help().Execute(log, Array.Empty<string>());
            return (int)code;
        }

        private static bool TryParseSubCommand(string[] arguments,
                                               IEnumerable<ICommandBase?> commands,
                                               [NotNullWhen(true)] out ICommandBase? cmd,
                                               out string[] parameters)
        {
            string name = arguments[0];
            cmd = commands
                .Where(x => string.Compare(x?.CommandName, name, true) == 0)
                .FirstOrDefault();

            parameters = arguments.Skip(1).ToArray();
            return cmd != null;
        }
    }
}