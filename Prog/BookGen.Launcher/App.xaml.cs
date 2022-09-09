using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace BookGen.Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string FileName = "BookGen.Launcher.exe";

        public static void UpdateJumplist(IEnumerable<string> items)
        {
            var list = new JumpList();
            foreach (string? item in items)
            {
                var task = new JumpTask()
                {
                    Title = item,
                    CustomCategory = BookGen.Launcher.Properties.Resources.RecentFolders,
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

    }
}
