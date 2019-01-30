//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;
using System;
using System.IO;
using Newtonsoft.Json;

namespace BookGen
{
    class Program
    {
        private static Config ReadConfig(FsPath config)
        {
            return JsonConvert.DeserializeObject<Config>(config.ReadFile());
        }

        private static void CreateDefaultConfig(FsPath config)
        {
            var def = JsonConvert.SerializeObject(Config.Default, Formatting.Indented);
            config.WriteFile(def);
        }

        public static void Main(string[] args)
        {
            typeof(Program).LogVersion();

            FsPath config = new FsPath(Environment.CurrentDirectory, "bookgen.json");

            if (config.IsExisting)
            {
                try
                {
                    DateTime start = DateTime.Now;
                    WebsiteBuilder builder = new WebsiteBuilder();

                    var cfg = ReadConfig(config);
                    if (ConfigValidator.ValidateConfig(cfg))
                    {
                        if (args.Length > 0 && args[0] == "test")
                            Test(cfg);
                        else
                            Build(start, builder, cfg);

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
                CreateDefaultConfig(config);
            }

            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
        }

        private static void Test(Config config)
        {
            config.HostName = "http://localhost:8080/";
            Console.WriteLine("Test server running on: http://localhost:8080/");
            SimpleHTTPServer server = new SimpleHTTPServer(config.OutputDir, 8080);
        }

        private static void Build(DateTime start, WebsiteBuilder builder, Config cfg)
        {
            builder.Run(cfg);
            Console.Write("Finished ");
            var runTime = DateTime.Now - start;
            runTime.LogToConsole();
        }
    }
}
