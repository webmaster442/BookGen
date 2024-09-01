// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2024 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

using Webmaster442.HttpServerFramework.Domain;

namespace Webmaster442.HttpServerFramework.Handlers;

/// <summary>
/// A file handler for /favicon.ico requests
/// </summary>
public sealed class FaviconHandler : IRequestHandler, IDisposable
{
    private readonly Stream _iconStream;
    private readonly string _mime;

    /// <summary>
    /// Creates a new instance of FaviconHandler
    /// </summary>
    /// <param name="iconStream">icon resource stream</param>
    /// <param name="mime">icon resoruce mime</param>
    public FaviconHandler(Stream iconStream, string mime)
    {
        _iconStream = iconStream;
        _mime = mime;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _iconStream.Dispose();
    }

    /// <inheritdoc/>
    public async Task<bool> Handle(ILogger logger, HttpRequest request, HttpResponse response)
    {
        if (request.Method != RequestMethod.Get
            || request.Url != "/favicon.ico")
        {
            return false;
        }

        logger.LogInformation("Serving favicon...");

        _iconStream.Seek(0, SeekOrigin.Begin);
        response.ContentType = _mime;
        response.ResponseCode = HttpResponseCode.Ok;
        await response.WriteAsync(_iconStream);

        return true;
    }
}
