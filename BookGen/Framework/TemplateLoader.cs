//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;

namespace BookGen.Framework
{
    public static class TemplateLoader
    {
        public static string LoadTemplate(FsPath workingDirectory,
                                          BuildConfig buildConfig,
                                          ILog log,
                                          string FallBackTemplate)
        {
            if (string.IsNullOrEmpty(buildConfig.TemplateFile))
                return FallBackTemplate;

            FsPath templatePath = workingDirectory.Combine(buildConfig.TemplateFile);

            if (!templatePath.IsExisting)
            {
                log.Warning("Template not found: {0}", buildConfig.TemplateFile);
                log.Info("Switching to built-in template.");
                return FallBackTemplate;
            }

            return templatePath.ReadFile(log);

        }

        public static bool FallbackTemplateRequired(FsPath workingDirectory, BuildConfig buildConfig)
        {
            if (string.IsNullOrEmpty(buildConfig.TemplateFile))
                return true;

            FsPath templatePath = workingDirectory.Combine(buildConfig.TemplateFile);

            return !templatePath.IsExisting;
        }
    }
}
