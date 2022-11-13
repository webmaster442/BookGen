using BookGen.ProjectHandling.Steps;

namespace BookGen.ProjectHandling
{
    internal sealed class ProjectLoaderV2
    {
        private LoadStep[] _loadSteps;
        private readonly LoadState _state;
        private readonly ILog _log;

        public ProjectLoaderV2(string workDir, ILog log)
        {
            _state = new LoadState(workDir);
            _log = log;
            _loadSteps = new LoadStep[]
            {
                new MigrateProjectIfOldStyle(_state, _log),
                new LoadConfig(_state, _log),
            };
        }
    }
}
