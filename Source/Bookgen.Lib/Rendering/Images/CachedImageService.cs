//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Caching.Memory;

namespace Bookgen.Lib.Rendering.Images;

public sealed class CachedImageService : IImgService
{
    private readonly IImgService _service;
    private readonly IMemoryCache _memoryCache;

    public CachedImageService(IImgService service, IMemoryCache memoryCache)
    {
        _service = service;
        _memoryCache = memoryCache;
    }

    private static ulong GetCacheKey(string str)
    {
        const ulong ofset = 0xcbf29ce484222325;
        const ulong prime = 0x00000100000001b3;

        ulong hash = ofset;

        foreach (char c in str)
        {
            hash ^= c;
            hash *= prime;
        }

        return hash;
    }

    public ImageResult GetImageEmbedData(string path)
    {
        ulong cacheKey = GetCacheKey(path);
        return _memoryCache.GetOrCreate(cacheKey, entry =>
        {

            entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(180));
            return _service.GetImageEmbedData(path);
        })!;
    }
}
