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
    public async Task<bool> Handle(IServerLog? log, HttpRequest request, HttpResponse response)
    {
        if (request.Method != RequestMethod.Get
            || request.Url != "/favicon.ico")
        {
            return false;
        }

        log?.Info("Serving favicon...");

        _iconStream.Seek(0, SeekOrigin.Begin);
        response.ContentType = _mime;
        response.ResponseCode = HttpResponseCode.Ok;
        await response.Write(_iconStream);

        return true;
    }
}
