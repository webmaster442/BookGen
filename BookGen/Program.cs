//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Framework;
using BookGen.Gui;
using System;
using System.Diagnostics;
using Terminal.Gui;

namespace BookGen
{
    internal static class Program
    {
        internal static GeneratorRunner Runner { get; private set; }

        internal static bool IsInGuiMode
        {
            get { return Application.Top.Running; }
        }

        public static void ShowMessageBox(string text, params object[] args)
        {
            if (IsInGuiMode)
            {
                string msg = string.Format(text, args);
                MessageBox.Query(80, 10, "Message", msg, "Ok");
            }
            else
            {
                Console.WriteLine(text, args);
                Console.ReadKey();
            }
        }


        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                ArgsumentList arguments = ArgsumentList.Parse(args);

                var action = arguments.GetArgument("a", "action");
                var gui = arguments.GetArgument("g", "gui");
                var dir = arguments.GetArgument("d", "dir")?.Value;

                if (dir == null) dir = Environment.CurrentDirectory;

                if (gui?.HasSwitch == true)
                {
                    var log = new EventedLog();
                    Runner = new GeneratorRunner(log, dir);
                    ConsoleGui ui = new ConsoleGui(log, Runner);
                    ui.Run();
                }
                else
                {
                    var Consolelog = new ConsoleLog();
                    Runner = new GeneratorRunner(Consolelog, dir);

                    switch (action?.Value)
                    {
                        case KnownArguments.BuildWeb:
                            if (Runner.Initialize())
                            {
                                Runner.DoBuild();
                            };
                            break;
                        case KnownArguments.Clean:
                            if (Runner.Initialize())
                            {
                                Runner.DoClean();
                            }
                            break;
                        case KnownArguments.TestWeb:
                            if (Runner.Initialize())
                            {
                                Runner.DoTest();
                            }
                            break;
                        case KnownArguments.BuildPrint:
                            if (Runner.Initialize())
                            {
                                Runner.DoPrint();
                            }
                            break;
                        case KnownArguments.CreateConfig:
                            Runner.DoCreateConfig();
                            break;
                        case KnownArguments.ValidateConfig:
                            Runner.Initialize();
                            break;
                        case KnownArguments.BuildEpub:
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
                Application.Top.Running = false;
                Console.Clear();
                ShowMessageBox("Unhandled exception\r\n{0}", ex);
#if DEBUG
                Debugger.Break();
#endif
            }
        }
    }
}
