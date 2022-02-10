﻿using System.Collections.Generic;

namespace WpLoad.Services
{
    internal static class FileServices
    {
        private static readonly Dictionary<string, string> _mimeTypes = new()
        {
            { ".jpg", "image/jpg" },
            { ".jpe", "image/jpe" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".webp", "image/webp" },
            { ".txt", "text/plain" },
            { ".mp4", "video/mp4" },
            { ".m4a", "audio/mp4" },
            { ".webm", "video/webm" },
        };

        public static string GetMimeType(string path)
        {
            var extension = Path.GetExtension(path).ToLower();
            if (_mimeTypes.ContainsKey(extension))
                return _mimeTypes[extension];
            else
                return "application/octet-stream";
        }

        public static (IReadOnlyList<string> htmls, IReadOnlyList<string> mediaFiles) GetSupportedFilesInDirectory(string path)
        {
            var dir = new DirectoryInfo(path);
            var files = dir.GetFiles();
            List<string> htmls = new(files.Length / 2);
            List<string> mediaFiles = new(files.Length);
            foreach (var file in files)
            {
                if (file.Exists)
                {
                    var extension = file.Extension.ToLower();
                    if (_mimeTypes.ContainsKey(extension))
                    {
                        mediaFiles.Add(file.FullName);
                    }
                    else if (extension == ".html"
                        || extension == ".htm")
                    {
                        htmls.Add(file.FullName);
                    }
                }
            }
            return (htmls, mediaFiles);
        }
    }
}
