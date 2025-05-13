//-----------------------------------------------------------------------------
// (c) 2024-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Spectre.Console;

namespace BookGen.Infrastructure;

public static class HelpRenderer
{
    public static string[][] GetPages(IEnumerable<string> article)
    {
        int pageSize = Console.WindowHeight - 3;
        IReadOnlyList<string> reWraped = DoReWrap(article, pageSize, Console.WindowWidth);
        return reWraped.Chunk(pageSize).ToArray();
    }

    public static void RenderPage(string[] pageContent)
    {
        foreach (var line in pageContent)
        {
            if (line.StartsWith("# "))
                AnsiConsole.MarkupInterpolated($"[green bold]{line}[/]{Environment.NewLine}");
            else if (line.StartsWith('`') || line.EndsWith('`'))
                AnsiConsole.MarkupInterpolated($"[aqua]{line}[/]{Environment.NewLine}");
            else
                AnsiConsole.MarkupInterpolated($"[italic]{line}[/]{Environment.NewLine}");
        }
    }

    public static void RenderHelp(IEnumerable<string> article)
    {
        var pages = GetPages(article);
        Console.Clear();

        int currentPage = -1;
        int nextPage = 0;
        bool run = pages.Length > 1;
        do
        {
            if (currentPage != nextPage)
            {
                currentPage = nextPage;
                Console.Clear();
                RenderPage(pages[currentPage]);
                RenderUsage(currentPage, pages.Length);
            }

            if (!run) continue;

            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                case ConsoleKey.UpArrow:
                    nextPage = CalculatePage(currentPage, pages.Length, -1);
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.RightArrow:
                    nextPage = CalculatePage(currentPage, pages.Length, +1);
                    break;
                case ConsoleKey.Escape:
                case ConsoleKey.Q:
                    run = false;
                    Console.Clear();
                    break;
            }
        }
        while (run);
    }

    private static void RenderUsage(int currentPage, int pages)
    {
        if (pages < 2)
            return;

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupInterpolated($"[teal]{currentPage + 1} of {pages}[/]");
        AnsiConsole.MarkupInterpolated($" [silver]ESC or Q: Exit, <- Prev, Next ->[/]{Environment.NewLine}");
    }

    private static int CalculatePage(int currentPage, int pages, int offset)
    {
        int newIndex = currentPage + offset;

        if (newIndex < 0)
            newIndex = 0;

        if (newIndex > pages - 1)
            newIndex = pages - 1;

        return newIndex;
    }

    private static List<string> DoReWrap(IEnumerable<string> article, int pageSize, int windowWidth)
    {
        List<string> result = new(pageSize);
        foreach (string line in article)
        {
            if (line.Length > windowWidth)
            {
                var newLines = line
                    .Chunk(windowWidth)
                    .Select(chrs => new string(chrs));
                result.AddRange(newLines);
            }
            else
            {
                result.Add(line);
            }
        }
        return result;
    }
}
