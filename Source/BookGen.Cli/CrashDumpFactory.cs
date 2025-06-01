using System.Text;

namespace BookGen.Cli;

public static class CrashDumpFactory
{
    public static void TryCreateCrashDump(Exception ex)
    {
        var fileName = $"bookgen_crash_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt";
        string dir = Path.Combine(AppContext.BaseDirectory, fileName);
        StringBuilder content = new();

        content
            .AppendLine(new string('-', 80))
            .AppendLine("Environment:")
            .AppendLine($"Command line: {string.Join(' ', Environment.GetCommandLineArgs())}")
            .AppendLine($"Crash reason: {ex.Message}")
            .AppendLine(new string('-', 80))
            .AppendLine("Stack trace:")
            .AppendLine(ex.StackTrace);

        if (ex.InnerException != null)
        {
            content
                .AppendLine(new string('-', 80))
                .AppendLine($"Inner exception: {ex.InnerException.Message}")
                .AppendLine(new string('-', 80))
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
