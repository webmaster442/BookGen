//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Concurrent;

using BookGen.Vfs;

namespace Bookgen.Lib.ImageService;

public sealed class CachedImageService : IImgService
{
    private readonly IImgService _service;
    private readonly ConcurrentDictionary<string, ImageResult> _cache;

    public CachedImageService(IImgService service)
    {
        _service = service;
        _cache = new ConcurrentDictionary<string, ImageResult>();
    }

    public ImageResult GetImageEmbedData(string path)
    {
        if (_cache.TryGetValue(path, out var data))
            return data;

        var result = _service.GetImageEmbedData(path);
        _cache.TryAdd(path, result);
        return result;
    }
}
