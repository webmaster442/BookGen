using System.Diagnostics;

namespace Bookgen.Lib.Markdown.RenderInterop;

internal static class ProcessInterop
{
    private static string GetBinary(string name)
    {
        return OperatingSystem.IsWindows()
            ? Path.Combine(AppContext.BaseDirectory, $"{name}.exe")
            : Path.Combine(AppContext.BaseDirectory, name);
    }

    public static string RunRatex(string input)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = GetBinary("ratex-svg"),
                Arguments = "--stdout",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.StandardInput.Write(input);
        process.StandardInput.Close();
        string outout = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        return process.ExitCode != 0
            ? throw new InvalidOperationException($"Ratex process exited with code {process.ExitCode}")
            : outout;
    }
}
