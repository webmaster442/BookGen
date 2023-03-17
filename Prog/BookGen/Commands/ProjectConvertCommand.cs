using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.CommandArguments;
using BookGen.Domain.Configuration;
using BookGen.Interfaces;

namespace BookGen.Commands
{
    [CommandName("projectconvert")]
    internal class ProjectConvertCommand : Command<ProjectConvertArguments>
    {
        private readonly ILog _log;

        public ProjectConvertCommand(ILog log) 
        {
            _log = log;
        }

        public override int Execute(ProjectConvertArguments arguments, string[] context)
        {
            var json = new FsPath(arguments.Directory, Constants.ConfigJson);
            var yml = new FsPath(arguments.Directory, Constants.ConfigYml);

            if (json.IsFile && yml.IsFile)
            {
                _log.Warning("Configuration exists in both formats. Can't continue");
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

        private static bool ConvertYmlToJson(FsPath yml, FsPath json, ILog log, bool backup)
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

        private static bool ConvertToYml(FsPath json, FsPath yml, ILog log, bool backup)
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
}