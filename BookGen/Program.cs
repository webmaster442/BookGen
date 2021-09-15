//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Domain;
using BookGen.Framework;
using BookGen.Modules;
using BookGen.Modules.Special;
using BookGen.Gui.ArgumentParser;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen
{
    internal static class Program
    {
        #region Internal API

        internal static ProgramState CurrentState { get; } = new ProgramState();
        internal static AppSetting AppSetting { get; private set; } = new AppSetting();

#if TESTBUILD
        internal static bool IsTesting { get; set; }
        internal static string ErrorText { get; set; } = "";
        internal static bool ErrorHappened { get; set; } = false;
#endif

        public static GeneratorRunner CreateRunner(bool verbose, string workDir)
        {
            CurrentState.Log.LogLevel = verbose ? LogLevel.Detail : LogLevel.Info;
            return new GeneratorRunner(CurrentState.Log, CurrentState.ServerLog, workDir);
        }

        public static void ShowMessageBox(string text, params object[] args)
        {
            Console.WriteLine(text, args);
            if (!CurrentState.Gui && !CurrentState.NoWaitForExit)
            {
                Console.Read();
            }
        }

        public static void Exit(ExitCode exitCode)
        {
#if TESTBUILD
            if (IsTesting && exitCode != ExitCode.Succes)
            {
                ErrorText = exitCode.ToString();
                ErrorHappened = true;
            }
            else
            {
#endif
                Environment.Exit((int)exitCode);
#if TESTBUILD
        }
#endif
        }
        #endregion

        private static readonly ModuleBase[] StatelessModules = new ModuleBase[]
        {
            new ConfigHelpModule(),
            new VersionModule(),
            new HelpModule(),
            new SubCommandsModule(),
            new ShellModule(),
        };


        public static void Main(string[] args)
        {
            ModuleBase? moduleToRun = null;
            try
            {
                var loaded = AppSettingHandler.LoadAppSettings();

                if (loaded != null)
                {
                    AppSetting = loaded;
                }

                var modulesWithState = CreateModules();
                ConfiugreStatelessModules(modulesWithState);


                string command = SubcommandParser.GetCommand(args, out string[] parameters);

                DebugHelper.WaitForDebugger(parameters);

                moduleToRun = GetModuleToRun(StatelessModules, modulesWithState, command);

                if (moduleToRun == null)
                {
                    Console.WriteLine(HelpUtils.GetGeneralHelp());
                    Cleanup(moduleToRun);
                    Exit(ExitCode.UnknownCommand);
                    return;
                }

                if (!moduleToRun.Execute(parameters))
                {
                    Console.WriteLine(moduleToRun.GetHelp());
                    Cleanup(moduleToRun);
                    Exit(ExitCode.BadParameters);
                }

                Cleanup(moduleToRun);
            }
            catch (Exception ex)
            {
#if TESTBUILD
                if (IsTesting)
                {
                    ErrorHappened = true;
                    ErrorText = ex.Message;
                    return;
                }
                else
                {
#endif
                    HandleUncaughtException(moduleToRun, ex);

#if TESTBUILD
                }
#endif
            }
        }

        private static void Cleanup(ModuleBase? moduleToRun)
        {
            if (moduleToRun is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        public static ModuleWithState[] CreateModules()
        {
            return new ModuleWithState[]
            {
                new BuildModule(CurrentState),
                new GuiModule(CurrentState),
                new AssemblyDocumentModule(CurrentState),
                new SettingsModule(CurrentState, AppSetting),
                new InitModule(CurrentState),
                new PagegenModule(CurrentState),
                new Md2HtmlModule(CurrentState),
                new ChaptersModule(CurrentState),
                new StatModule(CurrentState),
                new EditModule(CurrentState, AppSetting),
                new PreviewModule(CurrentState),
                new ServeModule(CurrentState),
                new UpdateModule(CurrentState),
                new ImgConvertModule(CurrentState),
            };
        }

        private static void ConfiugreStatelessModules(ModuleWithState[] modulesWithState)
        {
            IEnumerable<ModuleBase>? allmodules = StatelessModules.Concat(modulesWithState);
            foreach (var module in allmodules)
            {
                if (module is IModuleCollection moduleCollection)
                    moduleCollection.Modules = allmodules;
            }
        }

        private static ModuleBase? GetModuleToRun(IEnumerable<ModuleBase> statelessModules,
                                                  IEnumerable<ModuleBase> modulesWithState,
                                                  string command)
        {
            ModuleBase? stateless = statelessModules.FirstOrDefault(m => string.Compare(m.ModuleCommand, command, true) == 0);
            if (stateless != null)
                return stateless;

            ModuleBase? stated = modulesWithState.FirstOrDefault(m => string.Compare(m.ModuleCommand, command, true) == 0);
            if (stated != null)
                return stated;

            return null;
        }

        private static void HandleUncaughtException(ModuleBase? currentModule, Exception ex)
        {
            if (currentModule is ModuleWithState stateModule)
                stateModule.Abort();

            ShowMessageBox("Unhandled exception\r\n{0}", ex);
#if DEBUG
            System.Diagnostics.Debugger.Break();
#endif
            Exit(ExitCode.Exception);
        }

    }
}
