using BookGen.Api;
using BookGen.Cli.Annotations;
using BookGen.Cli.ArgumentParsing;
using System.Reflection;

namespace BookGen.Cli
{
    public sealed class CommandRunner
    {
        private readonly Dictionary<string, Type> _commands;
        private readonly IResolver _resolver;
        private readonly ILog _log;
        private readonly CommandRunnerSettings _settings;
        private string? _defaultCommandName;

        private class EmptyArgs : ArgumentsBase { }

        private static string GetCommandName<TCommand>() where TCommand : ICommand
        {
            var nameAttribure = typeof(TCommand).GetCustomAttribute<CommandNameAttribute>();
            return nameAttribure?.Name
                ?? throw new InvalidOperationException($"Command is missing a {nameof(CommandNameAttribute)}");
        }
        private static Type? GetArgumentType(Type cmd)
        {
            var method = cmd.GetMethod("Execute");

            var parameter = method
                ?.GetParameters()
                .Where(p => p.ParameterType.IsAssignableTo(typeof(ArgumentsBase)))
                .FirstOrDefault()
                ?.ParameterType;

            return parameter;
        }
        private void DefaultExceptionHandler(Exception obj)
        {
            _log.Critical(obj);
        }

        private ICommand CreateCommand(string commandName)
        {
            var constructor = _commands[commandName]
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            List<object> contructorParameters = new();
            foreach (var param in constructor.GetParameters())
            {
                contructorParameters.Add(_resolver.Resolve(param.ParameterType));
            }

            var instance = Activator.CreateInstance(_commands[commandName], contructorParameters.ToArray())
                ?? throw new InvalidOperationException();

            return (ICommand)instance;
        }

        public CommandRunner(IResolver resolver,
                             ILog log,
                             CommandRunnerSettings settings)
        {
            _commands = new Dictionary<string, Type>();
            _resolver = resolver;
            _log = log;
            _settings = settings;
            ExceptionHandlerDelegate = DefaultExceptionHandler;
        }

        public Action<Exception> ExceptionHandlerDelegate { get; set; }

        public CommandRunner Add<TCommand>() where TCommand : ICommand
        {
            string name = GetCommandName<TCommand>();
            _commands.Add(name.ToLower(), typeof(TCommand));
            return this;
        }

        public CommandRunner AddDefaultCommand<TCommand>() where TCommand : ICommand
        {
            string name = GetCommandName<TCommand>();
            if (!_commands.ContainsKey(name))
            {
                Add<TCommand>();
            }
            _defaultCommandName = name;
            return this;
        }

        public IEnumerable<string> CommandNames
            => _commands.Keys;

        public string[] GetAutoCompleteItems(string commandName)
        {
            if (_commands.ContainsKey(commandName))
            {
                var type =
                    _commands[commandName]
                    .GetType();

                var genTypes = type.GetGenericArguments();

                var args = genTypes.Length > 0 ? genTypes[0] : null;

                if (args != null)
                {
                    return Autocomplete.GetInfo(args).Order().ToArray();
                }
            }
            return Array.Empty<string>();
        }

        public Task<int> Run(IReadOnlyList<string> args)
        {
            string commandName;
            if (args.Count > 0)
            {
                commandName = args[0].ToLower();
            }
            else
            {
                if (string.IsNullOrEmpty(_defaultCommandName))
                    throw new InvalidOperationException("Default command hasn't been setup");
                commandName = _defaultCommandName;
            }

            var argsToParse = args.Skip(1).ToArray();

            return RunCommand(commandName, argsToParse);
        }

        public async Task<int> RunCommand(string commandName, string[] argsToParse)
        {
            if (!_commands.ContainsKey(commandName))
            {
                _log.Critical(_settings.UnknownCommandCodeAndMessage.message);
                return _settings.UnknownCommandCodeAndMessage.code;
            }

            var argumentType = GetArgumentType(_commands[commandName]);
            ICommand command = CreateCommand(commandName);

            try
            {

                if (argumentType == null)
                    return await command.Execute(new EmptyArgs(), argsToParse);

                ArgumentParser parser = new(argumentType);

                return await command.Execute(parser.Fill(argsToParse), argsToParse);
            }
            catch (Exception ex)
            {
                ExceptionHandlerDelegate.Invoke(ex);
                return -1;
            }
        }
    }
}
