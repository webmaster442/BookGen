//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework.Server;
using BookGen.GeneratorSteps;
using BookGen.Utilities;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace BookGen
{
    internal class GeneratorRunner
    {
        private readonly ILog _log;
        private readonly FsPath _config;
        private const string exitString = "Press a key to exit...";

        public Config Configuration { get; private set; }

        public string WorkDirectory
        {
            get;
        }

        public GeneratorRunner(ILog log, string workDir)
        {
            _log = log;
            WorkDirectory = workDir;
            _config = new FsPath(WorkDirectory, "bookgen.json");
        }

        public void RunHelp()
        {
            _log.Info(Properties.Resources.Help);
            if (!Program.IsInGuiMode)
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
            Version version = GetVersion();
            _log.Info("---------------------------------------------------------\n\n");
            _log.Info("BookGen V{0} Starting...", version);
            _log.Info("Working directory: {0}", WorkDirectory);
            _log.Info("---------------------------------------------------------\n\n");
            int cfgVersion = (version.Major * 100) + version.Minor;

            if (!_config.IsExisting)
            {
                _log.Info("No bookgen.json config found.");
                Program.ShowMessageBox(exitString);
                return false;
            }

            var cfgstring = _config.ReadFile();

            _log.Detail("Configuration content: {0}", cfgstring);

            Configuration = JsonConvert.DeserializeObject<Config>(cfgstring);

            if (Configuration.Version < cfgVersion)
            {
                Configuration.UpgradeTo(cfgVersion);
                WriteConfig(_config, Configuration);
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
                var toc = MarkdownUtils.ParseToc(tocFile.ReadFile());
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
            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.OutputDir), _log);
        }

        private static Version GetVersion()
        {
            var asm = Assembly.GetAssembly(typeof(Program));
            return asm.GetName().Version;
        }

        private static void WriteConfig(FsPath configFile, Config configuration)
        {
            var def = JsonConvert.SerializeObject(configuration, Formatting.Indented);
            configFile.WriteFile(def);
        }
        #endregion

        #region Argument handlers

        public void DoCreateConfig()
        {
            _log.Info("Creating default config file: {0}", _config.ToString());
            WriteConfig(_config, Config.CreateDefault());
        }

        public void DoBuild()
        {
            _log.Info("Building deploy configuration...");
            WebsiteBuilder builder = new WebsiteBuilder(WorkDirectory, Configuration, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
        }

        public void DoPrint()
        {
            _log.Info("Building print configuration...");
            PrintBuilder builder = new PrintBuilder(WorkDirectory, Configuration, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
        }

        public void DoEpub()
        {
            _log.Info("Building epub configuration...");
            EpubBuilder builder = new EpubBuilder(WorkDirectory, Configuration, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
        }

        public void DoTest()
        {
            _log.Info("Building test configuration...");
            Configuration.HostName = "http://localhost:8080/";
            WebsiteBuilder builder = new WebsiteBuilder(WorkDirectory, Configuration, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
            using (var server = new HttpTestServer(Path.Combine(WorkDirectory, Configuration.OutputDir), 8080, _log))
            {
                Console.Clear();
                _log.Info("Test server running on: http://localhost:8080/");
                _log.Info("Serving from: {0}", Configuration.OutputDir);

                Process p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = Configuration.HostName;
                p.Start();
                Program.ShowMessageBox(exitString);
            }
        }

        #endregion
    }
}
