//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace BookGen.Framework.Server
{
    public static class MimeTypes
    {
        private static readonly Dictionary<string, string>  _db =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                { ".asf", "video/x-ms-asf"},
                { ".asx", "video/x-ms-asf"},
                { ".avi", "video/x-msvideo"},
                { ".cco", "application/x-cocoa"},
                { ".crt", "application/x-x509-ca-cert"},
                { ".css", "text/css"},
                { ".der", "application/x-x509-ca-cert"},
                { ".ear", "application/java-archive"},
                { ".flv", "video/x-flv"},
                { ".gif", "image/gif"},
                { ".hqx", "application/mac-binhex40"},
                { ".htc", "text/x-component"},
                { ".htm", "text/html"},
                { ".html", "text/html"},
                { ".ico", "image/x-icon"},
                { ".jar", "application/java-archive"},
                { ".jardiff", "application/x-java-archive-diff"},
                { ".jng", "image/x-jng"},
                { ".jnlp", "application/x-java-jnlp-file"},
                { ".jpeg", "image/jpeg"},
                { ".jpg", "image/jpeg"},
                { ".js", "application/x-javascript"},
                { ".json", "application/json" },
                { ".webp", "image/webp"},
                { ".mml", "text/mathml"},
                { ".mng", "video/x-mng"},
                { ".mov", "video/quicktime"},
                { ".mp3", "audio/mpeg"},
                { ".mp4", "video/mp4"},
                { ".m4a", "video/mp4"},
                { ".m4b", "video/mp4"},
                { ".m4v", "video/mp4"},
                { ".webm", "video/webm"},
                { ".mpeg", "video/mpeg"},
                { ".mpg", "video/mpeg"},
                { ".pdb", "application/x-pilot"},
                { ".pdf", "application/pdf"},
                { ".pem", "application/x-x509-ca-cert"},
                { ".pl", "application/x-perl"},
                { ".pm", "application/x-perl"},
                { ".png", "image/png"},
                { ".prc", "application/x-pilot"},
                { ".ra", "audio/x-realaudio"},
                { ".rar", "application/x-rar-compressed"},
                { ".rpm", "application/x-redhat-package-manager"},
                { ".rss", "text/xml"},
                { ".run", "application/x-makeself"},
                { ".sea", "application/x-sea"},
                { ".shtml", "text/html"},
                { ".svg", "image/svg+xml"},
                { ".sit", "application/x-stuffit"},
                { ".swf", "application/x-shockwave-flash"},
                { ".tcl", "application/x-tcl"},
                { ".tk", "application/x-tcl"},
                { ".txt", "text/plain"},
                { ".war", "application/java-archive"},
                { ".wbmp", "image/vnd.wap.wbmp"},
                { ".wmv", "video/x-ms-wmv"},
                { ".xml", "text/xml"},
                { ".xpi", "application/x-xpinstall"},
                { ".zip", "application/zip"},
                { ".ttf", "application/x-font-ttf" },
                { ".otf", "application/x-font-opentype" },
                { ".woff", "application/font-woff" },
                { ".woff2", "application/font-woff2" },
                { ".eot", "application/vnd.ms-fontobject" },
                { ".sfnt", "application/font-sfnt" },
                { ".c", "text/plain"},
                { ".cpp", "text/plain"},
                { ".h", "text/plain"},
                { ".cs", "text/plain"},
                { ".bat", "text/plain"},
                { ".sh", "text/plain"},
                { ".md", "text/plain"},
            };

        public static string GetMimeForExtension(string extension)
        {
            if (_db.ContainsKey(extension))
                return _db[extension];
            else
                return "application/octet-stream";
        }

        public static string GetMimeTypeForFile(string file)
        {
            var ext = System.IO.Path.GetExtension(file);
            return GetMimeForExtension(ext);
        }
    }
}
