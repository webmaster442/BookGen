//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Framework;
using BookGen.GeneratorSteps;
using BookGen.Gui;
using System;
using System.Diagnostics;

namespace BookGen
{
    internal static class Program
    {
        internal static GeneratorRunner Runner { get; private set; }

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
                    Runner = new GeneratorRunner(log);
                    ConsoleGui ui = new ConsoleGui(log, Runner);
                    ui.Run();
                }
                else
                {
                    var Consolelog = new ConsoleLog();
                    Runner = new GeneratorRunner(Consolelog);

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
                                CreateOutputDirectory.CleanDirectory(new FsPath(Runner.Configuration.OutputDir), Consolelog);
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
                            GeneratorRunner.PressKeyToExit();
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
                Console.WriteLine("Unhandled exception");
                Console.WriteLine(ex);
#if DEBUG
                Debugger.Break();
#endif
            }
        }
    }
}
