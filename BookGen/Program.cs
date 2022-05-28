//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Domain;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Modules;
using BookGen.Modules.Special;
using BookGen.Utilities;
using System.Runtime.InteropServices;

namespace BookGen
{
    internal static class Program
    {
        #region Internal API
#pragma warning disable CS8618 
        // Non-nullable field must contain a non-null value when exiting constructor.
        // Consider declaring as nullable.
        internal static ProgramState CurrentState
        {
            get;
            private set;
        }
#pragma warning restore CS8618
        internal static AppSetting AppSetting { get; private set; } = new AppSetting();

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
            Environment.Exit((int)exitCode);
        }
        #endregion

        private static readonly ModuleBase[] StatelessModules = new ModuleBase[]
        {
            new ConfigHelpModule(),
            new VersionModule(),
            new HelpModule(),
            new SubCommandsModule(),
            new ShellModule(),
            new WikiModule(),
        };

        internal static ModuleWithState[] CreateModules()
        {
            return new ModuleWithState[]
            {
                new BuildModule(CurrentState),
                new GuiModule(CurrentState),
                new AssemblyDocumentModule(CurrentState),
                new SettingsModule(CurrentState, AppSetting),
                new InitModule(CurrentState),
                new Md2HtmlModule(CurrentState),
                new ChaptersModule(CurrentState),
                new StatModule(CurrentState),
                new EditModule(CurrentState, AppSetting),
                new PreviewModule(CurrentState),
                new ServeModule(CurrentState),
                new UpdateModule(CurrentState),
                new ImgConvertModule(CurrentState),
                new StockSearchModule(CurrentState),
                new MdTableModule(CurrentState),
                new ExternalLinksModule(CurrentState),
                new TagsModule(CurrentState),
            };
        }

        private static readonly List<ModuleWithState> ModulesWithState = new List<ModuleWithState>();

        private static SupportedOs GetCurrentOs()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return SupportedOs.Windows;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return SupportedOs.Linux;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return SupportedOs.OsX;
            else
                return SupportedOs.None;
        }

        internal static void RunModule(string moduleName,
                                       IReadOnlyList<string> parameters,
                                       bool skipLockCheck = false)
        {
            ModuleBase? moduleToRun = null;
            try
            {
                moduleToRun = GetModuleToRun(StatelessModules, ModulesWithState, moduleName);

                if (moduleToRun == null)
                {
                    Console.WriteLine(HelpUtils.GetGeneralHelp());
                    Cleanup(moduleToRun);
                    Exit(ExitCode.UnknownCommand);
                    return;
                }

                ExitCode exitCode = ExitCode.Succes;

                if (moduleToRun is ModuleWithState moduleWithState)
                {
                    moduleWithState.ShouldSkipLockCheck = skipLockCheck;
                    moduleToRun = moduleWithState;
                }

                if (!moduleToRun.SupportedOs.HasFlag(GetCurrentOs()))
                {
                    CurrentState.Log.Warning("this subcommand is only available on windows");
                    exitCode = ExitCode.PlatformNotSupported;
                }
                else
                {
                    var result = moduleToRun.Execute(parameters.ToArray());
                    switch (result)
                    {
                        case ModuleRunResult.ArgumentsError:
                            Console.WriteLine(moduleToRun.GetHelp());
                            Cleanup(moduleToRun);
                            exitCode = ExitCode.BadParameters;
                            break;
                        case ModuleRunResult.GeneralError:
                            exitCode = ExitCode.Exception;
                            break;
                        case ModuleRunResult.Succes:
                        default:
                            exitCode = ExitCode.Succes;
                            break;
                    }
                }

                Cleanup(moduleToRun);
                Exit(exitCode);
            }
            catch (Exception ex)
            {
                HandleUncaughtException(moduleToRun, ex);
            }
        }


        public static void Main(string[] args)
        {
            var loaded = AppSettingHandler.LoadAppSettings();
            var argumentsToParse = args.ToList();

            if (loaded != null)
            {
                AppSetting = loaded;
            }

            string module = SubcommandParser.GetCommand(argumentsToParse);
            ProgramConfigurator.WaitForDebugger(argumentsToParse);
            CurrentState = ProgramConfigurator.ConfigureState(argumentsToParse);

            ModulesWithState.AddRange(CreateModules());
            ConfiugreStatelessModules(ModulesWithState);


            RunModule(module, argumentsToParse);
            CurrentState.Log.Flush();
        }

        private static void Cleanup(ModuleBase? moduleToRun)
        {
            if (moduleToRun is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        private static void ConfiugreStatelessModules(IEnumerable<ModuleWithState> modulesWithState)
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

            CurrentState.Log.Critical(ex);

            ShowMessageBox("Unhandled exception\r\n{0}", ex.Message);
#if DEBUG
            System.Diagnostics.Debugger.Break();
#endif

            CurrentState.Log.Flush();
            Exit(ExitCode.Exception);
        }

    }
}
