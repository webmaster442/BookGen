//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Globalization;

namespace Bookgen.Lib.Markdown.RenderInterop;

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
        string outout = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        return process.ExitCode != 0
            ? throw new InvalidOperationException($"{binaryName} process exited with code {process.ExitCode}")
            : outout;
    }

    public static string RunRatex(string input, double scale)
        => RunBinaryAndCaptureStdOut("ratex-svg", $"--stdout --dpr {scale.ToString(CultureInfo.InvariantCulture)}", input);

    public static string RunMmmdr(string input)
        => RunBinaryAndCaptureStdOut("mmdr", "-e svg --nodeSpacing 60 --rankSpacing 80 -i -", input);
}
