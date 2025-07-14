namespace Bookgen.Lib.ImageService;

public sealed record class ImageResult
{
    public required ImageType ImageType { get; set; }
    public required string Data { get; set; }
    public required string OriginalName { get; set; }
}