using BookGen.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BookGen.Core;
using System.Net;

namespace BookGen.Framework.Editor
{
    internal class FileExplorerHelper
    {
        private Dictionary<string, FileInfo[]> _folders;
        private string _root;


        public FileExplorerHelper()
        {
            _folders = new Dictionary<string, FileInfo[]>();
            _root = string.Empty;
        }

        public string RenderFileExplorer(string inputFolder)
        {
            _folders.Clear();
            _root = inputFolder;

            StringBuilder output = new StringBuilder(16 * 1024);
            output.WriteElement(HtmlElement.Div, "ExplorerTree", "p-2");
            WalkDirTree(output, new DirectoryInfo(inputFolder));
            output.CloseElement(HtmlElement.Div);

            output.WriteElement(HtmlElement.Div, "DirectoryFiles", "p-2", "flex-grow-1");
            RenderFilesOfFolder(output, inputFolder);
            output.CloseElement(HtmlElement.Div);

            output.WriteElement(HtmlElement.Div, "Folders");
            RenderFolderAllFolderContents(output);
            output.CloseElement(HtmlElement.Div);
            return output.ToString();
        }

        private string GetUrl(string fullName)
        {
            return fullName.Remove(0, _root.Length).Replace("\\", "/");
        }

        private void WalkDirTree(StringBuilder targetBuffer, DirectoryInfo root)
        {
            DirectoryInfo[] subirs = root.GetDirectories();

            targetBuffer.WriteElement(HtmlElement.Ul);
            targetBuffer.WriteElement(HtmlElement.Li);

            targetBuffer.WriteHref($"{GetUrl(root.FullName)}", root.Name, "filetreeItem");

            targetBuffer.CloseElement(HtmlElement.Li);

            FileInfo[] files = root.GetFiles();
            _folders.Add(root.FullName, files);

            if (subirs.Length > 0)
            {
                foreach (DirectoryInfo subdir in subirs)
                {
                    if (subdir.Attributes.HasFlag(FileAttributes.Hidden)) continue;
                    WalkDirTree(targetBuffer, subdir);
                }
            }
            targetBuffer.CloseElement(HtmlElement.Ul);
        }

        private void RenderFilesOfFolder(StringBuilder output, string inputFolder)
        {
            output.WriteHeader(1, inputFolder);
            if (_folders[inputFolder].Length < 1)
            {
                output.WriteParagraph("No files found");
                return;
            }

            output.WriteElement(HtmlElement.Table, "DefaultView", "table", "table-striped");
            output.WriteTableHeader("Name", "Extension", "Size", "Last Written", "Actions");
            foreach (FileInfo file in _folders[inputFolder])
            {
                output.WriteTableRow(file.Name, file.Extension, file.Length, file.LastWriteTime, "[open] [edit]");
            }
            output.CloseElement(HtmlElement.Table);
        }

        private void RenderFolderAllFolderContents(StringBuilder output)
        {
            foreach (var folder in _folders.Keys)
            {
                string id = GetUrl(folder).Replace("/", "_");
                output.WriteElement(HtmlElement.Div, id);
                RenderFilesOfFolder(output, folder);
                output.CloseElement(HtmlElement.Div);
            }
        }
    }
}
