//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Framework;
using System;

namespace BookGen
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using (var Log = new Logger())
            {
                var argumentParser = new GeneratorRunner(args, Log);
                argumentParser.RunGenerator();
            }
        }
    }
}
