//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Data;
using System.Diagnostics;
using System.Threading;

using BookGen.Framework;
using BookGen.Gui;

namespace BookGen.Infrastructure;

internal static class ProgramConfigurator
{
    private const string DebuggerShort = "-wd";
    private const string DebuggerLong = "--wait-debugger";
    private const string DebuggerStartShort = "-ad";
    private const string DebuggerStartLong = "--attach-debugger";

    private const string JsonLogShort = "-js";
    private const string JsonLogLong = "--json-log";
    private const string LogFileShort = "-lf";
    private const string LogFileLong = "--log-file";

    public static IEnumerable<string> GeneralArguments
    {
        get
        {
            yield return DebuggerShort;
            yield return DebuggerLong;
            yield return DebuggerStartShort;
            yield return DebuggerStartLong;
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

    public static void AttachDebugger(IList<string> arguments)
    {
        if (GetSwitch(arguments, DebuggerStartShort, DebuggerStartLong))
        {
            Console.WriteLine("Attaching debugger...");
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
        }
    }

    internal static ILogger ConfigureLog(IList<string> arguments)
    {
        if (GetSwitch(arguments, JsonLogShort, JsonLogLong))
        {
            return new JsonLog();
        }

        bool logFile = GetSwitch(arguments, LogFileShort, LogFileLong);
        return new TerminalLog(logFile);
    }
}
