//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ProjectHandling;

internal static class ProjectFilesLocator
{
    public static (FsPath confgJson, FsPath configYaml, FsPath tags, FsPath tasks) Locate(FsPath workDir)
    {
        FsPath configJson = workDir.Combine(".bookgen/bookgen.json");
        FsPath configYaml = workDir.Combine(".bookgen/bookgen.yml");
        FsPath tagsJson = workDir.Combine(".bookgen/tags.json");
        FsPath tasksXml = workDir.Combine(".bookgen/tasks.xml");

        return (configJson, configYaml, tagsJson, tasksXml);
    }
}
