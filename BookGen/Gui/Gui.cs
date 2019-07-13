//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using System;
using Terminal.Gui;

namespace BookGen.Gui
{
    internal class ConsoleGui
    {
        private readonly MainWindow _main;
        private readonly EventedLog _log;

        public ConsoleGui(EventedLog log, GeneratorRunner runner)
        {
            _log = log;
            Application.Init();
            Toplevel top = Application.Top;

            MenuBar menu = new MenuBar(new MenuBarItem[]
            {
                 new MenuBarItem("_File", new MenuItem[]
                 {
                     new MenuItem("Create config", "Create configuration file", () =>
                     {
                         runner.DoCreateConfig();
                     }),
                     new MenuItem("Validate config", "Validate configuration file", () =>
                     {
                         runner.Initialize();
                     }),
                     new MenuItem("Exit", "Exit program", Stop),
                 }),
                 new MenuBarItem("_Build", new MenuItem[]
                 {
                     new MenuItem("Clean", "Clean output directory", () =>
                     {
                         if (runner.Initialize())
                             runner.DoClean();
                     }),
                     new MenuItem("Test web", "Build test website", () =>
                     {
                         if (runner.Initialize())
                             runner.DoTest();
                     }),
                     new MenuItem("Web", "Build release website", () =>
                     {
                         if (runner.Initialize())
                             runner.DoBuild();
                     }),
                     new MenuItem("Print", "Build print html", () =>
                     {
                         if (runner.Initialize())
                             runner.DoPrint();
                     }),
                     new MenuItem("E-pub", "Build E-pub", () =>
                     {
                         if (runner.Initialize())
                             runner.DoEpub();
                     }),
                 }),
                 new MenuBarItem("_Help", new MenuItem[]
                 {
                     new MenuItem("Command line args", null, () =>
                     {
                         runner.RunHelp();
                     })
                 }),
            });

            _main = new MainWindow(runner.WorkDirectory);

            top.Add(menu);
            top.Add(_main);
        }

        private void Stop()
        {
            _log.LogWritten -= _log_LogWritten;
            Application.Top.Running = false;
        }

        public void Run()
        {
            _log.LogWritten += _log_LogWritten;
            Application.Run();
        }

        private void _log_LogWritten(object sender, EventArgs e)
        {
            _main.UpdateLog(_log.LogText, _log.Lines);
        }
    }
}
