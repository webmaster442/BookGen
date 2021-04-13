//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace BookGen.Utilities
{
    internal static class EditorHelper
    {
        private const string vsCodePath = @"%localappdata%\Programs\Microsoft VS Code\Code.exe";
        private const string notepadPath = @"%windir%\notepad.exe";

        private static readonly HashSet<string> supportedFileTypes = new HashSet<string>
        {
            ".txt",
            ".md",
            ".js",
            ".json",
            ".yaml",
            ".html",
            ".htm",
            ".css",
            ".cmd", 
            ".ps",
            ".sh",
            ".css",
            ".php",
            ".py",
            ".xml",
        };

        public static bool IsSupportedFile(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            return supportedFileTypes.Contains(ext);
        }

        public static bool TryFindVsCodeInstall(out string installedVsCodePath)
        {
            installedVsCodePath = Environment.ExpandEnvironmentVariables(vsCodePath);
            return File.Exists(installedVsCodePath);
        }

        public static string GetNotepadPath()
        {
            return Environment.ExpandEnvironmentVariables(notepadPath);
        }
    }
}
