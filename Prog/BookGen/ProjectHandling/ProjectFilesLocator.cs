//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

namespace BookGen.ProjectHandling
{
    internal static class ProjectFilesLocator
    {
        public static (FsPath confgJson, FsPath configYaml, FsPath tags) Locate(FsPath workDir)
        {
            FsPath configJson = workDir.Combine(".bookgen/bookgen.json");
            FsPath configYaml = workDir.Combine(".bookgen/bookgen.yml");
            FsPath tagsJson = workDir.Combine(".bookgen/tags.json");

            return (configJson, configYaml, tagsJson);
        }
    }
}
