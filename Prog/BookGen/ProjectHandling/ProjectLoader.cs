﻿//-----------------------------------------------------------------------------
// (c) 2022-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.ProjectHandling.Steps;

namespace BookGen.ProjectHandling;

internal sealed class ProjectLoader
{
    private readonly LoadStep[] _loadSteps;
    private readonly LoadState _state;
    private readonly ILogger _log;
    private bool _loaded;

    public ProjectLoader(string workDir, ILogger log, ProgramInfo programInfo)
    {
        _state = new LoadState(workDir, programInfo.ConfigVersion);
        _log = log;
        _loadSteps =
        [
            new MigrateProjectIfOldStyle(_state, _log),
            new ConfigLoad(_state, _log),
            new ConfigUpgrade(_state, _log),
            new ConfigValidate(_state, _log),
            new TocLoad(_state, _log),
            new TocValidate(_state, _log),
            new TagsLoad(_state, _log),
        ];
    }

    public Config Configuration
    {
        get
        {
            EnsureThatLoaded();
            return _state.Config!;
        }
    }

    public ToC Toc
    {
        get
        {
            EnsureThatLoaded();
            return _state.Toc!;
        }
    }

    public Dictionary<string, string[]> Tags
    {
        get => _state.Tags;
    }

    public bool IsBookGenFolder
    {
        get
        {
            ProjectFiles projectFiles = ProjectFilesLocator.Locate(_state.WorkDir);
            return projectFiles.ConfigJson.IsExisting 
                || projectFiles.ConfigYaml.IsExisting;
        }
    }

    public bool LoadProject()
    {
        foreach (var step in _loadSteps)
        {
            if (!step.CanExecute())
            {
                _log.LogDebug("Skipping load step: {stepname}", step.GetType().Name);
                continue;
            }
            if (!step.Execute())
            {
                _log.LogCritical("Project load failed at step: {stepname}", step.GetType().Name);
                _loaded = false;
                return false;
            }
        }
        _loaded = true;
        return true;
    }

    public RuntimeSettings CreateRuntimeSettings(BuildConfig current)
    {
        EnsureThatLoaded();

        var settings = new RuntimeSettings()
        {
            SourceDirectory = _state.WorkDir,
            Configuration = _state.Config,
            TocContents = _state.Toc!,
            MetataCache = new Dictionary<string, string>(100),
            InlineImgCache = new ConcurrentDictionary<string, string>(),
            CurrentBuildConfig = current,
            Tags = new TagUtils(_state.Tags, _state.Config!.BookLanguage),
        };

        if (string.IsNullOrEmpty(_state.Config.ImageDir))
            settings.ImageDirectory = FsPath.Empty;
        else
            settings.ImageDirectory = settings.SourceDirectory.Combine(_state.Config.ImageDir);

        return settings;
    }

    private void EnsureThatLoaded()
    {
        if (!_loaded
            || _state.Config == null
            || _state.Toc == null)
        {
            throw new InvalidOperationException($"{nameof(CreateRuntimeSettings)} called on  invalid project");
        }
    }
}
