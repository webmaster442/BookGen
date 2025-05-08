//-----------------------------------------------------------------------------
// (c) 2022-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ProjectHandling;

internal static class ProjectFilesLocator
{
    public static ProjectFiles Locate(FsPath workDir)
    {
        return new ProjectFiles
        {
            ConfigJson = workDir.Combine(".bookgen/bookgen.json"),
            ConfigYaml = workDir.Combine(".bookgen/bookgen.yml"),
            TagsJson = workDir.Combine(".bookgen/tags.json"),
            TasksXml = workDir.Combine(".bookgen/tasks.xml"),
        };
    }
}
