//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BookGen.Framework.Server
{
    public static class HttpExtensions
    {
        public static void WriteString(this HttpListenerResponse response, string content, string mime)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                ms.Seek(0, SeekOrigin.Begin);
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentEncoding = Encoding.UTF8;
                response.ContentType = mime;
                response.ContentLength64 = ms.Length;
                response.SendChunked = true;

                ms.CopyTo(response.OutputStream, 4096);

                response.OutputStream.Flush();
            }
        }

        public static void WriteHtmlString(this HttpListenerResponse response, string content)
        {
            WriteString(response, content, MimeTypes.GetMimeForExtension(".html"));
        }

        public static Dictionary<string, string> ParsePostParameters(this HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
                return new Dictionary<string, string>();

            using (var reader = new StreamReader(request.InputStream))
            {
                return ParseQueryParameters(reader.ReadToEnd());
            }
        }

        public static Dictionary<string, string> ParseQueryParameters(this string query)
        {
            var dictionary = new Dictionary<string, string>();

            if (query.Length < 1)
                return dictionary;

            string[] parts;

            if (query.StartsWith('?'))
                parts = query.Substring(1).Split('&');
            else
                parts = query.Split('&');

            foreach (var part in parts)
            {
                var firstSplitter = part.IndexOf('=');
                if (firstSplitter != -1)
                {
                    var key = part.Substring(0, firstSplitter);
                    var value = part.Substring(firstSplitter+1);
                    if (dictionary.ContainsKey(key))
                    {
                        dictionary[key] = value;
                    }
                    else
                    {
                        dictionary.Add(key, value);
                    }
                }
            }

            return dictionary;
        }
    }
}
