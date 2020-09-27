//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Domain;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BookGen.Modules
{
    internal class ChaptersModule : StateModuleBase
    {
        public ChaptersModule(ProgramState currentState) : base(currentState)
        {
        }

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem("Chapters",
                                            "-a",
                                            "--action",
                                            "Scan",
                                            "GenSummary");
            }
        }

        public override string ModuleCommand => "Chapters";

        public override bool Execute(string[] arguments)
        {
            ChaptersParameters args = new ChaptersParameters();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            var log = new ConsoleLog(LogLevel.Info);

            var loader = new ProjectLoader(log, args.WorkDir);

            if (!loader.TryLoadAndValidateConfig(out Config? configuration)
                || configuration == null)
            {
                return false;
            }

            switch (args.Action)
            {
                case ChaptersAction.GenSummary:
                    return GenerateSummaryFile(args.WorkDir, configuration, log);
                case ChaptersAction.Scan:
                    ScanMarkdownFiles(args.WorkDir, configuration, log);
                    break;
            }

            return true;
        }

        private List<string> SetFiles(string[] files, string dir, string tOCFile)
        {
            List<string> result = new List<string>(files.Length);
            foreach (var file in files)
            {
                if (!file.Contains(tOCFile))
                    result.Add(file.Replace(dir, ""));
            }
            return result;
        }

        private void ScanMarkdownFiles(string workDir, Config configuration, ILog log)
        {
            log.Info("Scanning markdown files...");
            FsPath destination = new FsPath(workDir, ".chapters");

            List<Chapter> chapters = new List<Chapter>(10);

            string[] dirs = Directory.GetDirectories(workDir);
            string[] root = Directory.GetFiles(workDir, "*.md");

            chapters.Add(new Chapter
            {
                Title = "Root",
                Files = SetFiles(root, workDir, configuration.TOCFile)
            });

            foreach (var dir in dirs)
            {
                string[] files = Directory.GetFiles(dir, "*.md", SearchOption.AllDirectories);
                chapters.Add(new Chapter
                {
                    Title = Path.GetFileName(dir),
                    Files = SetFiles(files, dir, configuration.TOCFile)
                });
            }

            log.Info("Writing .chapters file...");
            ChapterSerializer.WriteToFile(destination, chapters);
        }

        private bool GenerateSummaryFile(string workDir, Config configuration, ILog log)
        {
            FsPath source = new FsPath(workDir, ".chapters");
            if (source.IsExisting)
            {
                log.Info(".chapters file doesn't exist.");
                return false;
            }

            StringBuilder buffer = new StringBuilder();

            List<Chapter> chapters = ChapterSerializer.ReadFromFile(source).ToList();
            foreach (var chapter in chapters)
            {
                buffer.AppendFormat("## {0}\r\n", chapter.Title);
                foreach (var file in chapter.Files)
                {
                    FsPath path = new FsPath(workDir, file);
                    string content = path.ReadFile(log);
                    string subtitle = MarkdownUtils.GetTitle(content);
                    buffer.AppendFormat("* [{0}]({1})", subtitle, file);
                }
                buffer.AppendLine();
            }

            FsPath destination = new FsPath(workDir, configuration.TOCFile);
            if (destination.IsExisting)
            {
                destination.CreateBackup(log);
            }
            destination.WriteFile(log, buffer.ToString());

            return true;
        }

        public override string GetHelp()
        {
            throw new NotImplementedException();
        }
    }
}
