//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli.Annotations;


[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ExitCodeAttribute : Attribute
{
    public int ExitCode { get; }
    public string Description { get; }

    public ExitCodeAttribute(int exitCode, string description)
    {
        ExitCode = exitCode;
        Description = description;
    }

}
