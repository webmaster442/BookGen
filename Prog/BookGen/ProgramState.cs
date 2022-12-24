//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Infrastructure;
using System.Reflection;
using Webmaster442.HttpServerFramework;

namespace BookGen
{
    internal sealed class ProgramState
    {
        public bool Gui { get; set; }
        public bool NoWaitForExit { get; set; }
        public Version ProgramVersion { get; }
        public DateTime BuildDateUtc { get; }
        public string ProgramDirectory { get; }
        public int ConfigVersion { get; }
        public IModuleApi Api { get; }
        public ILog Log { get; }
        public IServerLog ServerLog { get; }

        private static DateTime GetProgramDate()
        {
            var current = Assembly.GetAssembly(typeof(ProgramState));
            if (current != null)
            {
                AssemblyBuildDateAttribute? attribute = current.GetCustomAttribute<AssemblyBuildDateAttribute>();
                if (attribute != null)
                {
                    return attribute.BuildDateUtc;
                }
            }
            return new DateTime();
        }

        public ProgramState(IModuleApi apiImplementation, ILog log, IServerLog serverLog)
        {
            Log = log;
            ServerLog = serverLog;
            Api = apiImplementation;
            var asm = Assembly.GetAssembly(typeof(ProgramState));
            ProgramVersion = asm?.GetName()?.Version ?? new Version(1, 0);
            ConfigVersion = (ProgramVersion.Major * 1000) + (ProgramVersion.Minor * 100) + ProgramVersion.Build;
            BuildDateUtc = GetProgramDate();
            ProgramDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
        }

    }
}
