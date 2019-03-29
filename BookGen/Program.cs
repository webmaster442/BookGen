//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Framework;
using BookGen.Utilities;
using System;

namespace BookGen
{
    internal static class Program
    {
        public static ILog Log { get; private set; }

        [STAThread]
        public static void Main(string[] args)
        {
            using (Log = new Logger())
            {
                var argumentParser = new GeneratorRunner(args, Log);

                NativeMethods.ShowConsole();
                argumentParser.RunGenerator();
            }
        }
    }
}
