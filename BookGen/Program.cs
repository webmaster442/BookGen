using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Domain.ArgumentParsing;
using BookGen.Gui;
using BookGen.Help;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGen
{
    internal static class Program
    {
        internal static ProgramState CurrentState { get; } = new ProgramState();
        private static ConsoleMenu? _UI;

        public static void Main(string[] args)
        {
            try
            {
                var argumentHandler = new ProgramArgumentHandler(args);
                argumentHandler.DoBuild += ArgumentHandler_DoBuild;
                argumentHandler.DoGui += ArgumentHandler_DoGui;
                argumentHandler.DoHelp += ArgumentHandler_DoHelp;
                argumentHandler.DoConfigHelp += ArgumentHandler_DoConfigHelp;
                argumentHandler.DoUpdate += ArgumentHandler_DoUpdate;
                argumentHandler.Parse();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                HandleUncaughtException(ex);
            }
        }

        private static GeneratorRunner CreateRunner(bool verbose, string workDir)
        {
            LogLevel logLevel = verbose ? LogLevel.Detail : LogLevel.Info;
            var log = new ConsoleLog(logLevel);
            return new GeneratorRunner(log, workDir);
        }

        private static void ArgumentHandler_DoUpdate(object? sender, UpdateParameters e)
        {
            var log = new ConsoleLog(LogLevel.Info);
            var updater = new Updater(log);
            if (e.SearchOnly)
            {
                updater.FindNewerRelease(e.Prerelease);
            }
            else
            {
                updater.UpdateProgram(e.Prerelease);
            }
        }

        private static void ArgumentHandler_DoGui(object? sender, GuiParameters e)
        {
            CurrentState.Gui = true;
            CurrentState.GeneratorRunner = CreateRunner(e.Verbose, e.WorkDir);
            _UI = new ConsoleMenu(CurrentState.GeneratorRunner);
            _UI.Run();
        }

        private static void ArgumentHandler_DoBuild(object? sender, BuildParameters e)
        {
            CurrentState.GeneratorRunner = CreateRunner(e.Verbose, e.WorkDir);
            switch (e.Action)
            {
                case ActionType.BuildWeb:
                    CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoBuild());
                    break;
                case ActionType.Clean:
                    CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoClean());
                    break;
                case ActionType.Test:
                    CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoTest());
                    break;
                case ActionType.BuildPrint:
                    CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoPrint());
                    break;
                case ActionType.BuildWordpress:
                    CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoWordpress());
                    break;
                case ActionType.BuildEpub:
                    CurrentState.GeneratorRunner.InitializeAndExecute(x => x.DoEpub());
                    break;
                case ActionType.Initialize:
                    CurrentState.GeneratorRunner.DoInteractiveInitialize();
                    break;
                case ActionType.ValidateConfig:
                    CurrentState.GeneratorRunner.Initialize();
                    break;
            }
        }

        private static void ArgumentHandler_DoHelp(object? sender, EventArgs e)
        {
            Console.WriteLine(HelpTextCreator.GenerateHelpText());
#if DEBUG
            ShowMessageBox("Press a key to continue");
#endif
            Environment.Exit(1);
        }


        private static void ArgumentHandler_DoConfigHelp(object? sender, EventArgs e)
        {
            Console.WriteLine(HelpTextCreator.DocumentConfiguration());
            Environment.Exit(1);
        }

        public static void ShowMessageBox(string text, params object[] args)
        {
            Console.WriteLine(text, args);
            if (!CurrentState.Gui && !CurrentState.NoWaitForExit)
            {
                Console.ReadKey();
            }
        }

        private static void HandleUncaughtException(Exception ex)
        {
            if (_UI != null) _UI.ShouldRun = false;
            Console.Clear();
            ShowMessageBox("Unhandled exception\r\n{0}", ex);
#if DEBUG
            System.Diagnostics.Debugger.Break();
#endif
            Environment.Exit(-1);
        }
    }
}
