//-----------------------------------------------------------------------------
// (c) 2022-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ProjectHandling;

internal abstract class LoadStep
{
    protected readonly FsPath _configJson;
    protected readonly FsPath _configYaml;
    protected readonly FsPath _tagsJson;

    protected LoadStep(LoadState state, ILogger log)
    {
        State = state;
        Log = log;

        ProjectFiles projectFiles = ProjectFilesLocator.Locate(state.WorkDir);

        _configJson = projectFiles.ConfigJson;
        _configYaml = projectFiles.ConfigYaml;
        _tagsJson = projectFiles.TagsJson;
    }

    public LoadState State { get; }

    public ILogger Log { get; }

    public virtual bool CanExecute() => true;

    public abstract bool Execute();
}
