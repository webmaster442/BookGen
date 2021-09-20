//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace BookGen.Utilities
{
    internal static class DebugHelper
    {
        public static void WaitForDebugger(ref string[] args)
        {
            if (GetSwitch(args, "-wd", "--wait-debugger"))
            {
                args = args.Where(x => x != "-wd" && x != "--wait-debugger").ToArray();

                Console.WriteLine("Waiting for debugger to be attached...");
                Console.WriteLine("ESC to cancel & contine execution...");
                while (!Debugger.IsAttached)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                        {
                            return;
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                Debugger.Break();
                //Now you can debug the code
            }
        }

        private static bool GetSwitch(string[] inputs, string shortName, string longName)
        {
            return inputs.Any(i => i == shortName || i == longName);
        }
    }
}
