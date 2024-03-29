﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2023 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Net.Sockets;

using BookGen.Api;

using Webmaster442.HttpServerFramework.Domain;
using Webmaster442.HttpServerFramework.Internal;

namespace Webmaster442.HttpServerFramework;

/// <summary>
/// A Http server
/// </summary>
public sealed class HttpServer : IDisposable
{
    private readonly ILog? _log;
    private readonly HttpServerConfiguration _configuration;
    private readonly List<IRequestHandler> _handlers;

    private TcpListener? _listner;
    private bool _canRun;
    private int _freeClientSlots;

    /// <summary>
    /// Predicate that can be used to impelement whitelist or blaclist
    /// based on TcpClient information
    /// </summary>
    public Predicate<TcpClient>? ClientFilteringPredicate { get; set; }

    /// <summary>
    /// Creates a new instance of HttpServer
    /// </summary>
    /// <param name="configuration">Server configuration</param>
    /// <param name="log">logger to use</param>
    public HttpServer(HttpServerConfiguration configuration, ILog? log = null)
    {
        ConfigurationValidator.ValidateAndTrhowExceptions(configuration);

        _configuration = configuration;
        _log = log;
        _handlers = new List<IRequestHandler>();
        _freeClientSlots = configuration.MaxClients;
        _listner = new TcpListener(configuration.ListenAdress, configuration.Port);
    }

    /// <summary>
    /// Gets the Current configuration
    /// </summary>
    public HttpServerConfiguration Configuration
        => _configuration;

    /// <summary>
    /// Add a handler that can process requests to the server
    /// </summary>
    /// <param name="handler">Request handler</param>
    /// <returns>The server</returns>
    public HttpServer RegisterHandler(IRequestHandler handler)
    {
        _handlers.Add(handler);
        return this;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Stop();
        _listner = null;
        foreach (var handler in _handlers)
        {
            if (handler is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

    /// <summary>
    /// Starts listening for connections and serving them
    /// </summary>
    public void Start()
    {
        if (_handlers.Count < 1)
            throw new InvalidOperationException("No request handlers are configured");
        _canRun = true;
        Task.Run(StartPrivate);
    }

    private async Task StartPrivate()
    {
        _listner?.Start();
        while (_canRun && _listner != null)
        {
            using TcpClient client = await _listner.AcceptTcpClientAsync();
            try
            {
                if (ClientFilteringPredicate != null)
                {
                    _log?.Info("Running {0} ...", nameof(ClientFilteringPredicate));
                    bool canHandle = ClientFilteringPredicate.Invoke(client);
                    if (!canHandle)
                    {
                        _log?.Warning("{0} returned false. Refusing client connection", nameof(ClientFilteringPredicate));
                        client?.Dispose();
                        continue;
                    }
                }

                if (_freeClientSlots > 0)
                {
                    Interlocked.Decrement(ref _freeClientSlots);
                    await HandleClient(client);
                    Interlocked.Increment(ref _freeClientSlots);
                }
                else
                {
                    client?.Dispose();
                    _log?.Warning("Maximum number of clients ({0}) reached. Refusing connection", _configuration.MaxClients);
                }
            }
            catch (Exception ex)
            {
                _log?.Critical(ex);
            }
        }
    }


    /// <summary>
    /// Stops listening for connections and serving them
    /// </summary>
    public void Stop()
    {
        _canRun = false;
        _listner?.Stop();
    }

    private async Task HandleClient(TcpClient client)
    {
        await Task.Yield();
        var parser = new RequestParser(_configuration.MaxPostSize);
        using var stream = client.GetStream();
        HttpResponse response = new HttpResponse(stream);
        try
        {
            HttpRequest request = parser.ParseRequest(stream);

            if (string.IsNullOrEmpty(request.Url)) return;

            bool wasHandled = false;
            foreach (var handler in _handlers)
            {
                bool result = await handler.Handle(_log, request, response);
                if (result)
                {
                    wasHandled = true;
                    break;
                }
            }

            if (!wasHandled)
            {
                throw new ServerException(HttpResponseCode.NotFound, request.Url);
            }
        }
        catch (Exception ex)
        {
            if (ex is ServerException serverException)
            {
                _log?.Warning(serverException.Message);
                await ServeErrorHandler(response, serverException.ResponseCode);

            }
            else
            {
                _log?.Critical(ex);
                await ServeInternalServerError(response, ex);
            }
        }
    }

    private async ValueTask ServeInternalServerError(HttpResponse response, Exception ex)
    {
        response.ResponseCode = HttpResponseCode.InternalServerError;
        response.ContentType = "text/html";
        HtmlBuilder builder = new HtmlBuilder("Internal server error");
        builder.AppendParagraph("Internal server error happened");
        if (_configuration.DebugMode)
        {
            builder.AppendHr();
            builder.AppendParagraph($"Message: {ex.Message}");
            builder.AppendParagraph("Stack trace:");
            builder.AppendPre(ex.StackTrace ?? "");
        }
        await response.WriteAsync(builder.ToString());
    }

    private async ValueTask ServeErrorHandler(HttpResponse response, HttpResponseCode responseCode)
    {
        response.ResponseCode = responseCode;
        response.ContentType = "text/html";
        if (_configuration.CustomErrorHandlers.ContainsKey(responseCode))
        {
            await response.WriteAsync(_configuration.CustomErrorHandlers[responseCode]);
        }
        else
        {
            HtmlBuilder builder = new HtmlBuilder(responseCode.ToString());
            builder.AppendHeader(1, $"{(int)responseCode} - {responseCode}");
            builder.AppendHr();
            builder.AppendParagraph($"More info about this issue <a href=\"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{(int)responseCode}\">can be found here</a>");
            await response.WriteAsync(builder.ToString());
        }
    }
}
