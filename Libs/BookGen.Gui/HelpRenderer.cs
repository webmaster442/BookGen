﻿using Spectre.Console;

namespace BookGen.Gui;

public static class HelpRenderer
{
    public static void RenderHelp(IEnumerable<string> article)
    {
        int pageSize = Console.WindowHeight - 3;
        IReadOnlyList<string> reWraped = DoReWrap(article, pageSize, Console.WindowWidth);
        var pages = reWraped.Chunk(pageSize).ToArray();

        Console.Clear();

        int currentPage = 0;
        bool run = pages.Length > 1;
        do
        {
            Render(pages[currentPage]);
            RenderUsage(currentPage, pages.Length);

            if (!run) continue;

            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                case ConsoleKey.UpArrow:
                    currentPage = CalculatePage(currentPage, pages.Length, +1);
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.RightArrow:
                    currentPage = CalculatePage(currentPage, pages.Length, -1);
                    break;
                case ConsoleKey.Escape:
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
        AnsiConsole.MarkupInterpolated($"[teal]{currentPage+1} of {pages}[/]");
        AnsiConsole.Markup(" [silver]ESC: Exit, <- Prev, Next ->[/]\r\n");
    }

    private static void Render(string[] pageContent)
    {
        foreach (var line in pageContent)
        {
            if (line.StartsWith("# "))
                AnsiConsole.MarkupInterpolated($"[green bold]{line}[/]\r\n");
            else if (line.StartsWith("`") || line.EndsWith("`"))
                AnsiConsole.MarkupInterpolated($"[aqua]{line}[/]\r\n");
            else
                AnsiConsole.MarkupInterpolated($"[italic]{line}[/]\r\n");
        }
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

    private static IReadOnlyList<string> DoReWrap(IEnumerable<string> article, int pageSize, int windowWidth)
    {
        List<string> result = new List<string>(pageSize);
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
