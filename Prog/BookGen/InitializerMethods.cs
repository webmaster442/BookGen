//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Resources;

namespace BookGen;

internal static class InitializerMethods
{
    private const string EpubTemplateLocation = ".bookgen\\Templates\\TemplateEpub.html";
    private const string PrintTemplateLocation = ".bookgen\\Templates\\TemplatePrint.html";
    private const string WebTemplate = ".bookgen\\Templates\\TemplateWeb.html";

    internal static void CreateConfig(ILogger log,
                                      FsPath workDir,
                                      bool configInYaml,
                                      bool createMdFiles,
                                      bool createTemplates,
                                      int configVersion)
    {
        Config createdConfig = MakeConfigStructure(createMdFiles, createTemplates, configVersion);

        if (configInYaml)
        {
            FsPath? file = workDir.Combine(".bookgen\\bookgen.yml");
            file.SerializeYaml(createdConfig, log);
        }
        else
        {
            FsPath? file = workDir.Combine(".bookgen\\bookgen.json");
            file.SerializeJson(createdConfig, log, true);
        }

    }

    private static Config MakeConfigStructure(bool createdmdFiles, bool extractedTemplate, int configVersion)
    {
        var configuration = Config.CreateDefault(configVersion);

        if (createdmdFiles)
        {
            configuration.Index = "index.md";
            configuration.TOCFile = "summary.md";
        }

        if (extractedTemplate)
        {
            configuration.TargetEpub.TemplateFile = EpubTemplateLocation;
            configuration.TargetPrint.TemplateFile = PrintTemplateLocation;
            configuration.TargetWeb.TemplateFile = WebTemplate;
            configuration.TargetWeb.TemplateAssets = new List<Asset>
            {
                new Asset
                {
                    Source = ".bookgen\\Templates\\Assets\\prism.css",
                    Target = "Assets\\prism.css"
                },
                new Asset
                {
                    Source = ".bookgen\\Templates\\Assets\\prism.js",
                    Target = "Assets\\prism.js"
                }
            };
        }

        return configuration;
    }


    public static void DoCreateMdFiles(ILogger log, FsPath workdir)
    {
        log.LogInformation("Creating index.md...");
        ResourceHandler.ExtractKnownFile(KnownFile.IndexMd, workdir.ToString(), log);
        log.LogInformation("Creating Summary.md...");
        ResourceHandler.ExtractKnownFile(KnownFile.SummaryMd, workdir.ToString(), log);
    }

    public static void ExtractTemplates(ILogger log, FsPath workdir)
    {
        string? templatedir = workdir.Combine(".bookgen\\Templates").ToString();
        string? assetsdir = workdir.Combine(".bookgen\\Templates\\Assets").ToString();

        ResourceHandler.ExtractKnownFile(KnownFile.TemplateEpubHtml, templatedir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.TemplatePrintHtml, templatedir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.TemplateWebHtml, templatedir, log);

        ResourceHandler.ExtractKnownFile(KnownFile.PrismCss, assetsdir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.PrismJs, assetsdir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.BootstrapMinCss, assetsdir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.BootstrapMinJs, assetsdir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.JqueryMinJs, assetsdir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.PopperMinJs, assetsdir, log);
    }
}
