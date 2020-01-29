//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Mdoules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen
{
    internal static class Program
    {
        internal static ProgramState CurrentState { get; } = new ProgramState();

        public static GeneratorRunner CreateRunner(bool verbose, string workDir)
        {
            LogLevel logLevel = verbose ? LogLevel.Detail : LogLevel.Info;
            var log = new ConsoleLog(logLevel);
            return new GeneratorRunner(log, workDir);
        }

        public static void ShowMessageBox(string text, params object[] args)
        {
            Console.WriteLine(text, args);
            if (!CurrentState.Gui && !CurrentState.NoWaitForExit)
            {
                Console.ReadKey();
            }
        }

        private static List<ModuleBase> LoadModules()
        {
            return new List<ModuleBase>
            {
                new BuildModule(CurrentState),
                new ConfigHelpModule(CurrentState),
                new GuiModule(CurrentState),
                new HelpModule(CurrentState),
                new UpdateModule(CurrentState),
                new EditorModule(CurrentState),
                new AssemblyDocumentModule(CurrentState),
            };
        }

        public static void Main(string[] args)
        {
            ModuleBase? currentModule = null;
            try
            {
                ArgumentParser arguments = new ArgumentParser(args);
                List<ModuleBase> modules = LoadModules();

                string command = arguments.GetValues().FirstOrDefault() ?? string.Empty;

                currentModule = 
                    modules.FirstOrDefault(m => string.Compare(m.ModuleCommand, command, true) == 0) 
                    ?? new HelpModule(CurrentState);

                if (currentModule?.Execute(arguments) == false)
                {
                    currentModule = new HelpModule(CurrentState);
                    currentModule.Execute(arguments);
                }

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                HandleUncaughtException(currentModule, ex);
            }
        }

        private static void HandleUncaughtException(ModuleBase? currentModule, Exception ex)
        {
            currentModule?.Abort();
            Console.Clear();
            ShowMessageBox("Unhandled exception\r\n{0}", ex);
#if DEBUG
            System.Diagnostics.Debugger.Break();
#endif
            Environment.Exit(-1);
        }
    }
}
