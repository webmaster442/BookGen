//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace BookGen.Cli.CrashReporting;

internal static class MiniDumpGenerator
{
    public static void WriteDump(string filePath)
    {
        var process = System.Diagnostics.Process.GetCurrentProcess();

        if (OperatingSystem.IsWindows())
        {
            WriteDump(filePath, process);
        }
        else if (OperatingSystem.IsLinux())
        {
            EnableSelfTrace();
            WriteDump(filePath, process);
        }

    }

    [DllImport("libc", EntryPoint = "prctl", SetLastError = true)]
    private static extern int Prctl(int option, IntPtr arg2, IntPtr arg3, IntPtr arg4, IntPtr arg5);

    [SupportedOSPlatform("linux")]
    private static void EnableSelfTrace()
    {
        try
        {
            const int PR_SET_PTRACER = 0x59616d61;
            const nint PR_SET_PTRACER_ANY = -1;
            _ = Prctl(PR_SET_PTRACER, PR_SET_PTRACER_ANY, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }
        catch (Exception ex) when (ex is DllNotFoundException or EntryPointNotFoundException)
        {
            // libc/prctl isn't resolvable this way on this platform (e.g. musl/Alpine)
            // fall through and let createdump itself report any real permission failure.
        }
    }

    private static void WriteDump(string filePath, Process process)
    {
        using Process createDump = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "createdump"),
                ArgumentList = { "--withheap", "-f", filePath, process.Id.ToString() },
                UseShellExecute = false,
            }
        };
        createDump.Start();
        createDump.WaitForExit();

    }
}
