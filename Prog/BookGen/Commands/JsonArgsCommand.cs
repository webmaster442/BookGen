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
    private readonly ILog _log;

    public JsonArgsCommand(IModuleApi api, ILog log)
    {
        _commandNames = new HashSet<string>(api.GetCommandNames());
        _log = log;
    }

    public override int Execute(JsonArgsArguments arguments, string[] context)
    {
        _log.EnableVerboseLogingIfRequested(arguments);

        if (!_commandNames.Contains(arguments.CommandName))
        {
            _log.Critical("Command not found: {0}", arguments.CommandName);
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

        _log.Info("Creating json file for {0} command...", arguments.CommandName);
        var fileName = Path.Combine(arguments.Directory, $"{arguments.CommandName}.json");
        var json = JsonSerializer.Serialize(empty, new JsonSerializerOptions
        {
            WriteIndented = true,
        });
        File.WriteAllText(fileName, json);

        return Constants.Succes;
    }
}
