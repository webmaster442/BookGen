// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using Newtonsoft.Json;

namespace BookGen
{
    internal static class InitializerMethods
    {
        public static void DoCreateConfig(ILog log, FsPath ConfigFile)
        {
            log.Info("Creating default config file: {0}", ConfigFile.ToString());
            var def = JsonConvert.SerializeObject(Config.CreateDefault(), Formatting.Indented);
            ConfigFile.WriteFile(def);
        }
    }
}
