namespace Bookgen.Lib.ImageService;

public interface IImgService
{
    (string base64data, ImageType imageType) GetImageEmbedData(string path);
}