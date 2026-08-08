//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("script")]
[Description("Executes a script file.")]
[ExitCode(ExitCodes.Success, "The script completed successfully.")]
[ExitCode(ExitCodes.ArgumentsError, "The script file doesn't exist or a command couldn't be resolved.")]
internal sealed class ScriptCommand : AsyncCommand<ScriptCommand.Arguments>
{
    public sealed class Arguments : ArgumentsBase
    {
        [Argument(0)]
        [Description("The path to the script file to execute.")]
        public string ScriptPath { get; set; } = string.Empty;

        public override ValidationResult Validate(IValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(ScriptPath))
                return ValidationResult.Error("Script file path must be specified.");

            if (!context.FileSystem.FileExists(ScriptPath))
                return ValidationResult.Error($"Script file doesn't exist: {ScriptPath}");

            return ValidationResult.Ok();
        }
    }

    private readonly ICommandRunnerProxy _commandRunnerProxy;
    private readonly ILogger _logger;
    private readonly IReadOnlyFileSystem _fileSystem;

    public ScriptCommand(ICommandRunnerProxy commandRunnerProxy, ILogger logger, IReadOnlyFileSystem fileSystem)
    {
        _commandRunnerProxy = commandRunnerProxy;
        _logger = logger;
        _fileSystem = fileSystem;
    }

    public override async Task<int> ExecuteAsync(Arguments arguments, IReadOnlyList<string> context, CancellationToken token)
    {
        if (!_fileSystem.FileExists(arguments.ScriptPath))
        {
            _logger.LogError("Script file doesn't exist: {path}", arguments.ScriptPath);
            return ExitCodes.ArgumentsError;
        }

        using TextReader reader = _fileSystem.OpenTextReader(arguments.ScriptPath);

        List<string> rawLines = await reader.ReadAllLinesAsync(token);

        IReadOnlyList<string> commandNames = _commandRunnerProxy.CommandNames
            .OrderByDescending(x => x.Length)
            .ToArray();

        int exitCode = ExitCodes.Success;

        foreach (string logicalLine in JoinContinuations(rawLines))
        {
            token.ThrowIfCancellationRequested();

            // Handle log instructions: they must appear at the very start of the line.
            if (TryGetLogMessage(logicalLine, out string? logMessage))
            {
                _logger.LogInformation("{message}", logMessage);
                continue;
            }

            IReadOnlyList<string> tokens = Tokenize(logicalLine);

            if (tokens.Count == 0)
                continue;

            if (!TryResolveCommand(tokens, commandNames, out string? commandName, out IReadOnlyList<string> commandArgs))
            {
                _logger.LogError("Unknown command: {command}", string.Join(' ', tokens));
                return ExitCodes.ArgumentsError;
            }

            _logger.LogInformation("Running: {command}", logicalLine.Trim());

            exitCode = await _commandRunnerProxy.RunCommand(commandName, commandArgs);

            // pipefail style: stop on the first failing command and return its exit code.
            if (exitCode != ExitCodes.Success)
            {
                _logger.LogError("Command failed with exit code {code}, stopping script execution.", exitCode);
                return exitCode;
            }
        }

        return exitCode;
    }

    private static IEnumerable<string> JoinContinuations(IEnumerable<string> lines)
    {
        StringBuilder? builder = null;

        foreach (string line in lines)
        {
            string current = line;

            if (current.EndsWith('\\'))
            {
                builder ??= new StringBuilder();
                builder.Append(current.AsSpan(0, current.Length - 1));
                builder.Append(' ');
                continue;
            }

            if (builder != null)
            {
                builder.Append(current);
                yield return builder.ToString();
                builder = null;
            }
            else
            {
                yield return current;
            }
        }

        if (builder != null)
            yield return builder.ToString();
    }

    private static bool TryGetLogMessage(string line, out string? message)
    {
        message = null;
        string trimmed = line.TrimStart();

        foreach (string marker in (ReadOnlySpan<string>)["#log", "//log"])
        {
            if (trimmed.StartsWith(marker, StringComparison.Ordinal))
            {
                string rest = trimmed[marker.Length..];

                // The marker must be followed by whitespace or the end of line to be treated as a log instruction.
                if (rest.Length == 0 || char.IsWhiteSpace(rest[0]))
                {
                    message = rest.Trim();
                    return true;
                }
            }
        }

        return false;
    }

    private static List<string> Tokenize(string line)
    {
        var tokens = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;
        bool hasToken = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (inQuotes)
            {
                if (c == '"')
                    inQuotes = false;
                else
                    current.Append(c);

                continue;
            }

            switch (c)
            {
                case '"':
                    inQuotes = true;
                    hasToken = true;
                    break;
                case '#':
                    // Rest of the line is a comment.
                    FlushToken(tokens, current, ref hasToken);
                    return tokens;
                case '/' when i + 1 < line.Length && line[i + 1] == '/':
                    // Rest of the line is a comment.
                    FlushToken(tokens, current, ref hasToken);
                    return tokens;
                default:
                    if (char.IsWhiteSpace(c))
                    {
                        FlushToken(tokens, current, ref hasToken);
                    }
                    else
                    {
                        current.Append(c);
                        hasToken = true;
                    }
                    break;
            }
        }

        FlushToken(tokens, current, ref hasToken);
        return tokens;

        static void FlushToken(List<string> tokens, StringBuilder current, ref bool hasToken)
        {
            if (hasToken)
            {
                tokens.Add(current.ToString());
                current.Clear();
                hasToken = false;
            }
        }
    }

    private static bool TryResolveCommand(IReadOnlyList<string> tokens,
                                          IReadOnlyList<string> commandNames,
                                          out string commandName,
                                          out IReadOnlyList<string> commandArgs)
    {
        foreach (string candidate in commandNames)
        {
            string[] parts = candidate.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > tokens.Count)
                continue;

            bool matches = true;
            for (int i = 0; i < parts.Length; i++)
            {
                if (!string.Equals(parts[i], tokens[i], StringComparison.OrdinalIgnoreCase))
                {
                    matches = false;
                    break;
                }
            }

            if (matches)
            {
                commandName = candidate;
                commandArgs = tokens.Skip(parts.Length).ToArray();
                return true;
            }
        }

        commandName = string.Empty;
        commandArgs = [];
        return false;
    }
}
