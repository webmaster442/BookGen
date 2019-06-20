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
        private FsPath _config;

        public Config Configuration { get; private set; }

        private string _workdir;

        public string WorkDir
        {
            get { return _workdir; }
            set
            {
                _workdir = value;
                _config = new FsPath(_workdir, "bookgen.json");
            }
        }

        public GeneratorRunner(ILog log)
        {
            _log = log;
        }

        public void RunHelp()
        {
            _log.Info(Properties.Resources.Help);
#if DEBUG
            Console.ReadKey();
#endif
            Environment.Exit(1);
        }

        #region Helpers
        public bool Initialize()
        {
            Version version = GetVersion();
            Splash.DoSplash();
            _log.Info("BookGen V{0} Starting...", version);
            _log.Info("Working directory: {0}", _workdir);
            _log.Info("---------------------------------------------------------\n\n");
            int cfgVersion = (version.Major * 100) + version.Minor;

            if (!_config.IsExisting)
            {
                _log.Info("No bookgen.json config found.");
                PressKeyToExit();
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
                PressKeyToExit();
                return false;
            }

            ConfigValidator validator = new ConfigValidator(Configuration, _workdir);
            validator.Validate();
            
            if (!validator.IsValid)
            {
                _log.Warning("Errors found in configuration: ");
                foreach (var error in validator.Errors)
                {
                    _log.Warning(error);
                }
                PressKeyToExit();
                return false;
            }
            else
            {
                var tocFile = new FsPath(_workdir).Combine(Configuration.TOCFile);
                var toc = MarkdownUtils.ParseToc(tocFile.ReadFile());
                TocValidator tocValidator = new TocValidator(toc, _workdir);
                tocValidator.Validate();
                if (!tocValidator.IsValid)
                {
                    _log.Warning("Errors found in TOC file: ");
                    foreach (var error in tocValidator.Errors)
                    {
                        _log.Warning(error);
                    }
                    PressKeyToExit();
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

        public static void PressKeyToExit()
        {
            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
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
            WebsiteBuilder builder = new WebsiteBuilder(_workdir, Configuration, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
        }

        public void DoPrint()
        {
            _log.Info("Building print configuration...");
            PrintBuilder builder = new PrintBuilder(_workdir, Configuration, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
        }

        public void DoEpub()
        {
            _log.Info("Building epub configuration...");
            EpubBuilder builder = new EpubBuilder(_workdir, Configuration, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
        }


        public void DoTest()
        {
            _log.Info("Building test configuration...");
            Configuration.HostName = "http://localhost:8080/";
            WebsiteBuilder builder = new WebsiteBuilder(_workdir, Configuration, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
            using (var server = new HTTPTestServer(Path.Combine(_workdir, Configuration.OutputDir), 8080, _log))
            {
                Console.Clear();
                _log.Info("Test server running on: http://localhost:8080/");
                _log.Info("Serving from: {0}", Configuration.OutputDir);
                Process.Start(Configuration.HostName);
                Splash.PressKeyToExit();
            }
        }

        #endregion
    }
}
