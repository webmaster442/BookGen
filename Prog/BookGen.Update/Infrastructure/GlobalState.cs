//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.InteropServices;

using BookGen.Update.Dto;

using static BookGen.Update.ShellCommands.ShellFileGenerator;

namespace BookGen.Update.Infrastructure;

internal sealed class GlobalState
{
    public List<string> Issues { get; }
    public Release[] Releases { get; set; }
    public Release Latest { get; set; }
    public string TempFile { get; set; }
    public string TargetDir { get; }

    public List<(string source, string target)> PostProcessFiles { get; }

    public ShellType ShellType { get; }

    public string UpdateShellFileName { get; }

    public GlobalState()
    {
        Issues = new List<string>();
        Releases = Array.Empty<Release>();
        Latest = new Release();
        TempFile = string.Empty;
        TargetDir = AppContext.BaseDirectory;
        PostProcessFiles = new List<(string source, string target)>();
        ShellType = Detect();
        UpdateShellFileName = GetFileName();
    }

    private string GetFileName()
    {
        string name = ShellType == ShellType.Bash ? "update.sh" : "update.ps1";
        return Path.Combine(TargetDir, name);
    }

    public void Cleanup()
    {
        if (File.Exists(TempFile))
        {
            File.Delete(TempFile);
        }
    }

    private static ShellType Detect()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)
            || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return ShellType.Bash;
        }
        else
        {
            return ShellType.Powershell;
        }
    }
}
