//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;

using BookGen.Cli.Annotations;
using BookGen.Cli.ArgumentParsing;

using Microsoft.Extensions.Logging;

namespace BookGen.Cli;

public sealed class CommandRunner
{
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly Dictionary<string, Type> _commands;
    private readonly IResolver _resolver;
    private readonly ILogger _log;
    private readonly CommandRunnerSettings _settings;
    private readonly SupportedOs _currentOs;
    private string? _defaultCommandName;

    private static string GetCommandName(Type t)
    {
        var nameAttribure = t.GetCustomAttribute<CommandNameAttribute>();
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
        var method = cmd.GetMethod(nameof(AsyncCommand.ExecuteAsync))
                     ?? cmd.GetMethod(nameof(Command.Execute));

        if (method == null)
            throw new InvalidOperationException($"Command {cmd.FullName} is missing Exetutable method");

        var parameter = method
            ?.GetParameters()
            .FirstOrDefault(p => p.ParameterType.IsAssignableTo(typeof(ArgumentsBase)))
            ?.ParameterType;

        return parameter;
    }

    private void DefaultExceptionHandler(Exception obj)
    {
        _log.LogCritical(obj, obj.Message);
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
                         ILogger log,
                         CommandRunnerSettings settings)
    {
        _serializerOptions = new JsonSerializerOptions
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            WriteIndented = true
        };
        _commands = new Dictionary<string, Type>();
        _resolver = resolver;
        _log = log;
        _settings = settings;
        ExceptionHandlerDelegate = DefaultExceptionHandler;
        _currentOs = GetCurrentOs();

        ValidationContext = new IoCValidationContext(_resolver);

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

    public CommandRunner AddCommandsFrom(Assembly assembly)
    {
        var commands = assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ICommand)))
            .Where(t => !t.IsAbstract && !t.IsInterface);

        foreach (var command in commands)
        {
            var name = GetCommandName(command);
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
            var type = value;

            var args = GetArgumentType(type);

            if (args != null)
            {
                return Autocomplete.GetInfo(args).Order().ToArray();
            }
        }
        return Array.Empty<string>();
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

            var argsToParse = args.Skip(1).ToArray();

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
        await using var stream = File.OpenRead(jsonFile);
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

        var argumentType = GetArgumentType(value);
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
            var items = await LoadFromJsonFile(argsJson);

            return await ExecuteMultiple(items, argumentType, command);
        }
        return await ExecuteSingle(argsToParse, argumentType, command);
    }

    private async Task<int> ExecuteSingle(IReadOnlyList<string> argsToParse, Type argumentType, ICommand command)
    {
        ArgumentsBase args = ArgumentsBase.Empty;
        ArgumentParser parser = new(argumentType, _log);
        args = parser.Fill(argsToParse);

        var validationResult = args.Validate(ValidationContext);

        if (!validationResult.IsOk)
        {
            _log.LogCritical(validationResult.ToString());
            return _settings.BadParametersExitCode;
        }

        args.ModifyAfterValidation();

        if (BeforeRunHook != null)
            await BeforeRunHook.Invoke(args, argsToParse);

        return await command.ExecuteAsync(args, argsToParse);
    }

    private async Task<int> ExecuteMultiple(ArgumentJsonItem[] items, Type argumentType, ICommand command)
    {
        foreach (var item in items)
        {
            _log.LogInformation("Executing {name} from json file...", item.Name);
            int exitcode = await ExecuteSingle(item.Arguments, argumentType, command);
            if (exitcode != 0)
            {
                _log.LogCritical("Failed to execute {name}. Exit code: {exitcode}", item.Name, exitcode);
                return exitcode;
            }
        }
        return 0;
    }
}
