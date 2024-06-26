//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using WpfMath.Converters;
using WpfMath.Parsers;
using WpfMath.Rendering;

using XamlMath;

namespace BookGen.FormulaEdit.AppLogic;

internal static class Renderer
{
    private const int Scale = 20;

    private static (TexFormula parsedFormula, TexEnvironment environment) Create(string formula, double scale)
    {
        TexFormula parsedFormula = WpfTeXFormulaParser.Instance.Parse(formula);
        TexEnvironment environment = WpfTeXEnvironment.Create(style: TexStyle.Display, scale: scale, systemTextFontName: "Arial");
        return (parsedFormula, environment);
    }

    private static void RenderPng(string formula, Stream target)
    {
        var (parsedFormula, environment) = Create(formula, Scale);

        var bitmapSource = parsedFormula.RenderToBitmap(environment);
        PngBitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
        encoder.Save(target);
    }

    private static void RenderSvg(string formula, Stream target)
    {
        var (parsedFormula, environment) = Create(formula, Scale);

        var geometry = parsedFormula.RenderToGeometry(environment, scale: Scale);
        var converter = new SVGConverter();

        var width = geometry.Bounds.Width.ToString(CultureInfo.InvariantCulture);
        var height = geometry.Bounds.Height.ToString(CultureInfo.InvariantCulture);

        var svgPathText = converter.ConvertGeometry(geometry);

        using (var writer = new StreamWriter(target, leaveOpen: true))
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            writer.WriteLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\" width=\"{width}\" height=\"{height}\" >");
            writer.WriteLine(svgPathText);
            writer.WriteLine("</svg>");
        }
    }

    public static void RenderTo(string formula, string fileName, RenderFormat format)
    {
        switch (format)
        {
            case RenderFormat.Png:
                using (var stream = File.Create(fileName))
                {
                    RenderPng(formula, stream);
                }
                break;
            case RenderFormat.Svg:
                using (var stream = File.Create(fileName))
                {
                    RenderSvg(formula, stream);
                }
                break;
            default:
                throw new InvalidOperationException("Invalid render format");
        }
    }

    public static int RenderAllTo(string directory, string baseName, RenderFormat format, IEnumerable<string> formulas)
    {
        int counter = 0;
        foreach (var formula in formulas)
        {
            var fileName = $"{baseName}_{counter++}.{format.GetExtension()}";
            fileName = Path.Combine(directory, fileName);
            RenderTo(formula, fileName, format);
        }
        return counter;
    }
}
