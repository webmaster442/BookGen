//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Domain;
using BookGen.Framework;
using BookGen.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace BookGen
{
    internal class Program
    {
        private static Config _cfg;
        private static FsPath _menu;
        public static ILog Log { get; private set; }

        private static Config ReadConfig(FsPath config)
        {
            return JsonConvert.DeserializeObject<Config>(config.ReadFile());
        }

        private static void WriteMenu(FsPath menu, List<HeaderMenuItem> menuitems)
        {
            var def = JsonConvert.SerializeObject(menuitems, Formatting.Indented);
            menu.WriteFile(def);
        }

        private static void WriteConfig(FsPath configFile, Config configuration)
        {
            var def = JsonConvert.SerializeObject(configuration, Formatting.Indented);
            configFile.WriteFile(def);
        }

        private static Version GetVersion()
        {
            var asm = Assembly.GetAssembly(typeof(Program));
            return asm.GetName().Version;
        }

        public static void Main(string[] args)
        {
            ArgumentParser argumentParser = new ArgumentParser(args);
            argumentParser.CreateMenuJson += ArgumentParser_CreateMenuJson;
            argumentParser.BuildTestWebsite += ArgumentParser_BuildTestWebsite;
            argumentParser.BuildWebsite += ArgumentParser_BuildWebsite;
            argumentParser.BuildPrintHtml += ArgumentParser_BuildPrintHtml;

            Log = new Logger();
            if (argumentParser.IsLogEnabled)
            {
                var logfile = Path.Combine(Environment.CurrentDirectory, "boogken.log");
                Log.ConfigureFile(logfile);
            }

            Version version = GetVersion();
            Splash.DoSplash();
            Log.Info("BookGen V{0} Starting...", version);
            Log.Info("Working directory: {0}", Environment.CurrentDirectory);
            Log.Info("---------------------------------------------------------\n\n");
            int cfgVersion = (version.Major * 100) + version.Minor;

            FsPath config = new FsPath(Environment.CurrentDirectory, "bookgen.json");
            _menu = new FsPath(Environment.CurrentDirectory, "menuitems.json");

            if (!config.IsExisting)
            {
                Log.Info("No bookgen.json config found. Creating one");
                Log.Info("Review configuration then run program again");
                WriteConfig(config, Config.Default);
                return;
            }

            Log.Detail("Configuration content: {0}", config.ReadFile());

            _cfg = ReadConfig(config);
            if (!_cfg.ValidateConfig()) return;
            if (_cfg.Version < cfgVersion)
            {
                _cfg.UpgradeTo(cfgVersion);
                WriteConfig(config, _cfg);
                Log.Info("Configuration file migrated to new version.");
                Log.Info("Review configuration then run program again");
                return;
            }

            argumentParser.RunArgumentSteps();
        }

        private static void ArgumentParser_CreateMenuJson(object sender, EventArgs e)
        {
            WriteMenu(_menu, new List<HeaderMenuItem> { HeaderMenuItem.Default });
        }

        private static void ArgumentParser_BuildWebsite(object sender, EventArgs e)
        {
            Log.Info("Building deploy configuration...");
            DateTime start = DateTime.Now;
            WebsiteBuilder builder = new WebsiteBuilder(_cfg, _menu);
            Build(start, builder);
        }

        private static void ArgumentParser_BuildTestWebsite(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            Log.Info("Building test configuration...");
            _cfg.HostName = "http://localhost:8080/";
            WebsiteBuilder builder = new WebsiteBuilder(_cfg, _menu);
            Build(start, builder);
            using (var server = new SimpleHTTPServer(_cfg.OutputDir, 8080, Log))
            {
                Console.Clear();
                Log.Info("Test server running on: http://localhost:8080/");
                Process.Start(_cfg.HostName);
                Splash.PressKeyToExit();
            }
            Log.Dispose();
        }

        private static void ArgumentParser_BuildPrintHtml(object sender, EventArgs e)
        {
            Log.Info("Building print configuration...");
            DateTime start = DateTime.Now;
            PrintBuilder builder = new PrintBuilder(_cfg);
            Build(start, builder);
        }

        private static void Build(DateTime start, Generator builder)
        {
            builder.Run();
            var runTime = DateTime.Now - start;
            runTime.LogToConsole();
        }
    }
}
