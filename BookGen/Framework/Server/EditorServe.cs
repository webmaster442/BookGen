//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Net;
using System.Text;

namespace BookGen.Framework.Server
{
    public class EditorServe : IRequestHandler
    {
        public bool CanServe(string AbsoluteUri)
        {
            return AbsoluteUri == "/editor.html";
        }

        public void Serve(HttpListenerResponse response)
        {
            response.ContentType = "text/html";
            response.AddHeader("Date", DateTime.Now.ToString("r"));
            response.AddHeader("Last-Modified", DateTime.Now.ToString("r"));
            byte[] bytes = Encoding.UTF8.GetBytes(Properties.Resources.editor);
            response.ContentLength64 = bytes.Length;
            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.OutputStream.Flush();
        }
    }
}
