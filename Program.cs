//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework;
using BookGen.Utilities;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Reflection;

namespace BookGen
{
    class Program
    {
        private static Config ReadConfig(FsPath config)
        {
            return JsonConvert.DeserializeObject<Config>(config.ReadFile());
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

            FsPath config = new FsPath(Environment.CurrentDirectory, "bookgen.json");

            if (config.IsExisting)
            {
                try
                {
                    DateTime start = DateTime.Now;

                    var cfg = ReadConfig(config);
                    if (cfg.ValidateConfig())
                    {
                        if (cfg.Version < cfgVersion)
                        {
                            cfg.UpgradeTo(cfgVersion);
                            WriteConfig(config, cfg);
                            Console.WriteLine("Configuration file migrated to new version.");
                            Console.WriteLine("Review configuration then run program again");
                        }
                        else
                        {
                            if (args.Length > 0 && args[0] == "test")
                            {
                                Console.WriteLine("Building test configuration...");
                                cfg.HostName = "http://localhost:8080/";
                                WebsiteBuilder builder = new WebsiteBuilder(cfg);
                                Build(start, builder, cfg);
                                Console.WriteLine("Test server running on: http://localhost:8080/");
                                SimpleHTTPServer server = new SimpleHTTPServer(cfg.OutputDir, 8080);
                                Process.Start(cfg.HostName);
                            }
                            else
                            {
                                WebsiteBuilder builder = new WebsiteBuilder(cfg);
                                Build(start, builder, cfg);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    ex.LogToConsole();
                }
            }
            else
            {
                Console.WriteLine("No bookgen.json config found. Creating one");
                Console.WriteLine("Review configuration then run program again");
                WriteConfig(config, Config.Default);
            }

            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
        }

        private static void Build(DateTime start, WebsiteBuilder builder, Config cfg)
        {
            builder.Run();
            Console.Write("Finished ");
            var runTime = DateTime.Now - start;
            runTime.LogToConsole();
        }
    }
}
