using BookGen.Api;
using BookGen.Cli.Annotations;
using BookGen.Cli.ArgumentParsing;
using System.Reflection;

namespace BookGen.Cli
{
    public class CommandRunner
    {
        private readonly Dictionary<string, Type> _commands;
        private readonly IResolver _resolver;
        private readonly ILog _log;
        private readonly CommandRunnerSettings _settings;

        private class EmptyArgs : ArgumentsBase { }

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
        private void DefaultExceptionHandler(Exception obj)
        {
            _log.Critical(obj);
        }

        private ICommand CreateCommand(string commandName)
        {
            var constructor = _commands[commandName]
                .GetConstructors(BindingFlags.Public)
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

        public CommandRunner(IResolver resolver, ILog log, CommandRunnerSettings settings)
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

        public async Task<int> Run(IReadOnlyList<string> args)
        {
            var commandName = args[0].ToLower();
            var argsToParse = args.Skip(1).ToArray();

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

                return await command.Execute(parser.Fill(args), argsToParse);
            }
            catch (Exception ex)
            {
                ExceptionHandlerDelegate.Invoke(ex);
                return -1;
            }
        }
    }
}
