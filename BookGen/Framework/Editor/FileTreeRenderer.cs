//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BookGen.Framework.Editor
{
    public static class FileTreeRenderer
    {
        public static string Render(string folder)
        {
            var buffer = new StringBuilder();
            var folders = new Stack<string>(Directory.GetDirectories(folder));

            buffer.AppendFormat("<h3>{0}</h3>\n", folder);

            while (folders.Count > 0)
            {
                string current = folders.Pop();
                var files = Directory.GetFiles(current, "*.*");

                buffer.Append("<details open>\n");
                buffer.AppendFormat("<summary>{0}</summary>\n", GetName(current, folder));

                RenderFiles(buffer, files, current);

                var subDirectories = Directory.GetDirectories(current);
                foreach (var directory in subDirectories)
                {
                    folders.Push(directory);
                }
                buffer.Append("</details>\n");

            }
            return buffer.ToString();
        }

        private static string GetName(string item, string rootfolder)
        {
            return item.Substring(rootfolder.Length);
        }

        private static void RenderFiles(StringBuilder buffer, string[] files, string rootFolder)
        {
            if (files.Length < 1) return;

            buffer.Append("<ul>\n");
            foreach (var file in files)
            {
                buffer.AppendFormat("<li>{0}", GetName(file, rootFolder));
                if (IsMarkdownFile(file))
                {
                    buffer.Append(" <a href=\"#\">[ Edit ]</a>\n");
                }
                else if (OpenableFile(file))
                {
                    buffer.Append(" <a href=\"#\">[ Open ]</a>\n");
                }
                buffer.Append("</li>\n");
            }
            buffer.Append("</ul>\n");
        }

        private static bool OpenableFile(string file)
        {
            return true;
        }

        private static bool IsMarkdownFile(string file)
        {
            return Path.GetExtension(file).ToLower() == ".md"; 
        }
    }
}
