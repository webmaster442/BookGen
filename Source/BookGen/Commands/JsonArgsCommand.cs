//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Text.Json;

using BookGen.Cli;
using BookGen.Cli.Annotations;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("jsonargs")]
[Description("Creates an empty json arguments template file for a given bookgen command.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class JsonArgsCommand : Command<JsonArgsCommand.Arguments>
{
    internal sealed class Arguments : BookGenArgumentBase
    {
        [Switch("c", "command", Required = true)]
        [Description("Specifies the command for which the json template will be created.")]
        public string CommandName { get; set; }

        public Arguments()
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

    public JsonArgsCommand(ICommandRunnerProxy runnerProxy, ILogger log)
    {
        _commandNames = [.. runnerProxy.CommandNames];
        _log = log;
    }

    public override int Execute(Arguments arguments, IReadOnlyList<string> context)
    {
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

        return ExitCodes.Success;
    }
}
