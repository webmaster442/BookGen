//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Terminal.Gui;

namespace BookGen.Gui
{
    internal class ConsoleGui
    {
        private MainWindow main;

        public ConsoleGui()
        {
            Application.Init();
            Toplevel top = Application.Top;

            MenuBar menu = new MenuBar(new MenuBarItem[]
            {
                 new MenuBarItem("_File", new MenuItem[]
                 {
                     new MenuItem("Create config", "Create configuration file", null),
                     new MenuItem("Validate config", "Validate configuration file", null),
                     new MenuItem("Exit", "Exit program", () => top.Running = false),
                 }),
                 new MenuBarItem("_Build", new MenuItem[]
                 {
                     new MenuItem("Clean", "Clean output directory", null),
                     new MenuItem("Test web", "Build test website", null),
                     new MenuItem("Web", "Build release website", null),
                     new MenuItem("Print", "Build print html", null),
                     new MenuItem("E-pub", "Build E-pub", null),
                 }),
                 new MenuBarItem("_Help", new MenuItem[]
                 {
                     new MenuItem("Command line args", null, null)
                 }),
            });

            main = new MainWindow();

            top.Add(menu);
            top.Add(main);
        }

        public void Run()
        {
            Application.Run();
        }
    }
}
