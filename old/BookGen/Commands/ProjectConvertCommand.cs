//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Domain.Configuration;

namespace BookGen.Commands;

[CommandName("projectconvert")]
internal class ProjectConvertCommand : Command<ProjectConvertArguments>
{
    private readonly ILogger _log;

    public ProjectConvertCommand(ILogger log)
    {
        _log = log;
    }

    public override int Execute(ProjectConvertArguments arguments, string[] context)
    {
        var json = new FsPath(arguments.Directory, Constants.ConfigJson);
        var yml = new FsPath(arguments.Directory, Constants.ConfigYml);

        if (json.IsFile && yml.IsFile)
        {
            _log.LogWarning("Configuration exists in both formats. Can't continue");
            return Constants.GeneralError;
        }
        else if (json.IsFile)
        {
            return ConvertToYml(json, yml, _log, arguments.Backup)
                ? Constants.Succes
                : Constants.GeneralError;
        }
        else
        {
            return ConvertYmlToJson(yml, json, _log, arguments.Backup)
                ? Constants.Succes
                : Constants.GeneralError;
        }
    }

    private static bool ConvertYmlToJson(FsPath yml, FsPath json, ILogger log, bool backup)
    {
        Config? config = yml.DeserializeYaml<Config>(log);
        if (config == null)
        {
            return false;
        }
        if (backup)
        {
            yml.CreateBackup(log);
        }
        return yml.Delete(log)
            && json.SerializeJson(config, log, true);
    }

    private static bool ConvertToYml(FsPath json, FsPath yml, ILogger log, bool backup)
    {
        Config? config = json.DeserializeJson<Config>(log);
        if (config == null)
        {
            return false;
        }
        if (backup)
        {
            json.CreateBackup(log);
        }
        return json.Delete(log)
            && yml.SerializeYaml(config, log);
    }
}