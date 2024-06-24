﻿using System.IO;
using System.Windows.Media.Imaging;

using WpfMath.Converters;
using WpfMath.Parsers;
using WpfMath.Rendering;

using XamlMath;

namespace BookGen.FormulaEdit.AppLogic;

internal static class Renderer
{
    private static (TexFormula parsedFormula, TexEnvironment environment) Create(string formula, double scale)
    {
        TexFormula parsedFormula = WpfTeXFormulaParser.Instance.Parse(formula);
        TexEnvironment environment = WpfTeXEnvironment.Create(style: TexStyle.Display, scale: 20.0, systemTextFontName: "Arial");
        return (parsedFormula, environment);
    }

    private static void RenderPng(string formula, Stream target)
    {
        var (parsedFormula, environment) = Create(formula, 20);

        var bitmapSource = parsedFormula.RenderToBitmap(environment);
        PngBitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
        encoder.Save(target);
    }

    private static void RenderSvg(string formula, Stream target)
    {
        var (parsedFormula, environment) = Create(formula, 20);

        var geometry = parsedFormula.RenderToGeometry(environment, scale: 20);
        var converter = new SVGConverter();
        var svgPathText = converter.ConvertGeometry(geometry);

        using (var writer = new StreamWriter(target, leaveOpen: true))
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            writer.WriteLine("<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\" >");
            writer.WriteLine(svgPathText);
            writer.WriteLine("</svg>");
        }
    }
}
