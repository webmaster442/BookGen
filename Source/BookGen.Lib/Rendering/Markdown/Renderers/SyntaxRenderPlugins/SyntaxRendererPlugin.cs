//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Rendering.Images;

namespace BookGen.Lib.Rendering.Markdown.Renderers.SyntaxRenderPlugins;

internal abstract class SyntaxRendererPlugin
{
    public abstract string[] LanguageMonikers { get; }

    public abstract string Render(string code, string parsedLanguageMoniker);

    protected static string RendererImgage(ImageResult img)
    {
        return img.ImageType == ImageType.Svg
            ? img.Data
            : $"<img src=\"data:{img.ImageType.GetMimeType()};base64,{img.Data}\">";
    }
}
