//-----------------------------------------------------------------------------
// (c) 2022-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Webmaster442.WindowsTerminal;

namespace BookGen.Shell.Shared;

public static class TerminalProfileInstaller
{
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

#if DEBUG
    private const string fileName = "bookgen.dev.json";
#else
    private const string fileName = "bookgen.json";
#endif


    public static async Task<bool?> TryInstallAsync()
    {
        InstallResult installStatus = InstallDetector.GetInstallResult();
        if (!installStatus.IsWindowsTerminalInstalled)
            return null;

        string title = "BookGen Shell";
#if DEBUG
        title = "BookGen Shell (Dev version)";
#endif
        var fragment = new TerminalFragment();
        fragment.Profiles.Add(CreateProfile(title, installStatus.IsPsCoreInstalled));
        fragment.Schemes.Add(TerminalSchemes.PurplepeterShecme);
        fragment.Schemes.Add(TerminalSchemes.Dracula);
        fragment.Actions.Add(new TerminalAction
        {
            Command = TerminalCommand.SetColorScheme(TerminalSchemes.PurplepeterShecme.Name),
            Id = "User.SetSchemeToPurplepeter",
        });
        fragment.Actions.Add(new TerminalAction
        {
            Command = TerminalCommand.SetColorScheme(TerminalSchemes.Dracula.Name),
            Id = "User.SetSchemeToDracula",
        });

        return await WindowsTerminal.FragmentExtensions.TryInstallFragmentAsync("BookGen", fileName, fragment);
    }

    public static bool IsInstalled()
        => WindowsTerminal.FragmentExtensions.IsFragmentInstalled("BookGen", fileName);
}
