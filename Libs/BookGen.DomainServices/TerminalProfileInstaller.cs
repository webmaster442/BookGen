//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Terminal;
using System.Text.Json;

namespace BookGen.DomainServices;

public static class TerminalProfileInstaller
{
    private static WindowsTerminalProfile CreateProfile(string title)
    {
        return new WindowsTerminalProfile
        {
            StartingDirectory = "%userprofile%",
            Hidden = false,
            Icon = Path.Combine(AppContext.BaseDirectory, "bookgen-icon.png"),
            Name = title,
            TabTitle = title,
            CommandLine = GetCommandLine(),
        };
    }

    private static string GetCommandLine()
    {
        string shellScript = Path.Combine(AppContext.BaseDirectory, "BookGenShell.ps1");
        return $"powershell.exe -ExecutionPolicy Bypass -NoExit -File \"{shellScript}\"";
    }

    private static bool Write(TerminalFragment fragment, string fileName)
    {
        try
        {
            string? json = JsonSerializer.Serialize(fragment, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            if (string.IsNullOrEmpty(json))
                return false;

            string fragmentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows Terminal\Fragments\BookGen");
            if (!Directory.Exists(fragmentPath))
            {
                Directory.CreateDirectory(fragmentPath);
            }

            File.WriteAllText(Path.Combine(fragmentPath, fileName), json);
            return true;
        }
        catch (IOException)
        {
            return false;
        }
    }

    public static bool? TryInstall()
    {
        InstallStatus? installStatus = InstallDetector.GetInstallStatus();
        if (!installStatus.IsWindowsTerminalInstalled)
            return null;

        string title = "BookGen Shell";
        string fileName = "bookgen.json";
#if DEBUG
        title = "BookGen Shell (Dev version)";
        fileName = "bookgen.dev.json";
#endif
        var fragment = new TerminalFragment();
        fragment.Profiles.Add(CreateProfile(title));

        return Write(fragment, fileName);
    }
}
