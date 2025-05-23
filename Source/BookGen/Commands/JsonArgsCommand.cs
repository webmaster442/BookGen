//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json;

using BookGen.Cli;
using BookGen.Cli.Annotations;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("jsonargs")]
internal sealed class JsonArgsCommand : Command<JsonArgsCommand.JsonArgsArguments>
{
    internal sealed class JsonArgsArguments : BookGenArgumentBase
    {
        [Switch("c", "command")]
        public string CommandName { get; set; }

        public JsonArgsArguments()
        {
            CommandName = string.Empty;
        }

        public override ValidationResult Validate(IValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(CommandName))
            {
                return ValidationResult.Error("CommandName is missing");
            }
            return ValidationResult.Ok();
        }
    }


    private readonly HashSet<string> _commandNames;
    private readonly ILogger _log;
    private readonly ProgramInfo _programInfo;

    public JsonArgsCommand(ICommandRunnerProxy runnerProxy, ILogger log, ProgramInfo programInfo)
    {
        _commandNames = [.. runnerProxy.CommandNames];
        _log = log;
        _programInfo = programInfo;
    }

    public override int Execute(JsonArgsArguments arguments, IReadOnlyList<string> context)
    {
        _programInfo.EnableVerboseLogingIfRequested(arguments);

        if (!_commandNames.Contains(arguments.CommandName))
        {
            _log.LogCritical("Command not found: {commandname}", arguments.CommandName);
            return ExitCodes.ArgumentsError;
        }

        var empty = new ArgumentJsonItem[]
        {
            new ArgumentJsonItem
            {
                Name = "Log entry describing action",
                Arguments = Array.Empty<string>(),
            }
        };

        _log.LogInformation("Creating json file for {command} command...", arguments.CommandName);
        var fileName = Path.Combine(arguments.Directory, $"{arguments.CommandName}.json");
        var json = JsonSerializer.Serialize(empty, new JsonSerializerOptions
        {
            WriteIndented = true,
        });
        File.WriteAllText(fileName, json);

        return ExitCodes.Succes;
    }
}
