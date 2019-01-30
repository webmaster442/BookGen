//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookGen
{
    internal class WebsiteBuilder
    {
        private static void CopyImages(FsPath outdir, FsPath imgdir)
        {
            Console.WriteLine("Copy images to output...");
            imgdir.CopyDirectory(outdir.Combine(imgdir.GetName()));
        }

        private static void CreateOutputDirectory(FsPath outdir)
        {
            Console.WriteLine("Creating output directory...");
            outdir.CreateDir();
        }

        private static void CreateNavigationJson(FsPath outdir, List<string> files)
        {
            Console.WriteLine("Generating navigation.json...");
            var navigationJson = outdir.Combine("navigation.json");
            var navigationList = new List<string>(files.Count);
            foreach (var file in files)
            {
                var f = outdir.Combine(Path.ChangeExtension(file, ".html")).ToString();
                navigationList.Add(f);
            }
            navigationJson.WriteFile(JsonConvert.SerializeObject(navigationList));
        }

        public void Run(Config currentConfig)
        {
            var outdir = currentConfig.OutputDir.ToPath();
            var indir = new FsPath(Environment.CurrentDirectory);
            var imgdir = currentConfig.ImageDir.ToPath();
            var toc = currentConfig.TOCFile.ToPath();
            var files = MarkdownUtils.GetFilesToProcess(toc.ReadFile());

            CreateOutputDirectory(outdir);
            CopyImages(outdir, imgdir);
            CreateNavigationJson(outdir, files);

            var content = new Dictionary<string, string>();
            content.Add("toc", "");
            content.Add("content", "");
            var tocContent = MarkdownUtils.Markdown2HTML(toc.ReadFile());
            foreach (var file in files)
            {
                tocContent = tocContent.Replace(file, currentConfig.HostName+Path.ChangeExtension(file, ".html"));
            }
            content["toc"] = tocContent;

            Console.WriteLine("Generating Sub Markdown Files...");
            Template template = new Template(new FsPath("app://Resources/Template.html"));
            foreach (var file in files)
            {
                var input = indir.Combine(file);
                var output = outdir.Combine(Path.ChangeExtension(file, ".html"));

                content["content"] = MarkdownUtils.Markdown2HTML(input.ReadFile());
                var html = template.ProcessTemplate(content);
                output.WriteFile(html);
            }
        }
    }
}
