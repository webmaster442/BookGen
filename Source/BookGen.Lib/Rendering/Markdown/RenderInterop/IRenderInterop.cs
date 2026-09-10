//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Domain.IO.Configuration;
using BookGen.Lib.Rendering.Images;

using BookGen.Vfs;

namespace BookGen.Lib.Rendering.Markdown.RenderInterop;

public interface IRenderInterop : IDisposable
{
    ImageResult RenderNomnoml(string nomnomlCode);
    ImageResult RenderLatex(string latex, double scale = 1.0);
    ImageResult RenderMermaid(string mermaid);
    ImageResult RenderQrCode(string url);
    ImageResult RenderPlantUml(string plantUml);
    string PrismSyntaxHighlight(string code, string language);
    bool PreRenderCode { get; set; }

    public static IRenderInterop CreateForSvg(IAssetSource assetSource, IProgramPathResolver programPathResolver)
    {
        return new RenderInterop(assetSource, programPathResolver, new ImageConfig
        {
            SvgRecode = SvgRecodeOption.Passtrough,
        });
    }
}
