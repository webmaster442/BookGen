﻿//-----------------------------------------------------------------------------
// (c) 2021-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Resources;

using Webmaster442.HttpServerFramework;
using Webmaster442.HttpServerFramework.Domain;

namespace BookGen.Framework.Server;

internal sealed class PreviewStaticHandler : IRequestHandler
{
    private readonly Dictionary<string, KnownFile> _table;

    public PreviewStaticHandler()
    {
        _table = new()
        {
            { "/prism.js", KnownFile.PrismJs },
            { "/prism.css", KnownFile.PrismCss },
            { "/preview.css", KnownFile.PreviewCss },
            { "/edit.css", KnownFile.EditCss },
        };
    }


    private bool CanServe(string AbsoluteUri)
    {
        return _table.ContainsKey(AbsoluteUri);
    }

    public async Task<bool> Handle(ILogger log, HttpRequest request, HttpResponse response)
    {
        if (CanServe(request.Url))
        {
            log.LogInformation("Serving: {url}...", request.Url);
            string? file = ResourceHandler.GetFile(_table[request.Url]);
            response.ContentType = MimeTypes.GetMimeTypeForFile(request.Url);
            await response.WriteAsync(file);
            return true;
        }
        return false;
    }
}
