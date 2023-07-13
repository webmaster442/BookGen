//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ProjectHandling;

internal sealed class ProjectFiles
{
    public required FsPath ConfigJson { get; init; }
    public required FsPath ConfigYaml { get; init; }
    public required FsPath TagsJson { get; init; }
    public required FsPath TasksXml { get; init; }

    public void AddToPackListIfExist(IList<string> filesToPack)
    {
        if (ConfigJson.IsExisting)
            filesToPack.Add(ConfigJson.ToString());

        if (ConfigYaml.IsExisting)
            filesToPack.Add(ConfigYaml.ToString());

        if (TagsJson.IsExisting)
            filesToPack.Add(TagsJson.ToString());

        if (TasksXml.IsExisting)
            filesToPack.Add(TasksXml.ToString());
    }
}