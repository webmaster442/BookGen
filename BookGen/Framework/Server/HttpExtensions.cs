//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net;
using System.Text;

namespace BookGen.Framework.Server
{
    public static class HttpExtensions
    {
        public static void WriteString(this HttpListenerResponse response, string content, string mime)
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(content);
            response.StatusCode = 200;
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = mime;
            response.OutputStream.Write(responseBytes, 0, content.Length);
        }

        public static void WriteJavascriptString(this HttpListenerResponse response, string content)
        {
            WriteString(response, content, MimeTypes.GetMimeForExtension(".js"));
        }

        public static void WriteCssString(this HttpListenerResponse response, string content)
        {
            WriteString(response, content, MimeTypes.GetMimeForExtension(".css"));
        }

        public static void WriteHtmlString(this HttpListenerResponse response, string content)
        {
            WriteString(response, content, MimeTypes.GetMimeForExtension(".html"));
        }
    }
}
