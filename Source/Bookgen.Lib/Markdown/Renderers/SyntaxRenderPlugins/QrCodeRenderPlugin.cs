//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Markdown.RenderInterop;

namespace Bookgen.Lib.Markdown.Renderers.SyntaxRenderPlugins;

internal sealed class QrCodeRenderPlugin : SyntaxRendererPlugin
{
    private readonly IRenderInterop _renderInterop;

    public QrCodeRenderPlugin(IRenderInterop renderInterop)
    {
        _renderInterop = renderInterop;
    }

    public override string LanguageMoniker { get; } = "qrcode";

    public override string Render(string code)
    {
        return RendererImgage(_renderInterop.RenderQrCode(code));
    }
}
