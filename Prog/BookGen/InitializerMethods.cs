//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Domain.CsProj;
using BookGen.Framework;
using BookGen.Resources;

namespace BookGen;

internal static class InitializerMethods
{
    private const string EpubTemplateLocation = "Templates\\TemplateEpub.html";
    private const string PrintTemplateLocation = "Templates\\TemplatePrint.html";
    private const string WebTemplate = "Templates\\TemplateWeb.html";
    private const string ScriptProject = "Scripts\\ScriptProject.csproj";

    internal static void CreateConfig(ILog log,
                                      FsPath workDir,
                                      bool configInYaml,
                                      bool createMdFiles,
                                      bool createTemplates,
                                      bool createScripts,
                                      int configVersion)
    {
        Config createdConfig = MakeConfigStructure(createMdFiles, createTemplates, createScripts, configVersion);

        if (configInYaml)
        {
            FsPath? file = workDir.Combine("bookgen.yml");
            file.SerializeYaml(createdConfig, log);
        }
        else
        {
            FsPath? file = workDir.Combine("bookgen.json");
            file.SerializeJson(createdConfig, log, true);
        }

    }

    private static Config MakeConfigStructure(bool createdmdFiles, bool extractedTemplate, bool createdScript, int configVersion)
    {
        var configuration = Config.CreateDefault(configVersion);

        if (createdmdFiles)
        {
            configuration.Index = "index.md";
            configuration.TOCFile = "summary.md";
        }

        if (createdScript)
        {
            configuration.ScriptsDirectory = "Scripts";
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
                    Source = "Templates\\Assets\\prism.css",
                    Target = "Assets\\prism.css"
                },
                new Asset
                {
                    Source = "Templates\\Assets\\prism.js",
                    Target = "Assets\\prism.js"
                }
            };
        }

        return configuration;
    }


    public static void DoCreateMdFiles(ILog log, FsPath workdir)
    {
        log.Info("Creating index.md...");
        ResourceHandler.ExtractKnownFile(KnownFile.IndexMd, workdir.ToString(), log);
        log.Info("Creating Summary.md...");
        ResourceHandler.ExtractKnownFile(KnownFile.SummaryMd, workdir.ToString(), log);
    }

    public static void CreateScriptProject(ILog log, FsPath workdir, string ApiReferencePath)
    {
        log.Info("Creating scripts project...");
        var p = new Project
        {
            Sdk = "Microsoft.NET.Sdk",
            PropertyGroup = new PropertyGroup
            {
                Nullable = "enable",
                TargetFramework = "netstandard2.1"
            },
            ItemGroup = new ItemGroup
            {
                Reference = new Reference
                {
                    Include = "BookGen.Api",
                    HintPath = Path.Combine(ApiReferencePath, "BookGen.Api.dll")
                }
            }
        };
        FsPath csProj = workdir.Combine(ScriptProject);
        csProj.SerializeXml(p, log);

        ResourceHandler.ExtractKnownFile(KnownFile.ScriptTemplateCs, workdir.Combine("Scripts").ToString(), log);
    }

    internal static void DoCreateTasks(ILog log, FsPath workDir)
    {
        Domain.VsTasks.VsTaskRoot Tasks = VsTaskFactory.CreateTasks();
        FsPath file = workDir.Combine(".vscode\\tasks.json");
        file.SerializeJson(Tasks, log);
    }

    public static void ExtractTemplates(ILog log, FsPath workdir)
    {
        string? templatedir = workdir.Combine("Templates").ToString();
        string? assetsdir = workdir.Combine("Templates\\Assets").ToString();

        ResourceHandler.ExtractKnownFile(KnownFile.TemplateEpubHtml, templatedir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.TemplatePrintHtml, templatedir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.TemplateWebHtml, templatedir, log);

        ResourceHandler.ExtractKnownFile(KnownFile.PrismCss, assetsdir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.PrismJs, assetsdir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.BootstrapMinCss, assetsdir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.BootstrapMinJs, assetsdir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.JqueryMinJs, assetsdir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.PopperMinJs, assetsdir, log);
        ResourceHandler.ExtractKnownFile(KnownFile.TurbolinksJs, assetsdir, log);

    }
}
