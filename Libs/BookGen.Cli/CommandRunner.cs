using BookGen.Api;
using BookGen.Cli.Annotations;
using System.Reflection;

namespace BookGen.Cli
{
    public class CommandRunnerSettings
    {
        public (int code, string message) UnknownCommandCodeAndMessage { get; init; }

        public CommandRunnerSettings()
        {
            UnknownCommandCodeAndMessage = (-1, "Unknown command");
        }
    }

    public class CommandRunner
    {
        private readonly Dictionary<string, Type> _commands;
        private readonly IResolver _resolver;
        private readonly ILog _log;
        private readonly CommandRunnerSettings _settings;

        private static string GetCommandName<TCommand>() where TCommand : ICommand
        {
            var nameAttribure = typeof(TCommand).GetCustomAttribute<CommandNameAttribute>();
            return nameAttribure?.Name
                ?? throw new InvalidOperationException($"Command is missing a {nameof(CommandNameAttribute)}");
        }
        private static Type? GetArgumentType(Type cmd)
        {
            var arguments = cmd.GetGenericArguments();
            if (arguments.Length > 0) return arguments[0];
            return null;
        }

        public CommandRunner(IResolver resolver, ILog log, CommandRunnerSettings settings)
        {
            _commands = new Dictionary<string, Type>();
            _resolver = resolver;
            _log = log;
            _settings = settings;
        }

        public CommandRunner Add<TCommand>() where TCommand : ICommand
        {
            string name = GetCommandName<TCommand>();
            _commands.Add(name.ToLower(), typeof(TCommand));
            return this;
        }

        public async Task<int> Run(string[] args)
        {
            var commandName = args[0].ToLower();
            var argsToParse = args.Skip(1).ToArray();

            if (!_commands.ContainsKey(commandName))
            {
                _log.Critical(_settings.UnknownCommandCodeAndMessage.message);
                return _settings.UnknownCommandCodeAndMessage.code;
            }

            var ArgumentType = GetArgumentType(_commands[commandName]);
            ICommand command = CreateCommand(commandName);

            if (ArgumentType == null)
                return await command.Execute(null, argsToParse);


        }

        private ICommand CreateCommand(string commandName)
        {
            
        }
    }
}
