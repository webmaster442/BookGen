//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Commands;

using Microsoft.Extensions.Logging;

namespace BookGen;

internal sealed class ProgramInfo
{
    public bool Gui { get; set; }
    public bool NoWaitForExit { get; set; }

    public Version ProgramVersion { get; }

    public string ProgramDirectory { get; }
    public int ConfigVersion { get; }

    public LogLevel LogLevel { get; private set; }
    public bool LogToFile { get; set; }
    public bool JsonLogging { get; set; }

    internal void EableVerboseLogging(bool verbose)
        => LogLevel = verbose ? LogLevel.Debug : LogLevel.Information;

    internal void EnableVerboseLogingIfRequested(BookGenArgumentBase argumentBase) 
        => EableVerboseLogging(argumentBase.Verbose);

    public ProgramInfo()
    {
        var asm = Assembly.GetAssembly(typeof(ProgramInfo));
        ProgramVersion = asm?.GetName()?.Version ?? new Version(1, 0);
        ConfigVersion = Config.CurrentVersionTag;
        ProgramDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
    }
}
