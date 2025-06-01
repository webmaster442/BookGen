using Bookgen.Lib.Domain;
using Bookgen.Lib.Internals;
using Bookgen.Lib.Pipeline;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib;

public static class BookStatFactory
{
    public static async Task<BookStat> CreateBookStat(IBookEnvironment environment, ILogger logger)
    {
        BookStat stat = new();
        
        foreach (var chapter in environment.TableOfContents.Chapters)
        {
            stat.ChapterSizes[chapter.Title] = 0;
            foreach (var file in chapter.Files)
            {
                var sourceFile = await environment.Source.GetSourceFile(file, logger);
                long size = environment.Source.GetFileSize(file);
                stat.ChapterSizes[chapter.Title] += size;

                var (lineCount, wordCount, characterCount) = GetFileStats(sourceFile);
                stat.LineCount += lineCount;
                stat.WordCount += wordCount;
                stat.CharacterCount += characterCount;
            }
        }

        foreach (var file in environment.Source.GetFiles(environment.Source.Scope, "*.*", recursive: true))
        {
            var extension = Path.GetExtension(file);
            stat.FileCountsByExtension[extension] = !stat.FileCountsByExtension.TryGetValue(extension, out int count)
                ? 1
                : count + 1;

            stat.FileSizeByExtension[extension] = !stat.FileSizeByExtension.TryGetValue(extension, out long size) 
                ? 0 
                : size + environment.Source.GetFileSize(file);
        }

        return stat;
    }

    private static (long lineCount, long wordCount, long characterCount) GetFileStats(SourceFile sourceFile)
    {
        long lineCount = 1;
        long wordCount = 0;
        long characterCount = 0;

        using var reader = new StringReader(sourceFile.Content);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var lineStats = GetLineStats(line.Where(c => !IsMarkdownChar(c)));
            lineCount += (lineStats.length / 80) + (lineStats.length % 80) > 0 ? 1 : 0;
            wordCount += lineStats.words;
            characterCount += lineStats.length;
        }

        return (lineCount, wordCount, characterCount);
    }

    private static (int length, int words) GetLineStats(IEnumerable<char> chars)
    {
        int length = 0;
        int words = 0;
        bool inWord = false;
        foreach (var c in chars)
        {
            if (char.IsWhiteSpace(c) || c == '\n' || c == '\r')
            {
                if (inWord)
                {
                    words++;
                    inWord = false;
                }
            }
            else
            {
                inWord = true;
                length++;
            }
        }
        if (inWord) // If the last character was part of a word
        {
            words++;
        }
        return (length, words);
    }

    private static bool IsMarkdownChar(char c)
    {
        return c is '#' or '*' or '_' or '[' or ']' or
               '`' or '>' or '-' or '+' or '=' or '~' or '|';
    }
}