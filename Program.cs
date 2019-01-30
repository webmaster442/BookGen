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
            var def = JsonConvert.SerializeObject(Config.Default);
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
                        builder.Run(cfg);
                        Console.Write("Finished ");
                        var runTime = DateTime.Now - start;
                        runTime.LogToConsole();
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
        }
    }
}
