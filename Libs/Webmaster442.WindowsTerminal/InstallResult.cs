namespace Webmaster442.WindowsTerminal;

public sealed record class InstallResult
{
    public string? TerminalPath { get; init; }
    public string? PowershellCorePath { get; init; }
    public string? VsCodePath { get; init; }

    public bool IsWindowsTerminalInstalled => File.Exists(TerminalPath);

    public bool IsPsCoreInstalled => File.Exists(PowershellCorePath);

    public bool IsVSCodeInstalled => File.Exists(VsCodePath);
}
