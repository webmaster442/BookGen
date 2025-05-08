//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;

namespace BookGen.DomainServices
{
    public static class EditorHelper
    {
        private static readonly HashSet<string> SupportedFileTypes = new()
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
            string? ext = Path.GetExtension(file).ToLower();
            return SupportedFileTypes.Contains(ext);
        }

        public static bool TryFindVsCodeInstall(out string installedVsCodePath)
        {
            installedVsCodePath = Environment.ExpandEnvironmentVariables(Constants.VsCodePath);
            return File.Exists(installedVsCodePath);
        }

        public static string GetNotepadPath()
        {
            return Environment.ExpandEnvironmentVariables(Constants.NotepadPath);
        }
    }
}
