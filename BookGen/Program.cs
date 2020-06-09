//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Domain;
using BookGen.Modules;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen
{
    internal static class Program
    {
        internal static ProgramState CurrentState { get; } = new ProgramState();
        internal static AppSetting AppSetting { get; private set; } = new AppSetting();

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
                Console.Read();
            }
        }

        private static void Exit(ExitCode exitCode)
        {
            Environment.Exit((int)exitCode);
        }

        private static List<StateModuleBase> LoadModules()
        {
            return new List<StateModuleBase>
            {
                new BuildModule(CurrentState),
                new ConfigHelpModule(CurrentState),
                new GuiModule(CurrentState),
                new UpdateModule(CurrentState),
                new EditorModule(CurrentState),
                new AssemblyDocumentModule(CurrentState),
                new SettingsModule(CurrentState, AppSetting),
                new InitModule(CurrentState),
                new PagegenModule(CurrentState),
                new VersionModule(CurrentState),
            };
        }

        public static void Main(string[] args)
        {
            StateModuleBase? currentModule = null;
            try
            {
                AppSetting = AppSettingHandler.LoadAppSettings();
                var arguments = new ArgumentParser(args);
                List<StateModuleBase> modules = LoadModules();

                string command = arguments.GetValues().FirstOrDefault() ?? string.Empty;

                if (string.Compare(command, "SubCommands", true) == 0)
                {
                    PrintModules(modules);
                    Exit(ExitCode.Succes);
                }
                else if (string.Compare(command, "Help", true) == 0)
                {
                    var helpScope = arguments.GetValues().Skip(1).FirstOrDefault();

                    currentModule =
                        modules.Find(m => string.Compare(m.ModuleCommand, helpScope, true) == 0);

                    PrintGeneralHelpAndExitIfModuleNull(currentModule);

                    GetHelpForModuleAndExit(currentModule);
                }

                currentModule = 
                    modules.Find(m => string.Compare(m.ModuleCommand, command, true) == 0);

                PrintGeneralHelpAndExitIfModuleNull(currentModule);

                if (currentModule?.Execute(arguments) == false)
                {
                    GetHelpForModuleAndExit(currentModule);
                }

                Exit(ExitCode.Succes);
            }
            catch (Exception ex)
            {
                HandleUncaughtException(currentModule, ex);
            }
        }

        private static void GetHelpForModuleAndExit(StateModuleBase? module)
        {
            Console.WriteLine(module?.GetHelp());
            Exit(ExitCode.BadParameters);
        }

        private static void PrintGeneralHelpAndExitIfModuleNull(StateModuleBase? currentModule)
        {
            if (currentModule == null)
            {
                Console.WriteLine(HelpUtils.GetGeneralHelp());
                Exit(ExitCode.UnknownCommand);
            }
        }

        private static void PrintModules(IEnumerable<StateModuleBase> modules)
        {
            Console.WriteLine("Available sub commands: \r\n");
            foreach (var module in modules)
            {
                Console.WriteLine(module.ModuleCommand);
            }
        }

        private static void HandleUncaughtException(StateModuleBase? currentModule, Exception ex)
        {
            currentModule?.Abort();
            ShowMessageBox("Unhandled exception\r\n{0}", ex);
#if DEBUG
            System.Diagnostics.Debugger.Break();
#endif
            Exit(ExitCode.Exception);
        }
    }
}
