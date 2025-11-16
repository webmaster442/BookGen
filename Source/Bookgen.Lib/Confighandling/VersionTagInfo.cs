//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.Confighandling;

internal readonly record struct VersionTagInfo : IComparable<VersionTagInfo>
{
    public VersionTagInfo(int from, int to)
    {
        From = from;
        To = to;
    }

    public int From { get; }
    public int To { get; }

    public int CompareTo(VersionTagInfo other)
    {
        int result = From.CompareTo(other.From);
        if (result == 0)
        {
            result = To.CompareTo(other.To);
        }
        return result;
    }
}
