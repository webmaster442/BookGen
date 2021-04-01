//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Utilities
{
    internal static class StringExtensions
    {
        public static int GetWordCount(this string input)
        {
            int count = 0;
            int index = 0;

            while (index < input.Length && char.IsWhiteSpace(input[index]))
            {
                ++index;
            }

            while (index < input.Length)
            {
                // check if current char is part of a word
                while (index < input.Length && !char.IsWhiteSpace(input[index]))
                {
                    ++index;
                }
                ++count;
                // skip whitespace until next word
                while (index < input.Length && char.IsWhiteSpace(input[index]))
                {
                    ++index;
                }
            }

            return count;
        }

    }
}
