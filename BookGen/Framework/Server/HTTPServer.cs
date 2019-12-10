//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookGen.Framework.Server
{
    internal sealed class HttpTestServer : IDisposable
    {
        private readonly string _path;
        private readonly ILog _log;
        private readonly IEnumerable<IRequestHandler> _handlers;
        private HttpListener? _listener;
        private Semaphore? _sem;
        private CancellationTokenSource? _cts;

        public int Port { get; }

        public List<string> IndexFiles { get; }

        public HttpTestServer(string path, int port, ILog log, params IRequestHandler[] handlers)
        {
            _sem = new Semaphore(1, 3);
            _cts = new CancellationTokenSource();
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
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{Port.ToString()}/");
            _listener.Start();
            Task.Run(Serve, _cts.Token);
        }

        private async Task Serve()
        {
            while (true)
            {
                _sem?.WaitOne();
                try
                {
                    if (_listener != null)
                    {
                        HttpListenerContext context = await _listener.GetContextAsync().ConfigureAwait(false);
                        Process(context);
                    }
                    _sem?.Release();

                }
                catch (ObjectDisposedException)
                {
                    //_listener.GetContextAsync() will throw, no way to abort.
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
                        {
                            filename = GetIndexFile(_path);
                        }
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

            var error404 = ResourceLocator.GetResourceFile<HttpTestServer>("Resources/Error404.html");

            byte[] bytes = Encoding.UTF8.GetBytes(error404);
            response.ContentLength64 = bytes.Length;
            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.OutputStream.Flush();
        }

        public void Dispose()
        {
            if (_cts?.IsCancellationRequested == false)
            {
                _cts.Cancel();
                Thread.Sleep(100);
                _cts.Dispose();
                _cts = null;
            }
            if (_listener != null)
            {
                _listener.Stop();
                _listener.Close();
                _listener = null;
            }
            if (_sem != null)
            {
                _sem.Dispose();
                _sem = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
