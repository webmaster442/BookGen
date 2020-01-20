//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain.VsTasks
{
#pragma warning disable IDE1006 // Naming Styles

    public enum Quoting
    {
        /// <summary>
        /// escaping instead of quoting for an argument with spaces.
        /// </summary>
        escape,
        /// <summary>
        /// Uses the shell's strong quoting mechanism, which suppresses all evaluations inside the string. Under PowerShell and for shells under Linux and macOS, single quotes are used ('). For cmd.exe, " is used.
        /// </summary>
        strong,
        /// <summary>
        /// Uses the shell's weak quoting mechanism, which still evaluates expression inside the string (for example, environment variables). Under PowerShell and for shells under Linux and macOS, double quotes are used ("). cmd.exe doesn't support weak quoting so VS Code uses " as well.
        /// </summary>
        weak
    }

    public class Arg
    {
        public string? value { get; set; }
        public Quoting quoting { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
