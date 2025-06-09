using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using BookGen.Vfs;

namespace Bookgen.Lib.JsInterop;

public sealed class MathJaxInterop : JavascriptInterop
{
    private readonly dynamic TypeSet;

    public MathJaxInterop(IAssetSource assetSource)
    {
        string mathJax = assetSource.GetAsset(BundledAssets.MathJax);
        Execute(mathJax);
        TypeSet = Evaluate("MathJaxModule.typeset");
    }

    public string RenderLatexToSvg(string latex, double scale = 1.0)
    {
        dynamic svg = TypeSet(latex);

        var xml = XDocument.Load(new StringReader(svg));
        
        if (xml.Root == null)
            throw new InvalidOperationException("SVG root element is null.");

        string w = MakeScale(xml.Root.Attribute("width"), scale);
        string h = MakeScale(xml.Root.Attribute("height"), scale);
        xml.Root.SetAttributeValue("width", w);
        xml.Root.SetAttributeValue("height", h);

        return xml.ToString();
    }

    private static string MakeScale(XAttribute? xAttribute, double scale)
    {
        var rawValue = xAttribute?.Value ?? throw new InvalidOperationException("SVG has no width or hegiht");
        StringBuilder size = new();
        StringBuilder unit = new();
        foreach (var c in rawValue)
        {
            if (char.IsDigit(c) || c == '.' || c == '-')
                size.Append(c);
            else
                unit.Append(c);
        }

        double parsed = double.Parse(size.ToString(), CultureInfo.InvariantCulture);
        double scaled = parsed * scale;
        return $"{scaled.ToString(CultureInfo.InvariantCulture)}{unit}";
    }
}
