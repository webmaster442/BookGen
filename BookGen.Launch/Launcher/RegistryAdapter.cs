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
    }
}
