//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Domain;
using BookGen.Mdoules;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen
{
    internal static class Program
    {
        private const int ExitSucces = 0;
        private const int ExitException = -1;
        private const int ExitUnknownCommand = 1;
        private const int ExitBadParameters = 2;

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

        private static List<ModuleBase> LoadModules()
        {
            return new List<ModuleBase>
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
            ModuleBase? currentModule = null;
            try
            {
                AppSetting = AppSettingHandler.LoadAppSettings();
                var arguments = new ArgumentParser(args);
                List<ModuleBase> modules = LoadModules();

                string command = arguments.GetValues().FirstOrDefault() ?? string.Empty;

                if (string.Compare(command, "SubCommands", true) == 0)
                {
                    PrintModules(modules);
                    Environment.Exit(ExitSucces);
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

                Environment.Exit(ExitSucces);
            }
            catch (Exception ex)
            {
                HandleUncaughtException(currentModule, ex);
            }
        }

        private static void GetHelpForModuleAndExit(ModuleBase? module)
        {
            Console.WriteLine(module?.GetHelp());
            Environment.Exit(ExitBadParameters);
        }

        private static void PrintGeneralHelpAndExitIfModuleNull(ModuleBase? currentModule)
        {
            if (currentModule == null)
            {
                Console.WriteLine(HelpUtils.GetGeneralHelp());
                Environment.Exit(ExitUnknownCommand);
            }
        }

        private static void PrintModules(IEnumerable<ModuleBase> modules)
        {
            Console.WriteLine("Available sub commands: \r\n");
            foreach (var module in modules)
            {
                Console.WriteLine(module.ModuleCommand);
            }
        }

        private static void HandleUncaughtException(ModuleBase? currentModule, Exception ex)
        {
            currentModule?.Abort();
            ShowMessageBox("Unhandled exception\r\n{0}", ex);
#if DEBUG
            System.Diagnostics.Debugger.Break();
#endif
            Environment.Exit(ExitException);
        }
    }
}
