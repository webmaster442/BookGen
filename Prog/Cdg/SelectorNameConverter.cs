//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Cdg.Properties;

namespace Cdg;

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
            nameof(Resources._MenuSelectorCurrentDir_10) => Resources._MenuSelectorCurrentDir_10,
            nameof(Resources._MenuSelectorUpOneDir_20) => Resources._MenuSelectorUpOneDir_20,
            nameof(Resources._MenuSelectorRootDir_30) => Resources._MenuSelectorRootDir_30,
            nameof(Resources._MenuSelectorHomeDir_35) => Resources._MenuSelectorHomeDir_35,
            nameof(Resources._MenuSelectorKnownDirs_40) => Resources._MenuSelectorKnownDirs_40,
            _ => Path.GetFileName(arg) == string.Empty ? arg : Path.GetFileName(arg),
        };
    }
}
