using BookGen.Domain.Configuration;
using BookGen.Interfaces;

namespace BookGen.ProjectHandling.Steps
{
    internal sealed class LoadConfig : LoadStep
    {
        private readonly FsPath _configJson;
        private readonly FsPath _configYaml;

        public LoadConfig(LoadState state, ILog log) : base(state, log)
        {
            _configJson = state.WorkDir.Combine(".bookgen/bookgen.json");
            _configYaml = state.WorkDir.Combine(".bookgen/bookgen.yml");
        }

        public override bool Execute()
        {
            if (_configYaml.IsExisting && _configJson.IsExisting)
            {
                Log.Critical("both bookgen.json and bookgen.yml present. Decicde config format by deleting one of them");
                return false;
            }

            if (_configYaml.IsExisting)
                State.Config = _configYaml.DeserializeYaml<Config>(Log);

            if (_configJson.IsExisting)
                State.Config = _configJson.DeserializeJson<Config>(Log);

            if (State.Config == null)
            {
                Log.Critical("bookgen.json or boookgen.yml deserialize error. Invalid config file");
            }

            return State.Config != null;
        }
    }
}