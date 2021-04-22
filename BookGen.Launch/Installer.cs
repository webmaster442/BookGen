//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;

namespace BookGen.Launch
{
    internal class Installer
    {
        private readonly static string _Directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private const int STGM_READ = 0x00000000;

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

        public static bool IsInstalled
        {
            get
            {
                bool installed = false;
                foreach (var linkFile in Directory.GetFiles(_Directory, "*.lnk"))
                {
                    if (installed)
                    {
                        break;
                    }
                    if (new ShellLink() is IShellLinkW shellLink)
                    {
                        ((IPersistFile)shellLink).Load(linkFile, STGM_READ);

                        StringBuilder buffer = new StringBuilder(4096);
                        shellLink.GetPath(buffer, buffer.Capacity, IntPtr.Zero, 0);

                        var file = buffer.ToString();

                        if (file == Process.GetCurrentProcess().StartInfo.FileName)
                        {
                            installed = true;
                        }
                        Marshal.ReleaseComObject(shellLink);
                    }
                }
                return installed;
            }
        }

        private static void CreateShortcut()
        {
            var program = Process.GetCurrentProcess().StartInfo.FileName;

            string linkFileName = Path.Combine(_Directory, "Launch BookGen Shell" + ".lnk");
            CreateLink(program, linkFileName, string.Empty, string.Empty);
        }

        public static void HandleInstall()
        {
            if (!IsInstalled)
            {
                var result = MessageBox.Show("Would you like to create a shortcut on the desktop?",
                                             "Create Shortcut",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    CreateShortcut();
                }
            }
        }
    }
}
