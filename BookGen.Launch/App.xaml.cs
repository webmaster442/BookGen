//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
            JumpList list = new JumpList();
            foreach (var item in items)
            {
                JumpTask task = new JumpTask()
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
    }
}
