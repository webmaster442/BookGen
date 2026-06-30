//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public sealed class SwitchAttribute : Attribute
{
    public string LongName { get; }
    public string ShortName { get; }
    public bool Required { get; }

    public SwitchAttribute(string shortName, string longName, bool required)
    {
        LongName = longName;
        ShortName = shortName;
        Required = required;
    }
}
