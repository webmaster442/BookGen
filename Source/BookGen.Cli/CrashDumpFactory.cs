using System.Text;

namespace BookGen.Cli;

public static class CrashDumpFactory
{
    public static void TryCreateCrashDump(Exception ex)
    {
        var fileName = $"crash_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt";
        var dir = Path.Combine(AppContext.BaseDirectory, fileName);
        StringBuilder content = new();

        content
            .AppendLine($"Crash reason: {ex.Message}")
            .AppendLine("Stack trace:")
            .AppendLine(ex.StackTrace)
            .AppendLine("Environment:")
            .AppendLine($"Command line: {string.Join(' ', Environment.GetCommandLineArgs())}");

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
