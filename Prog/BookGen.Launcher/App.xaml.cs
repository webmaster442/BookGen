//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Windows.Shell;

namespace BookGen.Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : Application
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
