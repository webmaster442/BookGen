//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using BookGen.Shellprog.CommandCode.Cdg;

namespace BookGen.Shell.Cdg;

internal static partial class SelectionItemFactory
{
    public static IEnumerable<SelectionItemDirectory> GetDrives()
    {
        static string GetVolumeLabel(DriveInfo drive)
        {
            if (drive.IsReady)
            {
                return drive.VolumeLabel;
            }
            return string.Empty;
        }

        DriveInfo[] drives = DriveInfo.GetDrives();
        foreach (DriveInfo drive in drives)
        {
            yield return new SelectionItemDirectory
            {
                DisplayString = $"{drive.Name} - {GetVolumeLabel(drive)}",
                Icon = GetIcon(drive.DriveType),
                Color = Spectre.Console.Color.Blue,
                Path = drive.Name
            };
        }
    }

    public static IEnumerable<SelectionItemDirectory> CreateFromDirectories(string[] directories)
    {
        foreach (var directory in directories)
        {
            yield return new SelectionItemDirectory
            {
                DisplayString = Path.GetFileName(directory),
                Icon = ":open_file_folder:",
                Color = Spectre.Console.Color.Yellow,
                Path = directory
            };
        }
    }

    public static IEnumerable<SelectionItemDirectory> GetSpecialFolders()
    {
        foreach (Environment.SpecialFolder specialFolder in Enum.GetValues<Environment.SpecialFolder>())
        {
            var path = Environment.GetFolderPath(specialFolder);
            if (!string.IsNullOrEmpty(path))
            {
                yield return new SelectionItemDirectory
                {
                    DisplayString = $"{Humanize(specialFolder)}",
                    Icon = ":up_right_arrow:",
                    Color = Spectre.Console.Color.Green,
                    Path = path
                };
            }
        }
    }

    private static string Humanize(Environment.SpecialFolder specialFolder)
    {
        var original = specialFolder.ToString();
        return EnumNameHumanizer().Replace(original, " $1").TrimStart();
    }

    public static IEnumerable<SelectionItemDirectory> GetPathDirectories()
    {
        char seperator = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ';' : ':';
        var directories = Environment.GetEnvironmentVariable("PATH")
            ?.Split(seperator, StringSplitOptions.RemoveEmptyEntries)
            ?? Array.Empty<string>();

        foreach (var directory in directories)
        {
            if (Directory.Exists(directory))
            {
                yield return new SelectionItemDirectory
                {
                    DisplayString = directory,
                    Icon = ":open_file_folder:",
                    Color = Spectre.Console.Color.Yellow,
                    Path = directory
                };
            }
        }
    }

    private static string GetIcon(DriveType driveType)
    {
        return driveType switch
        {
            DriveType.Fixed => ":computer_disk:",
            DriveType.Unknown => ":white_question_mark:",
            DriveType.NoRootDirectory => ":open_file_folder:",
            DriveType.Removable => ":floppy_disk:",
            DriveType.Network => ":link:",
            DriveType.CDRom => ":optical_disk:",
            DriveType.Ram => ":memo:",
            _ => throw new UnreachableException(),
        };
    }

    [GeneratedRegex("([A-Z0-9]{1,3}|[0-9]+)")]
    private static partial Regex EnumNameHumanizer();
}
