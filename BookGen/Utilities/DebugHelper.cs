//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using System;
using System.Diagnostics;
using System.Threading;

namespace BookGen.Utilities
{
    internal static class DebugHelper
    {
        public static void WaitForDebugger(ArgumentParser argumentParser)
        {
            if (argumentParser.GetSwitch("-wd", "--wait-debugger"))
            {
                Console.WriteLine("Waiting for debugger to be attached...");
                Console.WriteLine("ESC to cancel & contine execution...");
                while (!Debugger.IsAttached)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                        {
                            break;
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
    }
}
