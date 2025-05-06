using Bookgen.Lib.Domain.IO.Configuration;

namespace Bookgen.Lib.ImageService;

internal sealed class CachedImageService : ImgService
{
    private readonly Dictionary<string, (string base64data, ImageType imageType)> _cache;

    public CachedImageService(VFS.IFolder sourceFolder, ImageConfig imageConfig) : base(sourceFolder, imageConfig)
    {
        _cache = new Dictionary<string, (string base64data, ImageType imageType)>();
    }

    public override (string base64data, ImageType imageType) GetImageEmbedData(string path)
    {
        if (_cache.ContainsKey(path))
            return _cache[path];

        var data = base.GetImageEmbedData(path);
        _cache.Add(path, data);

        return data;
    }
}
