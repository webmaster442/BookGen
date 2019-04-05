//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework.Server;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace BookGen
{
    internal class GeneratorRunner
    {
        private readonly string[] _args;
        private readonly ILog _log;
        private FsPath _config;
        private Config _cfg;

        public GeneratorRunner(string[] arguments, ILog log)
        {
            _args = arguments;
            _log = log;
            var logfile = Path.Combine(Environment.CurrentDirectory, "bookgen.log");
            _log.ConfigureFile(logfile);
            _config = new FsPath(Environment.CurrentDirectory, "bookgen.json");

        }

        public void RunGenerator()
        {
            switch (_args[0])
            {
                case "build":
                    if (!Initialize()) return;
                    DoBuild();
                    break;
                case "test":
                    if (!Initialize()) return;
                    DoTest();
                    break;
                case "print":
                    if (!Initialize()) return;
                    DoPrint();
                    break;
                case "createconfig":
                    DoCreateConfig();
                    break;
                default:
                    DoUnknownArgs();
                    break;
            }
        }

        #region Helpers
        private bool Initialize()
        {
            Version version = GetVersion();
            Splash.DoSplash();
            _log.Info("BookGen V{0} Starting...", version);
            _log.Info("Working directory: {0}", Environment.CurrentDirectory);
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

            _cfg = JsonConvert.DeserializeObject<Config>(cfgstring);
            if (!_cfg.ValidateConfig())
            {
                PressKeyToExit();
                return false;
            }

            if (_cfg.Version < cfgVersion)
            {
                _cfg.UpgradeTo(cfgVersion);
                WriteConfig(_config, _cfg);
                _log.Info("Configuration file migrated to new version.");
                _log.Info("Review configuration then run program again");
                PressKeyToExit();
                return false;
            }

            return true;
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

        private static void PressKeyToExit()
        {
            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
        }
        #endregion

        #region Argument handlers

        private void DoCreateConfig()
        {
            _log.Info("Creating default config file: {0}", _config.ToString());
            WriteConfig(_config, Config.Default);
        }


        private void DoUnknownArgs()
        {
            Console.WriteLine("usage: bookgen.exe [command] [--log]");
        }

        private void DoBuild()
        {
            _log.Info("Building deploy configuration...");
            WebsiteBuilder builder = new WebsiteBuilder(_cfg);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
        }

        private void DoPrint()
        {
            _log.Info("Building print configuration...");
            PrintBuilder builder = new PrintBuilder(_cfg);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
        }

        private void DoTest()
        {
            _log.Info("Building test configuration...");
            _cfg.HostName = "http://localhost:8080/";
            WebsiteBuilder builder = new WebsiteBuilder(_cfg);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
            using (var server = new HTTPTestServer(_cfg.OutputDir, 8080, _log))
            {
                Console.Clear();
                _log.Info("Test server running on: http://localhost:8080/");
                _log.Info("Serving from: {0}", _cfg.OutputDir);
                Process.Start(_cfg.HostName);
                Splash.PressKeyToExit();
            }
        }
        #endregion
    }
}
