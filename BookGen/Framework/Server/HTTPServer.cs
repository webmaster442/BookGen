//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.IO;
using BookGen.Core;
using System.Threading.Tasks;
using BookGen.Resources;

namespace BookGen.Framework.Server
{
    internal sealed class HttpServer : IDisposable
    {
        private readonly ILog _log;
        private readonly string _path;
        private readonly IEnumerable<IRequestHandler> _handlers;
        private HttpListener? _listener;
        private readonly object _handlerLock;

        private CancellationTokenSource? _cts;

        public int Port { get; }

        public List<string> IndexFiles { get; }

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
        }

        public HttpServer(string path, int port, ILog log, params IRequestHandler[] handlers)
        {
            _handlers = handlers;
            _cts = new CancellationTokenSource();
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
            _handlerLock = new object();
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{Port.ToString()}/");
            Task.Run(() => Start(_cts.Token));
        }

        private void Start(CancellationToken token)
        {
            _listener?.Start();
            do
            {
                try
                {
                    HttpListenerContext? request = _listener?.GetContext();
                    if (token.IsCancellationRequested) break;
                    ThreadPool.QueueUserWorkItem(ProcessRequest, request);

                }
                catch (HttpListenerException lex)
                {
                    _log.Warning(lex);
                }
                catch (InvalidOperationException iex)
                {
                    _log.Warning(iex);
                }
            }
            while (!token.IsCancellationRequested);
        }

        private void ProcessRequest(object? listenerContext)
        {
            try
            {
                if (!(listenerContext is HttpListenerContext context)) return;

                string filename = context.Request.Url.AbsolutePath;
                _log.Detail("Serving: {0}", filename);

                bool processed = false;

                using (context.Response)
                {
                    processed = TryToServeWitHandler(context, filename);

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
                            processed = context.Response.WriteFile(filename);
                    }

                    if (!processed)
                        Serve404(context.Response);
                }

            }
            catch (Exception ex)
            {
                _log.Warning(ex);
            }
        }

        private bool TryToServeWitHandler(HttpListenerContext context, string filename)
        {
            lock (_handlerLock)
            {
                if (_handlers != null)
                {
                    foreach (var handler in _handlers)
                    {
                        if (handler.CanServe(filename))
                        {
                            if (handler is ISimpleRequestHandler simple)
                                simple.Serve(filename, context.Response, _log);
                            else if (handler is IAdvancedRequestHandler advanced)
                                advanced.Serve(context.Request, context.Response, _log);

                            return true;
                        }
                    }
                }
                return false;
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

        private void Serve404(HttpListenerResponse response)
        {
            response.ContentType = "text/html";
            response.AddHeader("Date", DateTime.Now.ToString("r"));
            response.AddHeader("Last-Modified", DateTime.Now.ToString("r"));

            using (var error404 = ResourceHandler.GetResourceStream<HttpServer>("Resources/Error404.html"))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.ContentLength64 = error404.Length;
                error404.CopyTo(response.OutputStream);
            }
        }
    }
}
