//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json;

using BookGen.CommandArguments;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("jsonargs")]
internal sealed class JsonArgsCommand : Command<JsonArgsArguments>
{
    private readonly HashSet<string> _commandNames;
    private readonly ILogger _log;
    private readonly ProgramInfo _programInfo;

    public JsonArgsCommand(IModuleApi api, ILogger log, ProgramInfo programInfo)
    {
        _commandNames = new HashSet<string>(api.GetCommandNames());
        _log = log;
        _programInfo = programInfo;
    }

    public override int Execute(JsonArgsArguments arguments, string[] context)
    {
        _programInfo.EnableVerboseLogingIfRequested(arguments);

        if (!_commandNames.Contains(arguments.CommandName))
        {
            _log.LogCritical("Command not found: {commandname}", arguments.CommandName);
            return Constants.ArgumentsError;
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

        return Constants.Succes;
    }
}
