//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;

using BookGen.Cli.ArgumentParsing;
using BookGen.Cli.Internals;
using BookGen.Cli.OpenCli;
using BookGen.Cli.OpenCli.Draft;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BookGen.Cli;

public sealed class CommandRunner
{
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly CommandTree _commands;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICommandHelpProvider _helpProvider;
    private readonly ILogger _log;
    private readonly CommandRunnerSettings _settings;
    private readonly List<GlobalOptionParser> _globalOptionParsers;
    private readonly SupportedOs _currentOs;
    private string? _defaultCommandName;

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

    private ICommand CreateCommand(string commandName)
    {
        Type commandType = _commands.GetCommandByName(commandName);

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

        var instance = Activator.CreateInstance(commandType, constructorParameters.ToArray())
            ?? throw new InvalidOperationException();

        return (ICommand)instance;
    }

    public CommandRunner(IServiceProvider serviceProvider,
                         ICommandHelpProvider helpProvider,
                         ILogger log,
                         CommandRunnerSettings settings)
    {
        _serializerOptions = new JsonSerializerOptions
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            WriteIndented = true
        };
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
        if (!_commands.ContainsCommand(name))
        {
            AddCommand<TCommand>();
        }
        _defaultCommandName = name;
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

    public CommandRunner AddCommandsFrom(Assembly assembly)
    {
        IEnumerable<Type> commands = assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ICommand)))
            .Where(t => !t.IsAbstract && !t.IsInterface);

        foreach (Type? command in commands)
        {
            string name = command.GetCommandName();
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
        if (string.IsNullOrEmpty(_defaultCommandName))
            throw new InvalidOperationException("Default command hasn't been set");

        IEnumerable<(Type Value, Type?)> commands = _commands.CommandTypes.Select(x => (x, x.GetArgumentType()));

        return OpenCliDraftGenerator.GenerateOpenCli(_settings.ProgramMetaData.AppName,
                                                     _settings.ProgramMetaData.Version,
                                                     _commands.GetCommand(_defaultCommandName),
                                                     _globalOptionParsers,
                                                     commands,
                                                     _commands.BranchCommandNames);
    }

    public async Task<int> Run(IReadOnlyList<string> args)
    {
        _helpProvider.CommandsChanged(GenerateOpenCliDocs());
        try
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

            HashSet<string> parsedGlobals = new();
            foreach (var parser in _globalOptionParsers)
            {
                if (parser.TryParseGlobalOption(args.ToArray(), out string? globalOption))
                {
                    parsedGlobals.Add(globalOption);
                }
            }

            List<string> argsToParse = Helpers.GetArgsToParse(args, parsedGlobals);

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

    private async Task<ArgumentJsonItem[]> LoadFromJsonFile(string jsonFile)
    {
        await using FileStream stream = File.OpenRead(jsonFile);
        return await JsonSerializer.DeserializeAsync<ArgumentJsonItem[]>(stream, _serializerOptions)
            ?? throw new InvalidOperationException("Failed to load arguments from json");
    }

    public async Task<int> RunCommand(string commandName, IReadOnlyList<string> argsToParse)
    {
        if (!_commands.TryGetCommand(commandName, out Type? value))
        {
            _log.LogCritical(_settings.UnknownCommandCodeAndMessage.message + " {cmdName}", commandName);
            return _settings.UnknownCommandCodeAndMessage.code;
        }

        Type? argumentType = value.GetArgumentType();
        ICommand command = CreateCommand(commandName);

        if (!command.SupportedOs.HasFlag(_currentOs))
        {
            _log.LogCritical("{commandName} is not supported on {currentOs}", commandName, _currentOs);
            return _settings.PlatformNotSupportedExitCode;
        }

        using (var tokenSource = new ConsoleCancellationTokenSource())
        {
            if (argumentType == null)
                return await command.ExecuteAsync(ArgumentsBase.Empty, argsToParse, tokenSource.Token);

            string jsonFileName = Path.ChangeExtension(commandName, ".json");

            string argsJson = Path.Combine(Environment.CurrentDirectory, jsonFileName);

            if (argsToParse.Count < 1
                && File.Exists(argsJson))
            {
                _log.LogInformation("Loading arguments from {filename}...", jsonFileName);
                ArgumentJsonItem[] items = await LoadFromJsonFile(argsJson);

                return await ExecuteMultiple(items, argumentType, command, commandName, tokenSource.Token);
            }
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

    private async Task<int> ExecuteMultiple(ArgumentJsonItem[] items,
                                            Type argumentType,
                                            ICommand command,
                                            string commandName,
                                            CancellationToken token)
    {
        foreach (ArgumentJsonItem item in items)
        {
            _log.LogInformation("Executing {name} from json file...", item.Name);
            int exitcode = await ExecuteSingle(item.Arguments, argumentType, command, commandName, token);
            if (exitcode != 0)
            {
                _log.LogCritical("Failed to execute {name}. Exit code: {exitcode}", item.Name, exitcode);
                return exitcode;
            }
        }
        return 0;
    }
}
