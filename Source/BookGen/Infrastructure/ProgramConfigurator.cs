//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using Spectre.Console;

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
    private const string NoRuntimeShort = "-nr";
    private const string NoRuntimeLong = "--no-runtime";

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

    public static List<string> ParseGeneralArgs(string[] args, ProgramInfo info)
    {
        List<string> results = new List<string>(args.Length);
        var removeIndexes = new HashSet<int>();

        AttachDebugger(removeIndexes, args);
        WaitForDebugger(removeIndexes, args);
        ConfigureLog(removeIndexes, info, args);
        ConfigureRuntimePrinting(removeIndexes, info, args);

        for (int i = 0; i < args.Length; i++)
        {
            if (!removeIndexes.Contains(i))
            {
                results.Add(args[i]);
            }
        }
        return results;
    }

    private static bool GetSwitch(HashSet<int> state, IReadOnlyList<string> args, string shortName, string longName)
    {
        bool found = false;
        for (int i = 0; i < args.Count; i++)
        {
            if (args[i] == shortName || args[i] == longName)
            {
                state.Add(i);
                found = true;
            }
        }
        return found;
    }


    private static void AttachDebugger(HashSet<int> removeIndexes, IReadOnlyList<string> arguments)
    {
        if (GetSwitch(removeIndexes, arguments, DebuggerStartShort, DebuggerStartLong))
        {
            AnsiConsole.WriteLine("Attaching debugger...");
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
        }
    }

    private static void WaitForDebugger(HashSet<int> removeIndexes, IReadOnlyList<string> arguments)
    {
        if (GetSwitch(removeIndexes, arguments, DebuggerShort, DebuggerLong))
        {
            AnsiConsole.WriteLine("Waiting for debugger to be attached...");
            AnsiConsole.WriteLine("ESC to cancel & contine execution...");
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

    private static void ConfigureLog(HashSet<int> removeIndexes, ProgramInfo info, IReadOnlyList<string> arguments)
    {
        if (GetSwitch(removeIndexes, arguments, JsonLogShort, JsonLogLong))
        {
            info.JsonLogging = true;
        }

        info.LogToFile = GetSwitch(removeIndexes, arguments, LogFileShort, LogFileLong);
    }

    private static void ConfigureRuntimePrinting(HashSet<int> removeIndexes, ProgramInfo info, IReadOnlyList<string> arguments)
    {
        if (GetSwitch(removeIndexes, arguments, NoRuntimeShort, NoRuntimeLong))
        {
            info.PrintRuntime = false;
            return;
        }
        info.PrintRuntime = true;
    }
}
