//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

namespace BookGen.ProjectHandling
{
    internal abstract class LoadStep
    {
        protected readonly FsPath _configJson;
        protected readonly FsPath _configYaml;
        protected readonly FsPath _tagsJson;

        protected LoadStep(LoadState state, ILog log)
        {
            State = state;
            Log = log;
            _configJson = state.WorkDir.Combine(".bookgen/bookgen.json");
            _configYaml = state.WorkDir.Combine(".bookgen/bookgen.yml");
            _tagsJson = state.WorkDir.Combine(".bookgen/tags.json");
        }

        public LoadState State { get; }

        public ILog Log { get; }

        public virtual bool CanExecute() => true;

        public abstract bool Execute();
    }
}
