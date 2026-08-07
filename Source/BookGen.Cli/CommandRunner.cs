//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using BookGen.Cli.ArgumentParsing;
using BookGen.Cli.Internals;
using BookGen.Cli.OpenCli;
using BookGen.Cli.OpenCli.Draft;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BookGen.Cli;

public sealed class CommandRunner
{
    private readonly CommandTree _commands;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICommandHelpProvider _helpProvider;
    private readonly ILogger _log;
    private readonly CommandRunnerSettings _settings;
    private readonly List<GlobalOptionParser> _globalOptionParsers;
    private readonly SupportedOs _currentOs;
    private readonly Dictionary<string, ICommand> _cachedCommands;

    public IValidationContext ValidationContext { get; set; }

    public Func<ArgumentsBase, IReadOnlyList<string>, Task>? BeforeRunHook { get; set; }


    private void DefaultExceptionHandler(Exception obj)
        => _log.LogCritical(obj, obj.Message);

    private bool TryProvideInternal(Type parameterType, string commandName, [NotNullWhen(true)] out object? instance)
    {
        if (parameterType == typeof(BranchItemsProvider))
        {
            instance = new BranchItemsProvider()
            {
                BranchName = commandName,
                BranchItems = _commands.CommandNames.Where(c => c.StartsWith(commandName)).ToList()
            };
            return true;
        }

        if (parameterType == typeof(ICommandHelpProvider))
        {
            instance = _helpProvider;
            return true;
        }

        instance = null;
        return false;
    }

    private ICommand GetOrCreateCommand(Type commandType, string commandName)
    {
        if (_cachedCommands.ContainsKey(commandName))
        {
            return _cachedCommands[commandName];
        }

        ConstructorInfo constructor = commandType
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .OrderByDescending(c => c.GetParameters().Length)
            .First();

        List<object> constructorParameters = new();
        foreach (ParameterInfo param in constructor.GetParameters())
        {
            FromKeyedServicesAttribute? keyAttribute = param.GetCustomAttribute<FromKeyedServicesAttribute>();

            if (TryProvideInternal(param.ParameterType, commandName, out object? dependency))
            {
                constructorParameters.Add(dependency);
                continue;
            }

            object parameterInstance = keyAttribute != null
                ? _serviceProvider.GetRequiredKeyedService(param.ParameterType, keyAttribute.Key)
                : _serviceProvider.GetRequiredService(param.ParameterType);

            constructorParameters.Add(parameterInstance);
        }

        ICommand instance = Activator.CreateInstance(commandType, constructorParameters.ToArray()) as ICommand
            ?? throw new InvalidOperationException();

        _cachedCommands.TryAdd(commandName, instance);

        return instance;
    }

    public CommandRunner(IServiceProvider serviceProvider,
                         ICommandHelpProvider helpProvider,
                         ILogger log,
                         CommandRunnerSettings settings)
    {
        _cachedCommands = new Dictionary<string, ICommand>();
        _globalOptionParsers = new List<GlobalOptionParser>();
        _commands = new CommandTree();
        _serviceProvider = serviceProvider;
        _helpProvider = helpProvider;
        _log = log;
        _settings = settings;
        ExceptionHandlerDelegate = DefaultExceptionHandler;
        _currentOs = Helpers.GetCurrentOs();

        ValidationContext = new IoCValidationContext(serviceProvider);

        Helpers.ConfigureUtfSupport(_settings.EnableUtf8Output);
    }

    public Action<Exception> ExceptionHandlerDelegate { get; set; }

    public CommandRunner AddCommand<TCommand>() where TCommand : ICommand
    {
        string name = typeof(TCommand).GetCommandName();
        _commands.Add(name, typeof(TCommand));
        return this;
    }

    public CommandRunner AddDefaultCommand<TCommand>() where TCommand : ICommand
    {
        string name = typeof(TCommand).GetCommandName();

        if (_commands.IsDefaultCommandSet)
            throw new InvalidOperationException("Default command has already been set");

        _commands.AddDefault(typeof(TCommand));
        return this;
    }

    public CommandRunner AddGlobalOptionParser(GlobalOptionParser parser)
    {
        _globalOptionParsers.Add(parser);
        return this;
    }

    public CommandRunner AddGlobalOptionParser<TParser>() where TParser : GlobalOptionParser, new()
    {
        _globalOptionParsers.Add(new TParser());
        return this;
    }

    public IEnumerable<string> GetGlobalOptions()
    {
        foreach (GlobalOptionParser parser in _globalOptionParsers)
        {
            yield return parser.ShortName;
            yield return parser.LongName;
        }
    }

