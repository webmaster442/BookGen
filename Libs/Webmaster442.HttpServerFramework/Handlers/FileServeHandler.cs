﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2023 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using BookGen.Api;

using Webmaster442.HttpServerFramework.Domain;
using Webmaster442.HttpServerFramework.Internal;

namespace Webmaster442.HttpServerFramework.Handlers;

/// <summary>
/// Basic file handler.
/// </summary>
public class FileServeHandler : IRequestHandler
{
    private readonly string _path;
    private readonly string _mountPath;
    private readonly bool _sendLastAccesTime;
    private readonly bool _listFolders;

    /// <summary>
    /// List of files that are probed to serve, if the request is a folder;
    /// </summary>
    public string[] IndexFiles { get; }

    /// <summary>
    /// Creates a new instance of FileServeHandler
    /// </summary>
    /// <param name="path">Path in the file system to serve from.</param>
    /// <param name="mountPath">Mount path on server</param>
    /// <param name="listFolders">Allow listing of folders, if no index page is present</param>
    /// <param name="configuration">HTTP server configuration</param>
    public FileServeHandler(string path,
                            bool listFolders,
                            HttpServerConfiguration configuration,
                            string mountPath = "/")
    {
        _path = path;
        _mountPath = mountPath;
        _sendLastAccesTime = configuration.EnableLastAccesTime;
        _listFolders = listFolders;
        IndexFiles = new[]
        {
                "index.html",
                "index.htm",
                "default.html",
                "default.htm"
        };

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
        if (_listFolders)
        {
            return _path;
        }
        throw new ServerException(HttpResponseCode.NotFound);
    }


    /// <inheritdoc/>
    public async Task<bool> Handle(ILog? log, HttpRequest request, HttpResponse response)
    {
        if (request.Method != RequestMethod.Get)
        {
            return false;
        }

        if (request.Url.StartsWith(_mountPath))
        {
            string filename = request.Url.Substring(_mountPath.Length);
            var fileOnDisk = Path.Combine(_path, filename);
            if (string.IsNullOrEmpty(filename))
            {
                fileOnDisk = GetIndexFile(fileOnDisk);
            }

            if (Directory.Exists(fileOnDisk))
            {
                fileOnDisk = GetIndexFile(fileOnDisk);
            }

            if (Directory.Exists(fileOnDisk) && _listFolders)
            {
                await RenderFolder(fileOnDisk, request.Url, log, response);
                return true;
            }

            if (File.Exists(fileOnDisk))
            {
                DateTime lastModifiedDate = File.GetLastWriteTimeUtc(fileOnDisk);
                using (var stream = File.OpenRead(fileOnDisk))
                {
                    log?.Info("Serving {0}...", request.Url);
                    response.LastModified = _sendLastAccesTime ? lastModifiedDate : DateTime.UtcNow;
                    response.ContentType = MimeTypes.GetMimeTypeForFile(fileOnDisk);
                    response.ResponseCode = HttpResponseCode.Ok;
                    await response.WriteAsync(stream);
                }
                return true;
            }
        }
        return false;
    }

    private async ValueTask RenderFolder(string folder, string url, ILog? log, HttpResponse response)
    {
        log?.Info("Rendering file list for: {0}...", folder);
        string title = $"Index of {url}";

        HtmlBuilder builder = new HtmlBuilder(title);
        builder.AppendHeader(1, title);
        builder.AppendHr();

        builder.AppendHeader(2, "Directories:");

        builder.UnorderedList(Directory.GetDirectories(folder), item =>
        {
            return $"<a href=\"{GetUrl(url, item)}\">{Path.GetFileName(item)}</a>";
        });

        builder.AppendHeader(2, "Files:");

        builder.UnorderedList(Directory.GetFiles(folder), item =>
        {
            return $"<a href=\"{GetUrl(url, item)}\">{Path.GetFileName(item)}</a>";
        });

        response.ContentType = "text/html; charset=utf-8";
        response.ResponseCode = HttpResponseCode.Ok;
        await response.WriteAsync(builder.ToString());
    }

    private static string GetUrl(string baseUrl, string item)
    {
        var url = Path.Combine(baseUrl, Path.GetFileName(item));
        return url.Replace('\\', '/');
    }
}
