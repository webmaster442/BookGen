//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace BookGen.Launch
{
    internal class Installer
    {
        public bool IsInstalled
        {
            get
            {
                RegistryKey? key  = Registry.CurrentUser.OpenSubKey("Software", true);
                var subkey = key?.OpenSubKey(nameof(BookGen.Launch));
                if (subkey?.GetValue("isInstalled", false) is bool value)
                {
                    return value;
                }
                return false;
            }
            set
            {
                RegistryKey? key = Registry.CurrentUser.OpenSubKey("Software", true);
                var subkey = key?.OpenSubKey(nameof(BookGen.Launch));
                subkey?.SetValue("isInstalled", value);

            }
        }

        private static void CreateLink(string PathToFile,
                          string PathToLink,
                          string Arguments,
                          string Description)
        {
            if (new ShellLink() is IShellLinkW shellLink)
            {
                Marshal.ThrowExceptionForHR(shellLink.SetDescription(Description));
                Marshal.ThrowExceptionForHR(shellLink.SetPath(PathToFile));
                Marshal.ThrowExceptionForHR(shellLink.SetArguments(Arguments));

                ((IPersistFile)shellLink).Save(PathToLink, false);
                Marshal.ReleaseComObject(shellLink);
            }
        }

        public void CreateShortcut()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string linkFileName = Path.Combine(dir, "BookGen.Launch" + ".lnk");
            //CreateLink(program, linkFileName, string.Empty, string.Empty);
        }


        public Installer()
        {

        }
    }
}
