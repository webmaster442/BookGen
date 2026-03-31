using System.Diagnostics.CodeAnalysis;

namespace Bookgen.Lib.Markdown;

internal sealed class Search
{
    internal static readonly char[] Separators = ['\r', '\n', '.', '?', '!', '\t'];

    public static bool Contains(string document, string seachTerm, float similarity, [NotNullWhen(true)] out string? context)
    {
        var lines = document.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
        var searchWords = seachTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (searchWords.Length == 0)
        {
            context = null;
            return false;
        }

        foreach (var line in lines)
        {
            var lineWords = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (lineWords.Length < searchWords.Length)
                continue;

            for (var i = 0; i <= lineWords.Length - searchWords.Length; i++)
            {
                var allMatch = true;
                for (var j = 0; j < searchWords.Length; j++)
                {
                    int distance = LevenshteinDistance(lineWords[i + j], searchWords[j], ignoreCase: true);
                    int maxLength = Math.Max(lineWords[i + j].Length, searchWords[j].Length);
                    float wordSimilarity = maxLength == 0 ? 1f : (maxLength - distance) / (float)maxLength;

                    if (wordSimilarity < similarity)
                    {
                        allMatch = false;
                        break;
                    }
                }

                if (allMatch)
                {
                    context = line;
                    return true;
                }
            }
        }

        context = null;
        return false;
    }

    public static int LevenshteinDistance(ReadOnlySpan<char> source1, ReadOnlySpan<char> source2, bool ignoreCase = false)
    {
        var source1Length = source1.Length;
        var source2Length = source2.Length;

        if (source1Length == 0)
            return source2Length;

        // Single row instead of full matrix: O(min(n,m)) space
        const int stackAllocThreshold = 256;
        var bufferLength = source1Length + 1;

        Span<int> previousRow = bufferLength <= stackAllocThreshold
            ? stackalloc int[bufferLength]
            : new int[bufferLength];

        for (var i = 0; i <= source1Length; i++)
            previousRow[i] = i;

        for (var j = 1; j <= source2Length; j++)
        {
            var previousDiagonal = previousRow[0];
            previousRow[0] = j;
            var source2Char = source2[j - 1];

            for (var i = 1; i <= source1Length; i++)
            {
                var cost = ignoreCase
                    ? char.ToUpperInvariant(source1[i - 1]) == char.ToUpperInvariant(source2Char) ? 0 : 1
                    : source1[i - 1] == source2Char ? 0 : 1;
                var temp = previousRow[i];

                previousRow[i] = Math.Min(
                    Math.Min(temp + 1, previousRow[i - 1] + 1),
                    previousDiagonal + cost);

                previousDiagonal = temp;
            }
        }

        return previousRow[source1Length];
    }
}
