//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Infrastructure;
using System;
using System.Windows;

namespace BookGen.Editor
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Locator.Initialize();
            EditorSessionManager.Initialize();

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            App app = new App();
            app.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            app.InitializeComponent();

            StartupDialog startup = new StartupDialog(EditorSessionManager.CurrentSession);
            if (startup.ShowDialog() == true)
            {
                app.MainWindow = new MainWindow();
                app.MainWindow.Show();
                app.MainWindow.BringIntoView();
                app.Run(app.MainWindow);
            }
            Properties.Settings.Default.Save();
            EditorSessionManager.Close();
        }
    }
}
