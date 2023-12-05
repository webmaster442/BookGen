//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using TextResources = BookGen.Shell.Properties.Resources;

namespace BookGen.Shell.Cdg;

internal sealed class SelectorNameConverter
{
    private readonly string[] _drives;

    public SelectorNameConverter()
    {
        _drives = Environment.GetLogicalDrives();
    }

    public string Convert(string arg)
    {
        if (_drives.Contains(arg))
        {
            DriveInfo di = new(arg);
            return $"{di.Name} - {di.VolumeLabel}";
        }

        return arg switch
        {
            nameof(TextResources._MenuSelectorCurrentDir_10) => TextResources._MenuSelectorCurrentDir_10,
            nameof(TextResources._MenuSelectorUpOneDir_20) => TextResources._MenuSelectorUpOneDir_20,
            nameof(TextResources._MenuSelectorRootDir_30) => TextResources._MenuSelectorRootDir_30,
            nameof(TextResources._MenuSelectorHomeDir_35) => TextResources._MenuSelectorHomeDir_35,
            nameof(TextResources._MenuSelectorKnownDirs_40) => TextResources._MenuSelectorKnownDirs_40,
            _ => Path.GetFileName(arg) == string.Empty ? arg : Path.GetFileName(arg),
        };
    }
}
