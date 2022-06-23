//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using System.Diagnostics;
using System.Threading;


namespace BookGen.Infrastructure
{
    internal static class ProgramConfigurator
    {
        private const string DebuggerShort = "-wd";
        private const string DebuggerLong = "--wait-debugger";
        private const string JsonLogShort = "-js";
        private const string JsonLogLong = "--json-log";
        private const string LogFileShort = "-lf";
        private const string LogFileLong = "--log-file";
        private const string NoWait = "-nw";

        public static IEnumerable<string> GeneralArguments
        {
            get
            {
                yield return DebuggerShort;
                yield return DebuggerLong;
                yield return JsonLogShort;
                yield return JsonLogLong;
            }
        }

        private static bool GetSwitch(IList<string> inputs, string shortName, string longName)
        {
            bool result = false;
            var removeIndexes = new Stack<int>();
            for (int i = 0; i < inputs.Count; i++)
            {
                if (inputs[i] == shortName
                    || inputs[i] == longName)
                {
                    removeIndexes.Push(i);
                    result = true;
                }
            }

            while (removeIndexes.Count > 0)
            {
                inputs.RemoveAt(removeIndexes.Pop());
            }

            return result;
        }

        public static void WaitForDebugger(IList<string> arguments)
        {
            if (GetSwitch(arguments, DebuggerShort, DebuggerLong))
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

        internal static ProgramState ConfigureState(List<string> arguments)
        {
            ProgramState state;

            if (GetSwitch(arguments, JsonLogShort, JsonLogLong))
            {
                var log = new JsonLog();
                state = new ProgramState(new ModuleApi(), log, log);
            }
            else
            {
                bool logFile = GetSwitch(arguments, LogFileShort, LogFileLong);
                var log = new ConsoleLog(logFile);
                state = new ProgramState(new ModuleApi(), log, log);
            }
            state.NoWaitForExit = GetSwitch(arguments, NoWait, NoWait);
            return state;
        }
    }
}
