//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Globalization;
using System.Web;

using Bookgen.Lib.AppSettings;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Rendering.Images;

using BookGen.Vfs;

using SkiaSharp;

namespace Bookgen.Lib.Rendering.Markdown.RenderInterop;

public sealed class RenderInterop : IRenderInterop
{
    private readonly IAssetSource _assetSource;
    private readonly IProgramPathResolver _programPathResolver;
    private readonly JavascriptEngine _javascriptEngine;
    private readonly HashSet<string> _loadedScripts;
    private bool _disposed;

    private readonly ImageConfig _imageConfig;

    public RenderInterop(IAssetSource assetSource,
                         IProgramPathResolver programPathResolver,
                         ImageConfig imageConfig)
    {
        _assetSource = assetSource;
        _programPathResolver = programPathResolver;
        _imageConfig = imageConfig;
        _javascriptEngine = new JavascriptEngine(assetSource);
        _loadedScripts = new HashSet<string>();
    }

    public void Dispose()
    {
        _javascriptEngine.Dispose();
        _disposed = true;
    }

    private void LoadScriptIfNotLoaded(string scriptFile)
    {
        if (_loadedScripts.Contains(scriptFile))
        {
            return;
        }

        string script = _assetSource.GetAsset(scriptFile);
        _javascriptEngine.Execute(script);
        _loadedScripts.Add(scriptFile);
    }

    private static ImageType GetImateType(SvgRecodeOption recodeOption)
    {
        return recodeOption switch
        {
            SvgRecodeOption.AsPng => ImageType.Png,
            SvgRecodeOption.AsWebp => ImageType.Webp,
            SvgRecodeOption.Passtrough => ImageType.Svg,
            _ => throw new UnreachableException(),
        };
    }

    private static ImageResult EncodeSvg(string svgData, ImageConfig imageConfig)
    {
        if (imageConfig.SvgRecode == SvgRecodeOption.Passtrough)
        {
            return new ImageResult
            {
                Data = svgData,
                ImageType = ImageType.Svg,
                OriginalName = string.Empty,
            };
        }



        using SKData rendered = ImageUtils.RenderSvg(svgData,
                                                     imageConfig.ResizeWith,
                                                     imageConfig.ResizeHeight,
                                                     imageConfig.SvgRecode);

        return new ImageResult
        {
            Data = Convert.ToBase64String(rendered.AsSpan()),
            ImageType = GetImateType(imageConfig.SvgRecode),
            OriginalName = string.Empty
        };
    }

