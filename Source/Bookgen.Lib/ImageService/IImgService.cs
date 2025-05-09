namespace Bookgen.Lib.ImageService;

public interface IImgService
{
    (string data, ImageType imageType) GetImageEmbedData(string path);
}