    public CommandRunner AddCommandsFrom(Assembly assembly, bool includeDefault)
    {
        IEnumerable<Type> commands = assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ICommand)))
            .Where(t => !t.IsAbstract && !t.IsInterface);

        foreach (Type? command in commands)
        {
            string name = command.GetCommandName();

            if (_commands.GetDefaultCommandName() == name && !includeDefault)
                continue;

            if (!_commands.ContainsCommand(name))
            {
                _commands.Add(name.ToLower(), command);
            }
        }

        return this;
    }

    public IEnumerable<string> CommandNames
        => _commands.CommandNames;

    public string[] GetAutoCompleteItems(string commandName)
    {
        if (_commands.TryGetCommand(commandName, out Type? value))
        {
            Type type = value;

            Type? args = type.GetArgumentType();

            if (args != null)
            {
                return Autocomplete.GetInfo(args).Order().ToArray();
            }
        }
        return Array.Empty<string>();
    }

    public Document GenerateOpenCliDocs()
    {
        if (!_commands.IsDefaultCommandSet)
            throw new InvalidOperationException("Default command hasn't been set");

        IEnumerable<(Type Value, Type?)> commands = _commands.CommandTypes.Select(x => (x, x.GetArgumentType()));

        return OpenCliDraftGenerator.GenerateOpenCli(_settings.ProgramMetaData.AppName,
                                                     _settings.ProgramMetaData.Version,
                                                     _commands.GetDefaultCommand(),
                                                     _globalOptionParsers,
                                                     commands,
                                                     _commands.BranchCommandNames);
    }

    public async Task<int> Run(IReadOnlyList<string> args)
    {
        _helpProvider.CommandsChanged(GenerateOpenCliDocs());
        int skipCount = 1;
        try
        {
            string commandName;
            if (args.Count > 0)
            {
                if (args[0].StartsWith('-'))
                {
                    commandName = _commands.GetDefaultCommandName();
                    skipCount = 0;
                }
                else
                {
                    commandName = args[0].ToLower();
                }
            }
            else
            {
                if (!_commands.IsDefaultCommandSet)
                    throw new InvalidOperationException("Default command hasn't been setup");

                commandName = _commands.GetDefaultCommandName();
                skipCount = 0;
            }

            HashSet<string> parsedGlobals = new();
            foreach (var parser in _globalOptionParsers)
            {
                if (parser.TryParseGlobalOption(args, out string? globalOption))
                {
                    parsedGlobals.Add(globalOption);
                }
            }

            List<string> argsToParse = Helpers.GetArgsToParse(args, parsedGlobals, skipCount);

            return await RunCommand(commandName, argsToParse);
        }
        catch (Exception ex)
        {
#if DEBUG

            Debugger.Break();
#endif
            ExceptionHandlerDelegate.Invoke(ex);
            return _settings.ExcptionExitCode;
        }
    }

    public async Task<int> RunCommand(string commandName, IReadOnlyList<string> argsToParse)
    {
        Type? commandType;
        if (!_commands.TryGetDefaultCommand(commandName, out commandType))
        {
            if (!_commands.TryGetCommand(commandName, out commandType))
            {
                _log.LogCritical(_settings.UnknownCommandCodeAndMessage.message + " {cmdName}", commandName);
                return _settings.UnknownCommandCodeAndMessage.code;
            }
        }

        Type? argumentType = commandType.GetArgumentType();
        ICommand command = GetOrCreateCommand(commandType, commandName);

        if (!command.SupportedOs.HasFlag(_currentOs))
        {
            _log.LogCritical("{commandName} is not supported on {currentOs}", commandName, _currentOs);
            return _settings.PlatformNotSupportedExitCode;
        }

        using (var tokenSource = new ConsoleCancellationTokenSource())
        {
            if (argumentType == null)
                return await command.ExecuteAsync(ArgumentsBase.Empty, argsToParse, tokenSource.Token);

            return await ExecuteSingle(argsToParse, argumentType, command, commandName, tokenSource.Token);
        }
    }

    private async Task<int> ExecuteSingle(IReadOnlyList<string> argsToParse,
                                          Type argumentType,
                                          ICommand command,
                                          string commandName,
                                          CancellationToken token)
    {
        ArgumentsBase args = ArgumentsBase.Empty;
        ArgumentParser parser = new(argumentType, _log);
        args = parser.Fill(argsToParse);

        ValidationResult validationResult = args.Validate(ValidationContext);

        if (!validationResult.IsOk)
        {
            _log.LogCritical(validationResult.ToString());

            if (_settings.PrintHelpOnBadArgs)
            {
                string help = _helpProvider.GetHelp(commandName);
                _log.LogInformation("Command help:\r\n{help}", help);
            }
            else
            {
                _log.LogInformation("Use help {commandName} to get help on command", commandName);
            }
            return _settings.BadParametersExitCode;
        }

        args.ModifyAfterValidation();

        if (BeforeRunHook != null)
            await BeforeRunHook.Invoke(args, argsToParse);

        return await command.ExecuteAsync(args, argsToParse, token);
    }
}
