//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;

namespace BookGen.Framework;

public static class TemplateLoader
{
    public static string LoadTemplate(FsPath workingDirectory,
                                      BuildConfig buildConfig,
                                      ILogger log,
                                      string FallBackTemplate)
    {
        if (string.IsNullOrEmpty(buildConfig.TemplateFile))
            return FallBackTemplate;

        FsPath templatePath = workingDirectory.Combine(buildConfig.TemplateFile);

        if (!templatePath.IsExisting)
        {
            log.LogWarning("Template not found: {file}", buildConfig.TemplateFile);
            log.LogInformation("Switching to built-in template.");
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
