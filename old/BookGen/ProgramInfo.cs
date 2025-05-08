//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

using BookGen.CommandArguments;

namespace BookGen;

internal sealed class ProgramInfo
{
    public bool Gui { get; set; }
    public bool NoWaitForExit { get; set; }
    public Version ProgramVersion { get; }
    public DateTime BuildDateUtc { get; }
    public string ProgramDirectory { get; }
    public int ConfigVersion { get; }

    public LogLevel LogLevel { get; private set; }
    public bool LogToFile { get; set; }
    public bool JsonLogging { get; set; }

    private static DateTime GetProgramDate()
    {
        var current = Assembly.GetAssembly(typeof(ProgramInfo));
        if (current != null)
        {
            AssemblyBuildDateAttribute? attribute = current.GetCustomAttribute<AssemblyBuildDateAttribute>();
            if (attribute != null)
            {
                return attribute.BuildDateUtc;
            }
        }
        return new DateTime();
    }

    internal void EableVerboseLogging(bool verbose)
        => LogLevel = verbose ? LogLevel.Debug : LogLevel.Information;

    internal void EnableVerboseLogingIfRequested(BookGenArgumentBase argumentBase) 
        => EableVerboseLogging(argumentBase.Verbose);

    public ProgramInfo()
    {
        var asm = Assembly.GetAssembly(typeof(ProgramInfo));
        ProgramVersion = asm?.GetName()?.Version ?? new Version(1, 0);
        ConfigVersion = (ProgramVersion.Major * 1000) + (ProgramVersion.Minor * 100) + ProgramVersion.Build;
        BuildDateUtc = GetProgramDate();
        ProgramDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
    }
}
