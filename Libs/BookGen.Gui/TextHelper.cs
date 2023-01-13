//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui;

internal static class TextHelper
{
    public static IReadOnlyList<string> GetLines(string text, int lineMaxWidth)
    {
        List<string> buffer = new List<string>(8);
        using (var reader = new StringReader(text))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length < lineMaxWidth)
                {
                    buffer.Add(line);
                }
                else
                {
                    var newLines = line
                        .Chunk(lineMaxWidth)
                        .Select(chrArray => new string(chrArray))
                        .ToArray();

                    buffer.AddRange(newLines);
                }
            }
        }
        return buffer;
    }
}
