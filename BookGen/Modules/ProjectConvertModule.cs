//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Utilities;

namespace BookGen.Modules
{
    internal class ProjectConvertModule : ModuleWithState
    {
        public ProjectConvertModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "ProjectConvert";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-d",
                                            "--dir",
                                            "-b",
                                            "--backup");
            }
        }

        public override ModuleRunResult Execute(string[] arguments)
        {
            ProjectConvertArguments args = new ProjectConvertArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }
            var json = new FsPath(args.Directory, Constants.ConfigJson);
            var yml = new FsPath(args.Directory, Constants.ConfigYml);

            if (json.IsFile && yml.IsFile)
            {
                CurrentState.Log.Warning("Configuration exists in both formats. Can't continue");
                return ModuleRunResult.GeneralError;
            }
            else if (json.IsFile)
            {
                return ConvertToYml(json, yml, CurrentState.Log, args.Backup).ToSuccesOrError();
            }
            else
            {
                return ConvertYmlToJson(yml, json, CurrentState.Log, args.Backup).ToSuccesOrError();
            }
        }

        private bool ConvertYmlToJson(FsPath yml, FsPath json, ILog log, bool backup)
        {
            var config = yml.DeserializeYaml<Config>(log);
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

        private bool ConvertToYml(FsPath json, FsPath yml, ILog log, bool backup)
        {
            var config = json.DeserializeJson<Config>(log);
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
