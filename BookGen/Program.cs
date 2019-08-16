//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Framework;
using BookGen.Gui;
using System;
using System.Diagnostics;

namespace BookGen
{
    internal static class Program
    {
        internal static GeneratorRunner Runner { get; private set; }
        internal static bool IsInGuiMode { get; private set; }
        internal static ConsoleMenu UI { get; private set; }

        public static void ShowMessageBox(string text, params object[] args)
        {
            Console.WriteLine(text, args);
            if (!IsInGuiMode)
            {
                Console.ReadKey();
            }
        }

        public static void ShowMessageAndWait(string text)
        {
            Console.WriteLine(text);
            Console.ReadLine();
        }

        private static ParsedOptions ParseOptions(string[] args)
        {
            ParsedOptions parsed = new ParsedOptions
            {
                WorkingDirectory = Environment.CurrentDirectory,
                GuiReqested = false,
                ShowHelp = true,
                VerboseLog = false,
                Action = null
            };

            ArgsumentList arguments = ArgsumentList.Parse(args);

            var dir = arguments.GetArgument("d", "dir");

            if (dir != null)
                parsed.WorkingDirectory = dir.Value;

            var gui = arguments.GetArgument("g", "gui");

            if (gui?.HasSwitch == true)
            {
                parsed.ShowHelp = false;
                parsed.GuiReqested = true;
                IsInGuiMode = true;
            }

            var verbose = arguments.GetArgument("v", "verbose");

            if (verbose?.HasSwitch == true)
            {
                parsed.VerboseLog = true;
            }

            var action = arguments.GetArgument("a", "action");
            if (action != null)
            {
                bool succes = Enum.TryParse(action.Value, true, out ParsedOptions.ActionType genAction);
                if (succes)
                {
                    parsed.ShowHelp = false;
                    parsed.Action = genAction;
                }
            }

            return parsed;
        }

        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                IsInGuiMode = false;
                LogLevel logLevel = LogLevel.Info;

                ParsedOptions options = ParseOptions(args);

                if (options.VerboseLog)
                    logLevel = LogLevel.Detail;

                var Consolelog = new ConsoleLog(logLevel);

                if (options.GuiReqested)
                {
                    Runner = new GeneratorRunner(Consolelog, options.WorkingDirectory);
                    UI = new ConsoleMenu(Runner);
                    UI.Run();
                }
                else
                {
                    Runner = new GeneratorRunner(Consolelog, options.WorkingDirectory);

                    switch (options.Action)
                    {
                        case ParsedOptions.ActionType.BuildWeb:
                            if (Runner.Initialize())
                            {
                                Runner.DoBuild();
                            }
                            break;
                        case ParsedOptions.ActionType.Clean:
                            if (Runner.Initialize())
                            {
                                Runner.DoClean();
                            }
                            break;
                        case ParsedOptions.ActionType.Test:
                            if (Runner.Initialize())
                            {
                                Runner.DoTest();
                            }
                            break;
                        case ParsedOptions.ActionType.BuildPrint:
                            if (Runner.Initialize())
                            {
                                Runner.DoPrint();
                            }
                            break;
                        case ParsedOptions.ActionType.Initialize:
                            Runner.DoInteractiveInitialize();
                            break;
                        case ParsedOptions.ActionType.ValidateConfig:
                            Runner.Initialize();
                            break;
                        case ParsedOptions.ActionType.BuildEpub:
                            if (Runner.Initialize())
                            {
                                Runner.DoEpub();
                            }
                            break;
                        default:
                            Runner.RunHelp();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShouldRun = false;

                Console.Clear();
                ShowMessageBox("Unhandled exception\r\n{0}", ex);
#if DEBUG
                Debugger.Break();
#endif
            }
        }
    }
}
