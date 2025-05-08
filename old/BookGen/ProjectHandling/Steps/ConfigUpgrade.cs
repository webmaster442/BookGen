//-----------------------------------------------------------------------------
// (c) 2022-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ProjectHandling.Steps;

internal sealed class ConfigUpgrade : LoadStep
{
    public ConfigUpgrade(LoadState state, ILogger log) : base(state, log)
    {
    }

    public override bool CanExecute()
    {
        return
            State.Config != null
            && State.Config.Version < State.ConfigVersion;
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
            State.Config.UpgradeTo(State.ConfigVersion);
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

        Log.LogInformation("Configuration file migrated to new version.");
        Log.LogInformation("Review configuration then run program again");
        return false;
    }
}
