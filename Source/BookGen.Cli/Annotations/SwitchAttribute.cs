//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public sealed class SwitchAttribute : Attribute
{
    public string LongName { get; }
    public string ShortName { get; }
    public required bool Required { get; set; }

    public SwitchAttribute(string shortName, string longName)
    {
        LongName = longName;
        ShortName = shortName;
    }
}
