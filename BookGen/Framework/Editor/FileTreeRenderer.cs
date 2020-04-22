//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

namespace BookGen.Framework.Editor
{
    public static class FileTreeRenderer
    {
        private static string _rootfolder = string.Empty;

        public static string Render(string inputFolder)
        {
            _rootfolder = inputFolder;
            var buffer = new StringBuilder();
            buffer.AppendFormat("<h3>{0}</h3>\n", inputFolder);
            RenderFolderContents(buffer, new DirectoryInfo(inputFolder));
            return buffer.ToString();
        }

        private static void RenderFolderContents(StringBuilder buffer, DirectoryInfo inputFolder)
        {
            var directories = inputFolder.GetDirectories();

            buffer.Append("<details class=\"item\" open>\n");
            buffer.AppendFormat("<summary>{0}</summary>\n", inputFolder.Name);

            foreach (var directory in directories)
            {
                if (directory.Attributes.HasFlag(FileAttributes.Hidden)) continue;
                RenderFolderContents(buffer, directory);
            }
            var files = inputFolder.GetFiles();
            if (files.Length > 0)
            {
                buffer.Append("<ul>\n");
                foreach (var file in files)
                {
                    if (file.Attributes.HasFlag(FileAttributes.Hidden)) continue;
                    buffer.AppendFormat("<li>{0} {1}</li>", file.Name, GetLinksForFile(file));
                }
                buffer.Append("</ul>\n");
            }

            buffer.Append("</details>\n");
        }

        private static string GetLinksForFile(FileInfo file)
        {
            string path = file.FullName.Replace(_rootfolder, "");
            if (IsMarkdownFile(file))
            {
                string param = Convert.ToBase64String(Encoding.UTF8.GetBytes(path));
                return string.Format(" <a target=\"_blank\" href=\"/editor.html?file={0}\">[Edit]</a>\n", Uri.EscapeDataString(param));
            }
            else
            {
                return string.Format(" <a href=\"{0}\">[Open]</a>\n", path);
            }
        }

        private static bool IsMarkdownFile(FileInfo file)
        {
            return string.Equals(file.Extension, ".md", StringComparison.OrdinalIgnoreCase); 
        }
    }
}
