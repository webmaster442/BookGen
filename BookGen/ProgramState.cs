//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Framework;
using System;
using System.Reflection;
using Webmaster442.HttpServerFramework;

namespace BookGen
{
    internal class ProgramState
    {
        private readonly ConsoleLog _log;
        public bool Gui { get; set; }
        public bool NoWaitForExit { get; set; }
        public GeneratorRunner? GeneratorRunner { get; set; }
        public Version ProgramVersion { get; }
        public DateTime BuildDate { get; }
        public string ProgramDirectory { get; }
        public int ConfigVersion { get; }


#if TESTBUILD
        public ILog Log { get; set; }
        public IServerLog ServerLog { get; set; }
#else

        public ILog Log => _log;
        public IServerLog ServerLog => _log;
#endif

        private static DateTime GetProgramDate()
        {
            Assembly? current = Assembly.GetAssembly(typeof(ProgramState));
            if (current != null)
            {
                var attribute = current.GetCustomAttribute<AssemblyBuildDateAttribute>();
                if (attribute != null)
                {
                    return attribute.BuildDate;
                }
            }
            return new DateTime();
        }

        public ProgramState()
        {
            var asm = Assembly.GetAssembly(typeof(ProgramState));
            ProgramVersion = asm?.GetName()?.Version ?? new Version(1, 0);
            ConfigVersion = (ProgramVersion.Major * 1000) + (ProgramVersion.Minor * 100) + ProgramVersion.Build;
            BuildDate = GetProgramDate();
            ProgramDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
            _log = new ConsoleLog();
#if TESTBUILD
            var l = new ConsoleLog();
            Log = l;
            ServerLog = l;
#endif
        }

    }
}
