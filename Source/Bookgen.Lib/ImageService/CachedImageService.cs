using System.Collections.Concurrent;

namespace Bookgen.Lib.ImageService;

public sealed class CachedImageService : IImgService
{
    private readonly IImgService _service;
    private readonly ConcurrentDictionary<string, (string data, ImageType imageType)> _cache;

    public CachedImageService(IImgService service)
    {
        _service = service;
        _cache = new ConcurrentDictionary<string, (string data, ImageType imageType)>();
    }

    public (string data, ImageType imageType) GetImageEmbedData(string path)
    {
        if (_cache.TryGetValue(path, out var data))
            return data;

        var result = _service.GetImageEmbedData(path);
        _cache.TryAdd(path, result);
        return result;
    }
}
