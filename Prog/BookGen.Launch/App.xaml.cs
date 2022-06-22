//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shell;

namespace BookGen.Launch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string FileName = "BookGen.Launch.exe";

        public static void UpdateJumplist(IEnumerable<string> items)
        {
            var list = new JumpList();
            foreach (string? item in items)
            {
                var task = new JumpTask()
                {
                    Title = item,
                    CustomCategory = Launch.Properties.Resources.RecentFolders,
                    ApplicationPath = System.IO.Path.Combine(AppContext.BaseDirectory, FileName),
                    Arguments = $"launch \"{item}\"",
                    IconResourcePath = System.IO.Path.Combine(AppContext.BaseDirectory, FileName),
                    IconResourceIndex = 0,
                    Description = item,
                };
                list.JumpItems.Add(task);
            }
            list.Apply();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            bool? rerminalInstall = TerminalProfileInstaller.TryInstall();
            if (rerminalInstall == false)
            {
                MessageBox.Show(Launch.Properties.Resources.TerminalProfileInstallFail);
            }
        }
    }
}
