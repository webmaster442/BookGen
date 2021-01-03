//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Domain;
using BookGen.Modules;
using BookGen.Modules.Special;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;
using System;
using System.Linq;

namespace BookGen
{
    internal static class Program
    {
        #region Internal API

        internal static ProgramState CurrentState { get; private set; } = new ProgramState();
        internal static AppSetting AppSetting { get; private set; } = new AppSetting();

        internal static bool IsTesting { get; set; }
        internal static string ErrorText { get; set; } = "";
        internal static bool ErrorHappened { get; set; } = false;

        public static GeneratorRunner CreateRunner(bool verbose, string workDir)
        {
            CurrentState.Log.LogLevel = verbose ? LogLevel.Detail : LogLevel.Info;
            return new GeneratorRunner(CurrentState.Log, workDir);
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
            if (IsTesting && exitCode != ExitCode.Succes)
            {
                ErrorText = exitCode.ToString();
                ErrorHappened = true;
            }
            else
            {
                Environment.Exit((int)exitCode);
            }
        }
        #endregion

        public static readonly StateModuleBase[] ModulesWithState = new StateModuleBase[]
        {
            new BuildModule(CurrentState),
            new ConfigHelpModule(CurrentState),
            new GuiModule(CurrentState),
            new EditorModule(CurrentState),
            new AssemblyDocumentModule(CurrentState),
            new SettingsModule(CurrentState, AppSetting),
            new InitModule(CurrentState),
            new PagegenModule(CurrentState),
            new Md2HtmlModule(CurrentState),
            new VersionModule(CurrentState),
            new ChaptersModule(CurrentState),
        };

        private static readonly BaseModule[] StatelessModules = new BaseModule[]
        {
            new HelpModule(),
            new SubCommandsModule()
        };


        public static void Main(string[] args)
        {
            BaseModule? moduleToRun = null;
            try
            {
                ConfiugreStatelessModules();

                var loaded = AppSettingHandler.LoadAppSettings();

                if (loaded != null)
                {
                    AppSetting = loaded;
                }

                string command = SubcommandParser.GetCommand(args, out string[] parameters);

                DebugHelper.WaitForDebugger(parameters);

                moduleToRun = GetModuleToRun(command);

                if (moduleToRun == null)
                {
                    Console.WriteLine(HelpUtils.GetGeneralHelp());
                    Exit(ExitCode.UnknownCommand);
                    return;
                }

                if (moduleToRun.Execute(parameters) == false)
                {
                    Console.WriteLine(moduleToRun?.GetHelp());
                    Exit(ExitCode.BadParameters);
                }

            }
            catch (Exception ex)
            {
                if (IsTesting)
                {
                    ErrorHappened = true;
                    ErrorText = ex.Message;
                    return;
                }
                else
                {
                    HandleUncaughtException(moduleToRun, ex);
                }
            }
        }

        private static void ConfiugreStatelessModules()
        {
            foreach (var module in StatelessModules)
            {
                if (module is IModuleCollection moduleCollection)
                    moduleCollection.Modules = ModulesWithState;
            }
        }

        private static BaseModule? GetModuleToRun(string command)
        {
            BaseModule? stateless = StatelessModules.FirstOrDefault(m => string.Compare(m.ModuleCommand, command, true) == 0);
            if (stateless != null)
                return stateless;

            BaseModule? stated = ModulesWithState.FirstOrDefault(m => string.Compare(m.ModuleCommand, command, true) == 0);
            if (stated != null)
                return stated;

            return null;
        }

        private static void HandleUncaughtException(BaseModule? currentModule, Exception ex)
        {
            if (currentModule is StateModuleBase stateModule)
                stateModule?.Abort();

            ShowMessageBox("Unhandled exception\r\n{0}", ex);
#if DEBUG
            System.Diagnostics.Debugger.Break();
#endif
            Exit(ExitCode.Exception);
        }

    }
}
