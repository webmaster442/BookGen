//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
            string? extension = Path.GetExtension(path).ToLower();
            if (_mimeTypes.ContainsKey(extension))
                return _mimeTypes[extension];
            else
                return "application/octet-stream";
        }

        public static (IReadOnlyList<string> htmls, IReadOnlyList<string> mediaFiles) GetSupportedFilesInDirectory(string path)
        {
            var dir = new DirectoryInfo(path);
            FileInfo[]? files = dir.GetFiles();
            List<string> htmls = new(files.Length / 2);
            List<string> mediaFiles = new(files.Length);
            foreach (FileInfo? file in files)
            {
                if (file.Exists)
                {
                    string? extension = file.Extension.ToLower();
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
