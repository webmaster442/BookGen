//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.ImageService;

using BookGen.Vfs;

namespace Bookgen.Lib.Markdown.RenderInterop;

public interface IRenderInterop : IDisposable
{
    ImageResult RenderNomnoml(string nomnomlCode);
    ImageResult RenderLatex(string latex, double scale = 1.0);
    ImageResult RenderMermaid(string mermaid);
    ImageResult RenderQrCode(string url);
    string PrismSyntaxHighlight(string code, string language);
    bool PreRenderCode { get; set; }

    public static IRenderInterop CreateForSvg(IAssetSource assetSource)
    {
        return new RenderInterop(assetSource, new ImageConfig
        {
            SvgRecode = SvgRecodeOption.Passtrough,
        });
    }
}
