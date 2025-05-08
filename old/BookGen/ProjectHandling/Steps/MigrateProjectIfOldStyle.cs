//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ProjectHandling.Steps;

internal sealed class MigrateProjectIfOldStyle : LoadStep
{
    private readonly FsPath _OldconfigJson;
    private readonly FsPath _OldconfigYaml;
    private readonly FsPath _Oldtags;

    public MigrateProjectIfOldStyle(LoadState state, ILogger log) : base(state, log)
    {
        _OldconfigJson = state.WorkDir.Combine("bookgen.json");
        _OldconfigYaml = state.WorkDir.Combine("bookgen.yml");
        _Oldtags = state.WorkDir.Combine("tags.json");
    }

    public override bool CanExecute()
    {
        return
            (_OldconfigJson.IsExisting && !_configJson.IsExisting)
            || (_OldconfigYaml.IsExisting && !_configYaml.IsExisting)
            || (_Oldtags.IsExisting && !_tagsJson.IsExisting);
    }

    public override bool Execute()
    {
        bool retval = true;

        if (_OldconfigJson.IsExisting && !_configJson.IsExisting)
            retval = _OldconfigJson.Move(_configJson, Log);

        if (_OldconfigYaml.IsExisting && !_configYaml.IsExisting)
            retval = _OldconfigYaml.Move(_configYaml, Log);

        if (_Oldtags.IsExisting && !_tagsJson.IsExisting)
            retval = _Oldtags.Move(_tagsJson, Log);

        return retval;
    }
}
