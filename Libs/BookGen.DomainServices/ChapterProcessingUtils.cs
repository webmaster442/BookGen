//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain;
using BookGen.Domain.Configuration;
using BookGen.Interfaces;
using System.Text;

namespace BookGen.DomainServices
{
    internal static class ChapterProcessingUtils
    {
        public static void ScanMarkdownFiles(string workDir, Config configuration, ILog log)
        {
            log.Info("Scanning markdown files...");
            var destination = new FsPath(workDir, ".chapters");

            var chapters = new List<Chapter>(10);

            string[] dirs = Directory.GetDirectories(workDir);
            string[] root = Directory.GetFiles(workDir, "*.md");

            chapters.Add(new Chapter
            {
                Title = "Root",
                Files = SetFiles(root, workDir, configuration.TOCFile)
            });

            foreach (string? dir in dirs)
            {
                string[] files = Directory.GetFiles(dir, "*.md", SearchOption.AllDirectories);
                chapters.Add(new Chapter
                {
                    Title = Path.GetFileName(dir),
                    Files = SetFiles(files, dir, configuration.TOCFile)
                });
            }

            log.Info("Writing .chapters file...");

            destination.SerializeYaml(chapters, log);
        }

        public static bool GenerateSummaryFile(string workDir, Config configuration, ILog log)
        {
            var source = new FsPath(workDir, ".chapters");
            if (source.IsExisting)
            {
                log.Info(".chapters file doesn't exist.");
                return false;
            }

            var buffer = new StringBuilder();

            List<Chapter>? chapters = source.DeserializeYaml<List<Chapter>>(log);

            if (chapters == null)
            {
                log.Detail(".chapters file reading failed. Bad markup?");
                return false;
            }

            ConvertChaptersToMarkdown(workDir, log, buffer, chapters);

            var destination = new FsPath(workDir, configuration.TOCFile);
            if (destination.IsExisting)
            {
                destination.CreateBackup(log);
            }
            destination.WriteFile(log, buffer.ToString());

            return true;
        }

        private static void ConvertChaptersToMarkdown(string workDir, ILog log, StringBuilder buffer, List<Chapter> chapters)
        {
            if (chapters == null)
                return;

            foreach (Chapter? chapter in chapters)
            {
                buffer.AppendFormat("## {0}\r\n", chapter.Title);
                foreach (string? file in chapter.Files)
                {
                    var path = new FsPath(workDir, file);
                    string content = path.ReadFile(log);
                    string subtitle = MarkdownUtils.GetDocumentTitle(content, log);
                    buffer.AppendFormat("* [{0}]({1})", subtitle, file);
                }
                buffer.AppendLine();
            }
        }

        private static List<string> SetFiles(string[] files, string dir, string tOCFile)
        {
            var result = new List<string>(files.Length);
            foreach (string? file in files)
            {
                if (!file.Contains(tOCFile))
                    result.Add(file.Replace(dir, ""));
            }
            return result;
        }

    }
}
