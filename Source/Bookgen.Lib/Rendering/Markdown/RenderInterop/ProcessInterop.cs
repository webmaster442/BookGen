//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Globalization;
using System.Web;

namespace Bookgen.Lib.Rendering.Markdown.RenderInterop;

internal static class ProcessInterop
{
    private static string GetBinary(string name)
    {
        return OperatingSystem.IsWindows()
            ? Path.Combine(AppContext.BaseDirectory, $"{name}.exe")
            : Path.Combine(AppContext.BaseDirectory, name);
    }

    private static string RunBinaryAndCaptureStdOut(string binaryName, string arguments, string stdin)
    {
        var binary = GetBinary(binaryName);

        if (!File.Exists(binary))
        {
            return ErrorSvg($"{binaryName} not found in bookgen folder");
        }

        if (OperatingSystem.IsLinux()
            || OperatingSystem.IsMacOS())
        {
            UnixFileMode permissions = File.GetUnixFileMode(binary);
            if (!permissions.HasFlag(UnixFileMode.UserExecute))
            {
                permissions |= UnixFileMode.UserExecute;
                File.SetUnixFileMode(binary, permissions);
            }
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = binary,
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

    public static string RunRatex(string input, double scale)
        => RunBinaryAndCaptureStdOut("ratex-svg", $"--stdout --dpr {scale.ToString(CultureInfo.InvariantCulture)}", input);

    public static string RunMmmdr(string input)
        => RunBinaryAndCaptureStdOut("mmdr", "-e svg --nodeSpacing 60 --rankSpacing 80 -i -", input);

    public static string RunPlantuml(string plantUml)
        => RunBinaryAndCaptureStdOut("plantuml", "--svg -pipe", plantUml);
}
