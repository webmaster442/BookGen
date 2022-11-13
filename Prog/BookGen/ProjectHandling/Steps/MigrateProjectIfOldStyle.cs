using BookGen.Interfaces;

namespace BookGen.ProjectHandling.Steps
{
    internal sealed class MigrateProjectIfOldStyle : LoadStep
    {
        private readonly FsPath _configJson;
        private readonly FsPath _configYaml;
        private readonly FsPath _tags;
        private readonly FsPath _newConfigJson;
        private readonly FsPath _newConfigYaml;
        private readonly FsPath _newTags;

        public MigrateProjectIfOldStyle(LoadState state, ILog log) : base(state, log)
        {
            _configJson = state.WorkDir.Combine("bookgen.json");
            _configYaml = state.WorkDir.Combine("bookgen.yml");
            _tags = state.WorkDir.Combine("tags.json");
            _newConfigJson = state.WorkDir.Combine(".bookgen/bookgen.json");
            _newConfigYaml = state.WorkDir.Combine(".bookgen/bookgen.yml");
            _newTags = state.WorkDir.Combine(".bookgen/tags.json");
        }

        public override bool CanExecute()
        {
            return
                (_configJson.IsExisting && !_newConfigJson.IsExisting)
                || (_configYaml.IsExisting && !_newConfigYaml.IsExisting)
                || (_tags.IsExisting && !_newTags.IsExisting);
        }

        public override bool Execute()
        {
            bool retval = true;

            if (_configJson.IsExisting && !_newConfigJson.IsExisting)
                retval = _configJson.Move(_newConfigJson, Log);

            if (_configYaml.IsExisting && !_newConfigYaml.IsExisting)
                retval = _configYaml.Move(_newConfigYaml, Log);

            if (_tags.IsExisting && !_newTags.IsExisting)
                retval = _tags.Move(_newTags, Log);

            return retval;
        }
    }
}
