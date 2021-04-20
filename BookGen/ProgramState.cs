//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using System;
using System.Reflection;

namespace BookGen
{
    internal class ProgramState
    {
        public bool Gui { get; set; }
        public bool NoWaitForExit { get; set; }
        public GeneratorRunner? GeneratorRunner { get; set; }
        public Version ProgramVersion { get; }
        public DateTime BuildDate { get; }
        public string ProgramDirectory { get; }
        public int ConfigVersion { get; }
        public ILog Log { get; internal set; }

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
            Log = new ConsoleLog();
        }

    }
}
