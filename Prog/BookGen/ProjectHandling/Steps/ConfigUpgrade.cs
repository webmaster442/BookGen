﻿namespace BookGen.ProjectHandling.Steps
{
    internal sealed class ConfigUpgrade : LoadStep
    {
        public ConfigUpgrade(LoadState state, ILog log) : base(state, log)
        {
        }

        public override bool CanExecute()
        {
            return
                State.Config != null
                && State.Config.Version < Program.CurrentState.ConfigVersion;
        }

        public override bool Execute()
        {
            switch (State.ConfigFormat)
            {
                case ConfigFormat.Json:
                    _configJson.CreateBackup(Log);
                    break;
                case ConfigFormat.Yaml:
                    _configYaml.CreateBackup(Log);
                    break;
            }

            if (State.Config != null)
            {
                State.Config.UpgradeTo(Program.CurrentState.ConfigVersion);
                switch (State.ConfigFormat)
                {
                    case ConfigFormat.Json:
                        _configJson.SerializeJson(State.Config, Log, true);
                        break;
                    case ConfigFormat.Yaml:
                        _configYaml.SerializeYaml(State.Config, Log);
                        break;
                }
            }

            Log.Info("Configuration file migrated to new version.");
            Log.Info("Review configuration then run program again");
            return false;
        }
    }
}
