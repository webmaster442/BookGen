//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace BookGen.Cli;

public static class CrashDumpFactory
{
    public static void TryCreateCrashDump(Exception ex)
    {
        var fileName = $"bookgen_crash_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt";
        string dir = Path.Combine(Environment.CurrentDirectory, fileName);
        StringBuilder content = new();

        content
            .AppendLine("BookGen Crash Dump")
            .Append('-', 80).AppendLine()
            .AppendLine("Environment:")
            .AppendLine($"Command line: {string.Join(' ', Environment.GetCommandLineArgs())}")
            .AppendLine($"Crash reason: {ex.Message}")
            .Append('-', 80).AppendLine()
            .AppendLine("Source:")
            .AppendLine(ex.Source)
            .Append('-', 80).AppendLine()
            .AppendLine("Stack trace:")
            .AppendLine(ex.StackTrace);


        if (ex.InnerException != null)
        {
            content
                .Append('-', 80).AppendLine()
                .AppendLine($"Inner exception: {ex.InnerException.Message}")
                .Append('-', 80).AppendLine()
                .AppendLine(ex.InnerException.StackTrace);
        }


#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception
        try
        {
            File.WriteAllText(fileName, content.ToString());
        }
        catch(Exception)
        {
            //At this point we don't care
        }
#pragma warning restore RCS1075
    }
}
