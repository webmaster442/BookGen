//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;

using BookGen.Cli.Annotations;
using BookGen.Cli.ArgumentParsing;
using BookGen.Cli.OpenCli;
using BookGen.Cli.OpenCli.Draft;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BookGen.Cli;

public sealed class CommandRunner
{
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly Dictionary<string, Type> _commands;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICommandHelpProvider _helpProvider;
    private readonly ILogger _log;
    private readonly CommandRunnerSettings _settings;
    private readonly List<GlobalOptionParser> _globalOptionParsers;
    private readonly SupportedOs _currentOs;
    private string? _defaultCommandName;

    private static string GetCommandName(Type t)
    {
        CommandNameAttribute? nameAttribure = t.GetCustomAttribute<CommandNameAttribute>();
        return nameAttribure?.Name
            ?? throw new InvalidOperationException($"Command {t.FullName} is missing a {nameof(CommandNameAttribute)}");
    }

    public IValidationContext ValidationContext { get; set; }
    public Func<ArgumentsBase, IReadOnlyList<string>, Task>? BeforeRunHook { get; set; }

    private static SupportedOs GetCurrentOs()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return SupportedOs.Windows;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return SupportedOs.Linux;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return SupportedOs.OsX;
        else
            return SupportedOs.None;
    }

    private static Type? GetArgumentType(Type cmd)
    {
        Type originalType = cmd;

        // Walk up the inheritance hierarchy to find Command<T> or AsyncCommand<T>
        while (cmd != null && cmd != typeof(object))
        {
            if (cmd.IsGenericType)
            {
                Type baseGeneric = cmd.GetGenericTypeDefinition();

                if (baseGeneric == typeof(Command<>) || baseGeneric == typeof(AsyncCommand<>))
                {
                    // Get the concrete TArguments
                    Type tArguments = cmd.GetGenericArguments()[0];
#if DEBUG
                    Debug.WriteLine($"Via generics: {tArguments.FullName}");
#endif
                    return tArguments;
                }
            }
            if (cmd.BaseType == null)
            {
                break;
            }
            cmd = cmd.BaseType;
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

    private void DefaultExceptionHandler(Exception obj)
    {
        _log.LogCritical(obj, obj.Message);
    }

    private ICommand CreateCommand(string commandName)
    {
        ConstructorInfo constructor = _commands[commandName]
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .OrderByDescending(c => c.GetParameters().Length)
            .First();

        List<object> constructorParameters = new();
        foreach (ParameterInfo param in constructor.GetParameters())
        {
            FromKeyedServicesAttribute? keyAttribute = param.GetCustomAttribute<FromKeyedServicesAttribute>();

            object parameterInstance = keyAttribute != null
                ? _serviceProvider.GetRequiredKeyedService(param.ParameterType, keyAttribute.Key)
                : _serviceProvider.GetRequiredService(param.ParameterType);

            constructorParameters.Add(parameterInstance);
        }

        var instance = Activator.CreateInstance(_commands[commandName], constructorParameters.ToArray())
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
        _commands = new Dictionary<string, Type>();
        _serviceProvider = serviceProvider;
        _helpProvider = helpProvider;
        _log = log;
        _settings = settings;
        ExceptionHandlerDelegate = DefaultExceptionHandler;
        _currentOs = GetCurrentOs();

        ValidationContext = new IoCValidationContext(serviceProvider);

        ConfigureUtfSupport(_settings.EnableUtf8Output);
    }

    private static void ConfigureUtfSupport(bool enableUtf8Output)
    {
        if (enableUtf8Output)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
        }
    }

    // Skip the first argument (command name) and any parsed global options
    private static List<string> GetArgsToParse(IReadOnlyList<string> args, HashSet<string> parsedGlobals)
    {
        List<string> results = new();
        for (int i = 1; i < args.Count; i++)
        {
            if (!parsedGlobals.Contains(args[i]))
            {
                results.Add(args[i]);
            }
        }
        return results;
    }


    public Action<Exception> ExceptionHandlerDelegate { get; set; }

    public CommandRunner AddCommand<TCommand>() where TCommand : ICommand
    {
        string name = GetCommandName(typeof(TCommand));
        _commands.Add(name.ToLower(), typeof(TCommand));
        return this;
    }

    public CommandRunner AddDefaultCommand<TCommand>() where TCommand : ICommand
    {
        string name = GetCommandName(typeof(TCommand));
        if (!_commands.ContainsKey(name))
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
            string name = GetCommandName(command);
            if (!_commands.ContainsKey(name))
            {
                _commands.Add(name.ToLower(), command);
            }
        }

        return this;
    }

    public IEnumerable<string> CommandNames
        => _commands.Keys;

    public string[] GetAutoCompleteItems(string commandName)
    {
        if (_commands.TryGetValue(commandName, out Type? value))
        {
            Type type = value;

            Type? args = GetArgumentType(type);

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

        IEnumerable<(Type Value, Type?)> commands = _commands.Select(x => (x.Value, GetArgumentType(x.Value)));

        return OpenCliDraftGenerator.GenerateOpenCli(_settings.ProgramMetaData.AppName,
                                                     _settings.ProgramMetaData.Version,
                                                     _commands[_defaultCommandName],
                                                     _globalOptionParsers,
                                                     commands);
    }

    public async Task<int> Run(IReadOnlyList<string> args)
    {
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

            List<string> argsToParse = GetArgsToParse(args, parsedGlobals);

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
        if (!_commands.TryGetValue(commandName, out Type? value))
        {
            _log.LogCritical(_settings.UnknownCommandCodeAndMessage.message);
            return _settings.UnknownCommandCodeAndMessage.code;
        }

        Type? argumentType = GetArgumentType(value);
        ICommand command = CreateCommand(commandName);

        if (!command.SupportedOs.HasFlag(_currentOs))
        {
            _log.LogCritical("{commandName} is not supported on {currentOs}", commandName, _currentOs);
            return _settings.PlatformNotSupportedExitCode;
        }

        if (argumentType == null)
            return await command.ExecuteAsync(ArgumentsBase.Empty, argsToParse);

        string jsonFileName = Path.ChangeExtension(commandName, ".json");

        string argsJson = Path.Combine(Environment.CurrentDirectory, jsonFileName);

        if (argsToParse.Count < 1
            && File.Exists(argsJson))
        {
            _log.LogInformation("Loading arguments from {filename}...", jsonFileName);
            ArgumentJsonItem[] items = await LoadFromJsonFile(argsJson);

            return await ExecuteMultiple(items, argumentType, command, commandName);
        }
        return await ExecuteSingle(argsToParse, argumentType, command, commandName);
    }

    private async Task<int> ExecuteSingle(IReadOnlyList<string> argsToParse,
                                          Type argumentType,
                                          ICommand command,
                                          string commandName)
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
                string help = _helpProvider.GetHelp(commandName, argumentType);
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

        return await command.ExecuteAsync(args, argsToParse);
    }

    private async Task<int> ExecuteMultiple(ArgumentJsonItem[] items,
                                            Type argumentType,
                                            ICommand command,
                                            string commandName)
    {
        foreach (ArgumentJsonItem item in items)
        {
            _log.LogInformation("Executing {name} from json file...", item.Name);
            int exitcode = await ExecuteSingle(item.Arguments, argumentType, command, commandName);
            if (exitcode != 0)
            {
                _log.LogCritical("Failed to execute {name}. Exit code: {exitcode}", item.Name, exitcode);
                return exitcode;
            }
        }
        return 0;
    }
}
