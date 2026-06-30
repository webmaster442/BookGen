//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Rendering.Images;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.SyntaxRenderPlugins;

internal abstract class SyntaxRendererPlugin
{
    public abstract string LanguageMoniker { get; }

    public abstract string Render(string code);

    protected static string RendererImgage(ImageResult img)
    {
        return img.ImageType == ImageType.Svg
            ? img.Data
            : $"<img src=\"data:{img.ImageType.GetMimeType()};base64,{img.Data}\">";
    }
}
