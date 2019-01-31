//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace BookGen
{
    internal class WebsiteBuilder
    {
        private static void CopyImages(FsPath outdir, FsPath imgdir)
        {
            Console.WriteLine("Copy images to output...");
            imgdir.CopyDirectory(outdir.Combine(imgdir.GetName()));
        }

        private void CopyAssets(FsPath assets, FsPath outdir)
        {
            if (assets.IsExisting)
            {
                Console.WriteLine("Copy template assets to output...");
                assets.CopyDirectory(outdir.Combine(assets.GetName()));
            }
        }

        private static void CreateOutputDirectory(FsPath outdir)
        {
            Console.WriteLine("Creating output directory...");
            outdir.CreateDir();
        }

        public void Run(Config currentConfig)
        {
            var outdir = currentConfig.OutputDir.ToPath();
            var indir = new FsPath(Environment.CurrentDirectory);
            var imgdir = currentConfig.ImageDir.ToPath();
            var toc = currentConfig.TOCFile.ToPath();
            var files = MarkdownUtils.GetFilesToProcess(toc.ReadFile());

            CreateOutputDirectory(outdir);
            CopyAssets(indir.Combine(currentConfig.AssetsDir), outdir);
            CopyImages(outdir, imgdir);

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
            Template template = new Template(currentConfig.Template.ToPath());
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
