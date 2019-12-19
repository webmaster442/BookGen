using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BookGen
{
    internal class ProgramState
    {
        public bool Gui { get; set; }
        public bool NoWaitForExit { get; set; }
        public GeneratorRunner? GeneratorRunner { get; set; }
        public Version ProgramVersion { get; }
        public int ConfigVersion { get; }

        public ProgramState()
        {
            var asm = Assembly.GetAssembly(typeof(ProgramState));
            ProgramVersion = asm?.GetName()?.Version ?? new Version(1, 0);
            ConfigVersion = (ProgramVersion.Major * 1000) + (ProgramVersion.Minor * 100) + ProgramVersion.Build;
        }

    }
}
