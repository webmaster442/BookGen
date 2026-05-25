using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.ImageService;

namespace Bookgen.Lib.Markdown.RenderInterop;

internal interface IRenderInterop : IDisposable
{
    ImageResult RenderNomnoml(string nomnomlCode, ImageConfig imageConfig);
    ImageResult RenderLatex(string latex, ImageConfig imageConfig);
    ImageResult RenderQrCode(string url, ImageConfig imageConfig);
    string PrismSyntaxHighlight(string code, string language);
}
