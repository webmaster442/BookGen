//-----------------------------------------------------------------------------
// (c) 2022-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;

namespace BookGen.ProjectHandling.Steps;

internal sealed class ConfigLoad : LoadStep
{
    public ConfigLoad(LoadState state, ILogger log) : base(state, log)
    {
    }

    public override bool Execute()
    {
        if (_configYaml.IsExisting && _configJson.IsExisting)
        {
            Log.LogCritical("both bookgen.json and bookgen.yml present. Decicde config format by deleting one of them");
            return false;
        }

        if (_configYaml.IsExisting)
        {
            State.Config = _configYaml.DeserializeYaml<Config>(Log);
            State.ConfigFormat = ConfigFormat.Yaml;
        }

        if (_configJson.IsExisting)
        {
            State.Config = _configJson.DeserializeJson<Config>(Log);
            State.ConfigFormat = ConfigFormat.Json;
        }

        if (State.Config == null)
        {
            Log.LogCritical("bookgen.json or boookgen.yml deserialize error. Invalid config file");
        }

        return State.Config != null;
    }
}