//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Framework;
using System;
using System.Diagnostics;

namespace BookGen
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                ILog Log = new Logger();
                var argumentParser = new GeneratorRunner(args, Log);
                argumentParser.RunGenerator();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception");
                Console.WriteLine(ex);
#if DEBUG
                Debugger.Break();
#endif
            }
        }
    }
}
