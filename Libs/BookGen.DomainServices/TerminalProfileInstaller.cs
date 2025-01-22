//-----------------------------------------------------------------------------
// (c) 2022-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Terminal;
using System.Text.Json;
using System.Text.Json.Serialization;

using Webmaster442.WindowsTerminal;

namespace BookGen.DomainServices;

public static class TerminalProfileInstaller
{
    public static readonly string TerminalFragmentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                                                      "Microsoft",
                                                                      "Windows Terminal",
                                                                      "Fragments",
                                                                      "BookGen");

    private static TerminalProfile CreateProfile(string title, bool isPsCoreAvailable)
    {
        static string GetCommandLine(bool isPsCoreAvailable)
        {
            string shellScript = Path.Combine(AppContext.BaseDirectory, "BookGenShell.ps1");
            if (isPsCoreAvailable)
            {
                return $"{InstallDetector.PowershellCoreExe} -ExecutionPolicy Bypass -NoExit -File \"{shellScript}\"";
            }
            else
            {
                return $"powershell.exe -ExecutionPolicy Bypass -NoExit -File \"{shellScript}\"";
            }
        }

        return new TerminalProfile
        {
            StartingDirectory = "%userprofile%",
            Hidden = false,
            Icon = Path.Combine(AppContext.BaseDirectory, "icon-bookgen.png"),
            Name = title,
            TabTitle = title,
            CommandLine = GetCommandLine(isPsCoreAvailable),
            ColorScheme = TerminalSchemes.PurplepeterShecme.Name,
            BackgroundImage = Path.Combine(AppContext.BaseDirectory, "background.png"),
            BackgroundImageStretchMode = TerminalBackgroundImageStretchMode.None,
            BackgroundImageAlignment = TerminalBackgroundImageAlignment.BottomRight,
            UseAcrylic = true,
            Opacity = 80,
        };
    }

    public static async Task<bool?> TryInstallAsync()
    {
        InstallResult installStatus = InstallDetector.GetInstallResult();
        if (!installStatus.IsWindowsTerminalInstalled)
            return null;

        string title = "BookGen Shell";
        string fileName = "bookgen.json";
#if DEBUG
        title = "BookGen Shell (Dev version)";
        fileName = "bookgen.dev.json";
#endif
        var fragment = new TerminalFragment();
        fragment.Profiles.Add(CreateProfile(title, installStatus.IsPsCoreInstalled));
        fragment.Schemes.Add(TerminalSchemes.PurplepeterShecme);

        return await WindowsTerminal.TryInstallFragmentAsync("BookGen", fileName, fragment);
    }
}
