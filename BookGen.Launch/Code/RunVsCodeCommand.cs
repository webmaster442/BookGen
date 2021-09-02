//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.IO;
using System.Windows;

namespace BookGen.Launch.Code
{
    internal sealed class RunVsCodeCommand : ProcessCommandBase
    {
        public override bool CanExecute(string? folder)
        {
            return isVSCodeInstalled;
        }

        public override void Execute(string? folder)
        {
            if (string.IsNullOrEmpty(folder)
                || !Directory.Exists(folder))
            {
                Message(Properties.Resources.FolderNoLongerExists, MessageBoxImage.Error);
                return;
            }

            RunProgram(VsCodeExe, folder);
        }
    }
}
