//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

namespace BookGen.Cli;

public sealed record class ProgramMetaData
{
    public required string AppName { get; init; }
    public required Version Version { get; init; }

    public static ProgramMetaData FromExecutingAssembly()
    {
        AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
        return new ProgramMetaData
        {
            AppName = assemblyName.Name ?? "",
            Version = assemblyName.Version ?? new Version(0, 0, 0),
        };
    }
}
