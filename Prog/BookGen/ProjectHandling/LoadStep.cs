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

    protected LoadStep(LoadState state, ILog log)
    {
        State = state;
        Log = log;

        var (confgJson, configYaml, tags, _) = ProjectFilesLocator.Locate(state.WorkDir);

        _configJson = confgJson;
        _configYaml = configYaml;
        _tagsJson = tags;
    }

    public LoadState State { get; }

    public ILog Log { get; }

    public virtual bool CanExecute() => true;

    public abstract bool Execute();
}
