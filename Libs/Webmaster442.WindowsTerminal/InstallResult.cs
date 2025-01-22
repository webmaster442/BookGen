namespace Webmaster442.WindowsTerminal;

/// <summary>
/// Represents the Installation status of various applications
/// </summary>
public sealed record class InstallResult
{
    /// <summary>
    /// The path to the Windows Terminal executable
    /// </summary>
    public string? TerminalPath { get; init; }

    /// <summary>
    /// The path to the Powershell Core executable
    /// </summary>
    public string? PowershellCorePath { get; init; }

    /// <summary>
    /// The path to the Visual Studio Code executable
    /// </summary>
    public string? VsCodePath { get; init; }

    /// <summary>
    /// Indicates if Windows Terminal is installed
    /// </summary>
    public bool IsWindowsTerminalInstalled => File.Exists(TerminalPath);

    /// <summary>
    /// Indicates if Powershell Core is installed
    /// </summary>
    public bool IsPsCoreInstalled => File.Exists(PowershellCorePath);

    /// <summary>
    /// Indicates if Visual Studio Code is installed
    /// </summary>
    public bool IsVSCodeInstalled => File.Exists(VsCodePath);
}
