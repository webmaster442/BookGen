//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework.Server;
using BookGen.GeneratorSteps;
using BookGen.Gui;
using BookGen.Help;
using BookGen.Utilities;
using System;
using System.Diagnostics;
using System.IO;

namespace BookGen
{
    internal class GeneratorRunner
    {
        private readonly ILog _log;

        private const string exitString = "Press a key to exit...";

        public Config Configuration { get; private set; }

        public FsPath ConfigFile { get; private set; }

        public string WorkDirectory
        {
            get;
        }

        public GeneratorRunner(ILog log, string workDir)
        {
            _log = log;
            WorkDirectory = workDir;
            ConfigFile = new FsPath(WorkDirectory, "bookgen.json");
        }

        internal void DoInteractiveInitialize()
        {
            var menu = new InteractiveInitializer(_log, new FsPath(WorkDirectory));
            menu.Run();
        }

        public void RunHelp(bool exits = true)
        {
            _log.Info(HelpTextCreator.GenerateHelpText());

            if (exits)
            {
#if DEBUG
                Program.ShowMessageBox("Press a key to continue");
#endif
                Environment.Exit(1);
            }
        }

        #region Helpers
        public bool Initialize()
        {
            _log.Info("---------------------------------------------------------");
            _log.Info("BookGen V{0} Starting...", Program.ProgramVersion);
            _log.Info("Working directory: {0}", WorkDirectory);
            _log.Info("---------------------------------------------------------");

            if (!ConfigFile.IsExisting)
            {
                _log.Info("No bookgen.json config found.");
                Program.ShowMessageBox(exitString);
                return false;
            }

            Configuration = ConfigFile.DeserializeJson<Config>(_log);

            if (Configuration.Version < Program.ConfigVersion)
            {
                ConfigFile.CreateBackup(_log);
                Configuration.UpgradeTo(Program.ConfigVersion);
                ConfigFile.SerializeJson(Configuration, _log, true);
                _log.Info("Configuration file migrated to new version.");
                _log.Info("Review configuration then run program again");
                Program.ShowMessageBox(exitString);
                return false;
            }

            ConfigValidator validator = new ConfigValidator(Configuration, WorkDirectory);
            validator.Validate();

            if (!validator.IsValid)
            {
                _log.Warning("Errors found in configuration: ");
                foreach (var error in validator.Errors)
                {
                    _log.Warning(error);
                }
                Program.ShowMessageBox(exitString);
                return false;
            }
            else
            {
                var tocFile = new FsPath(WorkDirectory).Combine(Configuration.TOCFile);
                _log.Info("Parsing TOC file...");
                var toc = MarkdownUtils.ParseToc(tocFile.ReadFile(_log));
                _log.Info("Found {0} chapters and {1} files", toc.ChapterCount, toc.FilesCount);
                TocValidator tocValidator = new TocValidator(toc, WorkDirectory);
                tocValidator.Validate();
                if (!tocValidator.IsValid)
                {
                    _log.Warning("Errors found in TOC file: ");
                    foreach (var error in tocValidator.Errors)
                    {
                        _log.Warning(error);
                    }
                    Program.ShowMessageBox(exitString);
                    return false;
                }
                else
                {
                    _log.Info("Config file contains no errors");
                }
            }

            return true;
        }

        public void DoClean()
        {
            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.TargetWeb.OutPutDirectory), _log);
            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.TargetPrint.OutPutDirectory), _log);
            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.TargetEpub.OutPutDirectory), _log);
            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.TargetWordpress.OutPutDirectory), _log);
        }
        #endregion

        #region Argument handlers

        public void DoBuild()
        {
            _log.Info("Building deploy configuration...");
            WebsiteBuilder builder = new WebsiteBuilder(WorkDirectory, Configuration, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
        }

        public void DoPrint()
        {
            _log.Info("Building print configuration...");
            PrintBuilder builder = new PrintBuilder(WorkDirectory, Configuration, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
        }

        public void DoEpub()
        {
            _log.Info("Building epub configuration...");
            EpubBuilder builder = new EpubBuilder(WorkDirectory, Configuration, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
        }

        public void DoWordpress()
        {
            _log.Info("Building Wordpress configuration...");
            WordpressBuilder builder = new WordpressBuilder(WorkDirectory, Configuration, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
        }

        public void DoTest()
        {
            _log.Info("Building test configuration...");
            Configuration.HostName = "http://localhost:8080/";
            WebsiteBuilder builder = new WebsiteBuilder(WorkDirectory, Configuration, _log);
            var runTime = builder.Run();
            
            using (var server = new HttpTestServer(Path.Combine(WorkDirectory, Configuration.TargetWeb.OutPutDirectory), 8080, _log))
            {
                _log.Info("-------------------------------------------------");
                _log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
                _log.Info("Test server running on: http://localhost:8080/");
                _log.Info("Serving from: {0}", Configuration.TargetWeb.OutPutDirectory);

                Process p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = Configuration.HostName;
                p.Start();
                Program.ShowMessageAndWait(exitString);
            }
        }

        #endregion
    }
}