    private static string RunBinaryAndCaptureStdOut(string binaryName, string arguments, string stdin)
    {
        if (!File.Exists(binaryName))
        {
            return ErrorSvg($"{binaryName} not found in bookgen folder");
        }

        if (OperatingSystem.IsLinux()
            || OperatingSystem.IsMacOS())
        {
            UnixFileMode permissions = File.GetUnixFileMode(binaryName);
            if (!permissions.HasFlag(UnixFileMode.UserExecute))
            {
                permissions |= UnixFileMode.UserExecute;
                File.SetUnixFileMode(binaryName, permissions);
            }
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = binaryName,
                Arguments = arguments,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.StandardInput.Write(stdin);
        process.StandardInput.Close();
        string output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        return Validate(process.ExitCode, output, binaryName);
    }

    private static string Validate(int exitCode, string output, string binaryName)
    {
        if (exitCode != 0)
        {
            return ErrorSvg($"{binaryName} exited with code {exitCode}");
        }

        return !output.Contains("<svg") || !output.Contains("</svg>")
            ? ErrorSvg($"{binaryName} did not return valid SVG output")
            : output;
    }

    private static string ErrorSvg(string message)
    {
        return $$"""
            <?xml version="1.0" encoding="UTF-8" standalone="no"?>
            <svg
              width="187.98837mm"
              height="20.16725mm"
              viewBox="0 0 187.98837 20.16725"
              version="1.1"
              id="svg1"
              xmlns="http://www.w3.org/2000/svg"
              xmlns:svg="http://www.w3.org/2000/svg">
              <g id="layer1" transform="translate(-11.00753,-1.0563699)">
                <text
                  xml:space="preserve"
                  style="font-style:normal;font-variant:normal;font-weight:normal;font-stretch:normal;font-size:5.64444px;font-family:Arial;-inkscape-font-specification:Arial;text-align:center;writing-mode:lr-tb;direction:ltr;text-anchor:middle;fill:#ff0500;fill-opacity:1;stroke:#000000;stroke-width:0"
                  x="105.00688"
                  y="11.157382"
                  id="text1">
            	   <tspan
                     id="tspan1"
                     style="font-style:normal;font-variant:normal;font-weight:normal;font-stretch:normal;font-size:14.1111px;font-family:Arial;-inkscape-font-specification:Arial;fill:#ff0500;fill-opacity:1;stroke-width:0"
                     x="105.00688"
                     y="11.157382">Rendering Failed</tspan>
            		<tspan
                     style="font-style:normal;font-variant:normal;font-weight:normal;font-stretch:normal;font-size:4.23333px;font-family:Arial;-inkscape-font-specification:Arial;fill:#ff0500;fill-opacity:1;stroke-width:0"
                     x="105.00688"
                     y="21.169876"
                     id="tspan2">{{HttpUtility.HtmlEncode(message)}}</tspan>
            		 </text>
              </g>
            </svg>
            """;
    }


    public bool PreRenderCode { get; set; } = true;

    public string PrismSyntaxHighlight(string code, string language)
    {
        if (PreRenderCode)
        {
            ObjectDisposedException.ThrowIf(_disposed, nameof(RenderInterop));
            LoadScriptIfNotLoaded(BundledAssets.PrismJs);

            _javascriptEngine.Script.code = code;
            return _javascriptEngine.ExecuteAndGetResult($"Prism.highlight(code, Prism.languages.{language}, '{language}');");
        }

        return HttpUtility.HtmlEncode(code);
    }

    public ImageResult RenderNomnoml(string nomnomlCode)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RenderInterop));
        LoadScriptIfNotLoaded(BundledAssets.GraphreJs);
        LoadScriptIfNotLoaded(BundledAssets.NomnomlJs);

        _javascriptEngine.Script.nomnomlCode = nomnomlCode;
        string svg = _javascriptEngine.ExecuteAndGetResult("nomnoml.renderSvg(nomnomlCode)");
        return EncodeSvg(svg, _imageConfig);
    }

    public ImageResult RenderQrCode(string url)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RenderInterop));
        LoadScriptIfNotLoaded(BundledAssets.QrCodeJs);

        string cmd = $"new QRCode({{content: \"{url}\", padding: 2, color: \"#000000\"}}).svg();";
        string svg = _javascriptEngine.ExecuteAndGetResult(cmd);
        return EncodeSvg(svg, _imageConfig);
    }

    public ImageResult RenderLatex(string latex, double scale = 1.0)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RenderInterop));

        if (!_programPathResolver.TryResolveRatex(out string? ratexPath))
        {
            return EncodeSvg(ErrorSvg("Ratex binary not found"), _imageConfig);
        }

        string svg = RunBinaryAndCaptureStdOut(ratexPath, $"--stdout --dpr {scale.ToString(CultureInfo.InvariantCulture)}", latex);
        return EncodeSvg(svg, _imageConfig);

    }

    public ImageResult RenderMermaid(string mermaid)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RenderInterop));

        if (!_programPathResolver.TryResolveMmdr(out string? mmmdrPath))
        {
            return EncodeSvg(ErrorSvg("Mmmdr binary not found"), _imageConfig);
        }

        string svg = RunBinaryAndCaptureStdOut(mmmdrPath, "-e svg --nodeSpacing 60 --rankSpacing 80 -i -", mermaid);
        return EncodeSvg(svg, _imageConfig);
    }

    public ImageResult RenderPlantUml(string plantUml)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RenderInterop));

        if (!_programPathResolver.TryResolvePlantUml(out string? plantUmlPath))
        {
            return EncodeSvg(ErrorSvg("PlantUML binary not found"), _imageConfig);
        }

        string svg = RunBinaryAndCaptureStdOut(plantUmlPath, "--svg -pipe", plantUml);
        return EncodeSvg(svg, _imageConfig);
    }
}
