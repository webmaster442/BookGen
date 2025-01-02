//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using Microsoft.Extensions.Logging;

namespace BookGen.DomainServices;
public sealed class BrowserInteract
{
    private readonly string? _chromePath;
    private readonly ILogger _logger;

    public BrowserInteract(ILogger logger)
    {
        var paths = Environment.ExpandEnvironmentVariables("%path%").Split(';');
        foreach (var path in paths)
        {
            var test = Path.Combine(path, "chrome.exe");
            if (File.Exists(test))
            {
                _chromePath = test;
                break;
            }
        }

        _logger = logger;
    }

    private async Task RunBrowser(IReadOnlyList<string> arguments)
    {
        using var process = new Process();
        process.StartInfo.FileName = _chromePath ?? "msedge.exe";
        process.StartInfo.Arguments = string.Join(' ', arguments);
        process.StartInfo.UseShellExecute = _chromePath == null;
        process.Start();
        await process.WaitForExitAsync();
    }

    private static string NormalizeUrlOrThrow(string url)
    {
        if (url.StartsWith("http://")
            || url.StartsWith("https://")
            || url.StartsWith("file://"))
        {
            return url;
        }

        return File.Exists(url)
            ? $"file:///{url}"
            : throw new ArgumentException($"Invalid URL: {url}");
    }

    public async Task<bool> Html2Pdf(string input, string output)
    {
        try
        {
            var normalized = NormalizeUrlOrThrow(input);

            var arguments = new[]
            {
                "--headless",
                "--disable-gpu",
                $"--print-to-pdf=\"{output}\"",
                "--disable-extensions",
                "--no-pdf-header-footer",
                "--disable-popup-blocking",
                "--run-all-compositor-stages-before-draw",
                "--disable-checker-imaging",
                normalized
            };

            await RunBrowser(arguments);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Critical Error");
            return false;
        }
    }

    public async Task<bool> Html2Png(string input,
                                     string output,
                                     int width,
                                     int height)
    {
        try
        {
            var normalized = NormalizeUrlOrThrow(input);

            var arguments = new[]
            {
                "--headless",
                "--disable-gpu",
                $"--screenshot=\"{output}\"",
                "--disable-extensions",
                "--no-pdf-header-footer",
                "--disable-popup-blocking",
                "--run-all-compositor-stages-before-draw",
                "--disable-checker-imaging",
                $"--window-size={width}x{height}",
                normalized
            };
            
            await RunBrowser(arguments);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Critical Error");
            return false;
        }
    }
}
