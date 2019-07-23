//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Framework;
using BookGen.Gui;
using System;
using System.Diagnostics;

namespace BookGen
{
    internal static class Program
    {
        internal static GeneratorRunner Runner { get; private set; }

        public static void ShowMessageBox(string text, params object[] args)
        {
            Console.WriteLine(text, args);
            Console.ReadKey();
        }

        private static ParsedOptions ParseOptions(string[] args)
        {
            ParsedOptions parsed = new ParsedOptions
            {
                WorkingDirectory = Environment.CurrentDirectory,
                GuiReqested = false,
                ShowHelp = true,
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
                ParsedOptions options = ParseOptions(args);
                var Consolelog = new ConsoleLog();

                if (options.GuiReqested)
                {
                    Runner = new GeneratorRunner(Consolelog, options.WorkingDirectory);
                    ConsoleGui ui = new ConsoleGui(Runner);
                    ui.Run();
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
                        case ParsedOptions.ActionType.CreateConfig:
                            Runner.DoCreateConfig();
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
                ConsoleGui.ShouldRun = false;

                Console.Clear();
                ShowMessageBox("Unhandled exception\r\n{0}", ex);
#if DEBUG
                Debugger.Break();
#endif
            }
        }
    }
}
