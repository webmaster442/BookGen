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
using System.Reflection;

namespace BookGen
{
    internal static class Program
    {
        internal static GeneratorRunner Runner { get; private set; }
        internal static bool IsInGuiMode { get; private set; }
        internal static ConsoleMenu UI { get; private set; }
        internal static bool NoWaitForExit { get; private set; }

        internal static Version ProgramVersion { get; private set; }
        internal static int ConfigVersion { get; private set; }


        public static void ShowMessageBox(string text, params object[] args)
        {
            Console.WriteLine(text, args);
            if (!IsInGuiMode && !NoWaitForExit)
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
                Action = null,
            };

            ArgumentList arguments = ArgumentList.Parse(args);

            var dir = arguments.GetArgument("d", "dir");

            if (dir != null)
                parsed.WorkingDirectory = dir.Value;

            var verbose = arguments.GetArgument("v", "verbose");

            if (verbose?.HasSwitch == true)
            {
                parsed.VerboseLog = true;
            }

            var gui = arguments.GetArgument("g", "gui");
            if (gui?.HasSwitch == true)
            {
                parsed.ShowHelp = false;
                parsed.GuiReqested = true;
                IsInGuiMode = true;

                return parsed;
            }

            var nowait = arguments.GetArgument("n", "nowait");
            if (nowait != null)
            {
                NoWaitForExit = true;
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
                var asm = Assembly.GetAssembly(typeof(Program));
                ProgramVersion = asm.GetName().Version;
                ConfigVersion = (ProgramVersion.Major * 1000) + (ProgramVersion.Minor * 100) + ProgramVersion.Build;

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
                        case ParsedOptions.ActionType.BuildWordpress:
                            if (Runner.Initialize())
                            {
                                Runner.DoWordpress();
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
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                if (UI != null)
                    UI.ShouldRun = false;

                Console.Clear();
                ShowMessageBox("Unhandled exception\r\n{0}", ex);
#if DEBUG
                Debugger.Break();
#endif
                Environment.Exit(-1);
            }
        }
    }
}
