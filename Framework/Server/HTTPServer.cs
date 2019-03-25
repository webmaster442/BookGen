//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace BookGen.Framework.Server
{
    internal class HTTPTestServer : IDisposable
    {
        private readonly string _path;
        private readonly ILog _log;
        private readonly Thread _server;
        private readonly IEnumerable<IRequestHandler> _handlers;
        private HttpListener _listener;

        public int Port { get; }

        public List<string> IndexFiles { get; }

        public HTTPTestServer(string path, int port, ILog log, params IRequestHandler[] handlers)
        {
            _handlers = handlers;
            IndexFiles = new List<string>
            {
                "index.html",
                "index.htm",
                "default.html",
                "default.htm"
            };
            _path = path;
            Port = port;
            _log = log;
            _server = new Thread(Serve);
            _server.Start();
        }

        private void Serve()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:" + Port.ToString() + "/");
            _listener.Start();
            while (true)
            {
                try
                {
                    HttpListenerContext context = _listener.GetContext();
                    Process(context);
                }
                catch (Exception ex)
                {
                    _log.Warning(ex);
                }
            }
        }

        private void Process(HttpListenerContext context)
        {
            string filename = context.Request.Url.AbsolutePath;
            _log.Detail("Serving: {0}", filename);
            bool processed = false;
            using (context.Response)
            {
                try
                {
                    if (_handlers != null)
                    {
                        foreach (var handler in _handlers)
                        {
                            if (handler.CanServe(filename))
                            {
                                handler.Serve(context.Response);
                                processed = true;
                                break;
                            }
                        }
                    }

                    if (!processed)
                    {
                        filename = filename.Substring(1);

                        if (string.IsNullOrEmpty(filename))
                            filename = GetIndexFile(_path);

                        else
                        {
                            filename = Path.Combine(_path, filename);
                            if (Directory.Exists(filename))
                                filename = GetIndexFile(filename);
                        }
                        if (File.Exists(filename))
                            ServeFile(context.Response, filename);
                        else
                            Serve404(context.Response);

                        Thread.Sleep(10);
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _log.Warning(ex);
                }
            }
        }

        private string GetIndexFile(string _path)
        {
            foreach (string indexFile in IndexFiles)
            {
                var localindex = Path.Combine(_path, indexFile);
                if (File.Exists(localindex))
                {
                    return localindex;
                }
            }

            return "$$ERROR404$$";
        }

        private void ServeFile(HttpListenerResponse response, string filename)
        {
            response.ContentType = MimeTypes.GetMimeForExtension(Path.GetExtension(filename));
            response.AddHeader("Date", DateTime.Now.ToString("r"));
            response.AddHeader("Last-Modified", File.GetLastWriteTime(filename).ToString("r"));

            using (var stream = File.OpenRead(filename))
            {
                response.ContentLength64 = stream.Length;
                stream.CopyTo(response.OutputStream);
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.OutputStream.Flush();
        }

        private void Serve404(HttpListenerResponse response)
        {
            response.ContentType = "text/html";
            response.AddHeader("Date", DateTime.Now.ToString("r"));
            response.AddHeader("Last-Modified", DateTime.Now.ToString("r"));
            byte[] bytes = Encoding.UTF8.GetBytes(Properties.Resources.Error404);
            response.ContentLength64 = bytes.Length;
            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.OutputStream.Flush();
        }

        public void Dispose()
        {
            if (_listener != null)
            {
                _listener.Stop();
                _listener.Close();
                _server.Abort();
                _server.Join();
                _listener = null;
            }
        }
    }
}
