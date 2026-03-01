//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.ImageService;

public interface IImgService
{
    ImageResult GetImageEmbedData(string filePath);
    ImageResult EncodeSvg(string svgData);
}
