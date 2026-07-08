//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Lib.Rendering.Images;

public interface IImgService
{
    ImageResult GetImageEmbedData(string filePath);
}
