//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli.Annotations;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class CommandNameAttribute : Attribute
{
    public string Name { get; }

    public CommandNameAttribute(string name)
    {
        Name = name;
    }
}
