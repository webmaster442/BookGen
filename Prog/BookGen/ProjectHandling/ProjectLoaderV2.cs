using BookGen.Domain.Configuration;
using BookGen.Interfaces;
using BookGen.ProjectHandling.Steps;

namespace BookGen.ProjectHandling
{
    internal sealed class ProjectLoaderV2
    {
        private readonly LoadStep[] _loadSteps;
        private readonly LoadState _state;
        private readonly ILog _log;
        private bool _loaded;

        public ProjectLoaderV2(string workDir, ILog log)
        {
            _state = new LoadState(workDir);
            _log = log;
            _loadSteps = new LoadStep[]
            {
                new MigrateProjectIfOldStyle(_state, _log),
                new ConfigLoad(_state, _log),
                new ConfigUpgrade(_state, _log),
                new ConfigValidate(_state, _log),
                new TocLoad(_state, _log),
                new TocValidate(_state, _log),
                new TagsLoad(_state, _log),
            };
        }

        public bool LoadProject()
        {
            foreach (var step in _loadSteps)
            {
                if (!step.CanExecute())
                {
                    _log.Detail("Skipping load step: {0}", step.GetType().Name);
                    continue;
                }
                if (!step.Execute())
                {
                    _log.Critical("Project load failed at step: {0}", step.GetType().Name);
                    _loaded = false;
                    return false;
                }
            }
            _loaded = true;
            return true;
        }
 
        public RuntimeSettings CreateRuntimeSettings(BuildConfig current)
        {
            if (!_loaded
                || _state.Config == null 
                || _state.Toc == null)
            {
                throw new InvalidOperationException($"{nameof(CreateRuntimeSettings)} called on  invalid project");
            }

            var tagUtils = new TagUtils(_state.Tags, _state.Config.BookLanguage);

            var settings = new RuntimeSettings(tagUtils)
            {
                SourceDirectory = _state.WorkDir,
                Configuration = _state.Config,
                TocContents = _state.Toc,
                MetataCache = new Dictionary<string, string>(100),
                InlineImgCache = new ConcurrentDictionary<string, string>(),
                CurrentBuildConfig = current,
            };

            if (string.IsNullOrEmpty(_state.Config.ImageDir))
                settings.ImageDirectory = FsPath.Empty;
            else
                settings.ImageDirectory = settings.SourceDirectory.Combine(_state.Config.ImageDir);

            return settings;
        }
    }
}
