// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Template;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BookGen
{
    internal static class InitializerMethods
    {
        private const string EpubTemplateLocation = "Templates\\TemplateEpub.html";
        private const string PrintTemplateLocation = "Templates\\TemplatePrint.html";
        private const string WebTemplate = "Templates\\TemplateWeb.html";

        public static void DoCreateConfig(ILog log,
                                          FsPath ConfigFile,
                                          bool createdmdFiles,
                                          bool extractedTemplate)
        {
            Config configuration = Config.CreateDefault(Program.ConfigVersion);

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

            log.Info("Creating config file: {0}", ConfigFile.ToString());
            var def = JsonConvert.SerializeObject(configuration, Formatting.Indented);
            ConfigFile.WriteFile(def);
        }

        public static void DoCreateMdFiles(ILog log, FsPath workdir)
        {
            log.Info("Creating index.md...");

            FsPath index = workdir.Combine("index.md");
            index.WriteFile(BuiltInTemplates.IndexMd);

            log.Info("Creating summary.md...");
            FsPath summary = workdir.Combine("summary.md");
            summary.WriteFile(BuiltInTemplates.SummaryMd);
        }

        public static void ExtractTemplates(ILog log, FsPath workdir)
        {
            FsPath epub = workdir.Combine(EpubTemplateLocation);
            epub.WriteFile(BuiltInTemplates.Epub);

            FsPath print = workdir.Combine(PrintTemplateLocation);
            print.WriteFile(BuiltInTemplates.Print);

            FsPath web = workdir.Combine(WebTemplate);
            web.WriteFile(BuiltInTemplates.TemplateWeb);

            FsPath prismcss = workdir.Combine("Templates\\Assets\\prism.css");
            prismcss.WriteFile(BuiltInTemplates.AssetPrismCss);

            FsPath prismjs = workdir.Combine("Templates\\Assets\\prism.js");
            prismjs.WriteFile(BuiltInTemplates.AssetPrismJs);

            FsPath bootstrapcss = workdir.Combine("Templates\\Assets\\bootstrap.min.css");
            bootstrapcss.WriteFile(BuiltInTemplates.AssetBootstrapCSS);

            FsPath bootstrapjs = workdir.Combine("Templates\\Assets\\bootstrap.min.js");
            bootstrapjs.WriteFile(BuiltInTemplates.AssetBootstrapJs);

            FsPath jquery = workdir.Combine("Templates\\Assets\\jquery.min.js");
            jquery.WriteFile(BuiltInTemplates.AssetJqueryJs);

            FsPath popper = workdir.Combine("Templates\\Assets\\popper.min.js");
            popper.WriteFile(BuiltInTemplates.AssetPopperJs);

        }
    }
}
