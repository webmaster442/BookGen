namespace Webmaster442.WindowsTerminal;

/// <summary>
/// Detects installed applications
/// </summary>
public static class InstallDetector
{
    /// <summary>
    /// The name of the Windows Terminal executable
    /// </summary>
    public const string WindowsTerminalExe = "wt.exe";
    /// <summary>
    /// The name of the Powershell Core executable
    /// </summary>
    public const string PowershellCoreExe = "pwsh.exe";
    /// <summary>
    /// The name of the Visual Studio Code executable
    /// </summary>
    public const string VsCodeExe = "code.cmd";

    private static void IfNotSetCheck(ref string? varable, string path, string exeName)
    {
        if (!string.IsNullOrEmpty(varable))
            return;
        var check = Path.Combine(path, exeName);
        if (File.Exists(check))
        {
            varable = check;
        }
    }

    /// <summary>
    /// Gets the installation status of various applications
    /// </summary>
    /// <returns>An InstallResult</returns>
    public static InstallResult GetInstallResult()
    {
        string? terminalPath = null;
        string? psCorePath = null;
        string? vsCodePath = null;

        string[] pathDirs = Environment.GetEnvironmentVariable("path")?.Split(';') ?? Array.Empty<string>();
        foreach (string? dir in pathDirs)
        {
            if (string.IsNullOrEmpty(dir)) continue;

            IfNotSetCheck(ref terminalPath, dir, WindowsTerminalExe);
            IfNotSetCheck(ref vsCodePath, dir, VsCodeExe);
            IfNotSetCheck(ref psCorePath, dir, PowershellCoreExe);
        }

        return new InstallResult
        {
            TerminalPath = terminalPath,
            PowershellCorePath = psCorePath,
            VsCodePath = vsCodePath
        };
    }
}
