//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework;
using BookGen.Utilities;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace BookGen
{
    internal class Program
    {
        private static Config _cfg;
        private static FsPath _menu;
        public static Logger Log { get; private set; }

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
            Version version = GetVersion();
            Console.WriteLine("BookGen V{0} Starting...", version);
            Console.WriteLine("Working directory: {0}", Environment.CurrentDirectory);
            Console.WriteLine("---------------------------------------------------------\n\n");
            int cfgVersion = (version.Major * 100) + version.Minor;

            ArgumentParser argumentParser = new ArgumentParser(args);
            argumentParser.CreateMenuJson += ArgumentParser_CreateMenuJson;
            argumentParser.BuildTestWebsite += ArgumentParser_BuildTestWebsite;
            argumentParser.BuildWebsite += ArgumentParser_BuildWebsite;
            argumentParser.BuildPrintHtml += ArgumentParser_BuildPrintHtml;

            if (argumentParser.IsLogEnabled)
            {
                LogConfigure.ConfigureNlog();
            }
            Log = LogManager.GetCurrentClassLogger();

            FsPath config = new FsPath(Environment.CurrentDirectory, "bookgen.json");
            _menu = new FsPath(Environment.CurrentDirectory, "menuitems.json");
            Log.Info("Working directory: {0}", Environment.CurrentDirectory);


            if (!config.IsExisting)
            {
                Log.Warn("Config not found, Default config created");
                Console.WriteLine("No bookgen.json config found. Creating one");
                Console.WriteLine("Review configuration then run program again");
                WriteConfig(config, Config.Default);
                return;
            }

            Log.Info("Configuration content: {0}", config.ReadFile());

            _cfg = ReadConfig(config);
            if (!_cfg.ValidateConfig()) return;
            if (_cfg.Version < cfgVersion)
            {
                _cfg.UpgradeTo(cfgVersion);
                WriteConfig(config, _cfg);
                Console.WriteLine("Configuration file migrated to new version.");
                Console.WriteLine("Review configuration then run program again");
                Log.Warn("Configuration updated to new version. Review needed");
                return;
            }

            argumentParser.RunArgumentSteps();

#if DEBUG
            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
#endif
        }

        private static void ArgumentParser_CreateMenuJson(object sender, EventArgs e)
        {
            WriteMenu(_menu, new List<HeaderMenuItem> { HeaderMenuItem.Default });
        }

        private static void ArgumentParser_BuildWebsite(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            WebsiteBuilder builder = new WebsiteBuilder(_cfg, _menu);
            Build(start, builder);
        }

        private static void ArgumentParser_BuildTestWebsite(object sender, EventArgs e)
        {
            Log.Info("Test build started");
            DateTime start = DateTime.Now;
            Console.WriteLine("Building test configuration...");
            _cfg.HostName = "http://localhost:8080/";
            WebsiteBuilder builder = new WebsiteBuilder(_cfg, _menu);
            Build(start, builder);
            Console.WriteLine("Test server running on: http://localhost:8080/");
            SimpleHTTPServer server = new SimpleHTTPServer(_cfg.OutputDir, 8080);
            Process.Start(_cfg.HostName);
        }

        private static void ArgumentParser_BuildPrintHtml(object sender, EventArgs e)
        {
            Log.Info("Printable HTML build started");
            DateTime start = DateTime.Now;
            PrintBuilder builder = new PrintBuilder(_cfg);
            Build(start, builder);
        }

        private static void Build(DateTime start, Generator builder)
        {
            Log.Info("Website build started");
            builder.Run();
            Console.Write("Finished ");
            var runTime = DateTime.Now - start;
            runTime.LogToConsole();
        }
    }
}
