using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Vfs;

namespace Bookgen.Lib.ImageService;

internal sealed class CachedImageService : IImgService
{
    private readonly Dictionary<string, (string base64data, ImageType imageType)> _cache;
    private readonly ImgService _imgService;

    public CachedImageService(IReadOnlyFileSystem sourceFolder, ImageConfig imageConfig)
    {
        _imgService = new(sourceFolder, imageConfig);
        _cache = new Dictionary<string, (string base64data, ImageType imageType)>();
    }

    public (string data, ImageType imageType) GetImageEmbedData(string path)
    {
        if (_cache.ContainsKey(path))
            return _cache[path];

        var data = _imgService.GetImageEmbedData(path);
        _cache.Add(path, data);

        return data;
    }
}
