//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace BookGen.Launch.Launcher
{
    internal class RegistryAdapter
    {
        private const int maxDirectories = 10;
        private readonly string _subKey;

        public RegistryAdapter(string subKey)
        {
            _subKey = subKey;
        }

        public IEnumerable<string> GetRecentDirectoryList()
        {
            using var key = Registry.CurrentUser.OpenSubKey(_subKey);
            if (key != null)
            {
                for (int i = 0; i < maxDirectories; i++)
                {
                    var dirKey = key.GetValue($"Dir{i}");
                    if (dirKey != null)
                    {
                        yield return (string)dirKey;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }

        public void SaveRecentDirectoryList(IList<string> recentFolders)
        {
            using var key = Registry.CurrentUser.CreateSubKey(_subKey);
            if (key != null)
            {
                int count = Math.Min(recentFolders.Count, maxDirectories);
                for (int i=0; i<count; i++)
                {
                    key.SetValue($"Dir{i}", recentFolders[i], RegistryValueKind.String);
                }
            }
        }

        public void DeleteRecentDirectoryList()
        {
            using var key = Registry.CurrentUser.CreateSubKey(_subKey);
            if (key != null)
            {
                for (int i=0; i<maxDirectories; i++)
                {
                    var dirKey = key.GetValue($"Dir{i}");
                    if (dirKey != null)
                    {
                        key.DeleteValue($"Dir{i}");
                    }
                }
            }
        }

        internal void SaveWindowsTerminal(bool value)
        {
            using var key = Registry.CurrentUser.CreateSubKey(_subKey);
            if (key != null)
            {
                int write = value ? 1 : 0;
                key.SetValue("WinTerminal", write, RegistryValueKind.DWord);
            }
        }

        internal bool? GetWindowsTerminalUsage()
        {
            using var key = Registry.CurrentUser.OpenSubKey(_subKey);
            if (key != null)
            {
                var value = key.GetValue("WinTerminal");
                if (value != null)
                {
                    return (int)value == 1;
                }
            }
            return null;
        }

        internal void DeleteRecentItem(string directory)
        {
            using var key = Registry.CurrentUser.CreateSubKey(_subKey);
            if (key != null)
            {
                for (int i = 0; i < maxDirectories; i++)
                {
                    var dirKey = key.GetValue($"Dir{i}");
                    if (dirKey != null
                        && (string)dirKey == directory)
                    {
                        key.DeleteValue($"Dir{i}");
                    }
                }
            }
        }
    }
}
