using System.Text;

namespace BookGen.Cli.Dotenv;

public static class DotEnvParser
{
    public static DotEnvSettings Parse(TextReader reader, StringComparer comparer)
    {
        var result = new Dictionary<string, string>(comparer);
        string? line;

        while ((line = reader.ReadLine()) is not null)
        {
            int pos = SkipWhitespace(line, 0);

            // Empty line
            if (pos >= line.Length)
            {
                continue;
            }

            // Comment line
            if (IsCommentStart(line, pos))
            {
                continue;
            }

            // Key
            int keyStart = pos;
            if (!IsKeyStart(line[pos]))
            {
                throw EnvironmentException.Env003(line);
            }

            pos++;
            while (pos < line.Length && line[pos] != '=')
            {
                pos++;
            }

            string key = line[keyStart..pos];
            if (!IsValidKey(key))
            {
                if (key.Contains('\\'))
                    throw EnvironmentException.Env006(key);

                throw EnvironmentException.Env003(line);
            }

            // Equals with optional whitespace
            pos = SkipWhitespace(line, pos);
            if (pos >= line.Length || line[pos] != '=')
            {
                throw EnvironmentException.Env001(line);
            }

            pos++; // skip '='

            string value = ParseValue(reader, line, ref pos);

            if (result.ContainsKey(key))
            {
                throw EnvironmentException.Env002(key);
            }

            result[key] = value;
        }

        return new DotEnvSettings(result);
    }

    private static bool IsValidKey(string key)
    {
        foreach (char c in key)
        {
            if (!IsKeyChar(c))
            {
                return false;
            }
        }
        return true;
    }

    private static string ParseValue(TextReader reader, string line, ref int pos)
    {
        if (pos >= line.Length)
        {
            return string.Empty;
        }

        char c = line[pos];

        return c is '"' or '\''
            ? ParseQuotedValue(reader, line, ref pos, c)
            : ParseUnquotedOrContinuation(reader, line, ref pos);
    }

    private static string ParseQuotedValue(TextReader reader, string line, ref int pos, char quote)
    {
        var sb = new StringBuilder();
        pos++; // skip opening quote
        while (true)
        {
            while (pos < line.Length)
            {
                char c = line[pos];
                if (c == quote)
                {
                    pos++;
                    return sb.ToString();
                }
                sb.Append(c);
                pos++;
            }

            // Multi-line: append newline and read next line
            string? next = reader.ReadLine() ?? throw EnvironmentException.Env004();
            sb.Append('\n');
            line = next;
            pos = 0;
        }
    }

    private static string ParseUnquotedOrContinuation(TextReader reader, string line, ref int pos)
    {
        var sb = new StringBuilder();

        while (true)
        {
            int valueEnd = pos;
            int lastNonWs = pos - 1;
            bool sawComment = false;

            while (valueEnd < line.Length)
            {
                char c = line[valueEnd];
                if (c == '#')
                {
                    sawComment = true;
                    break;
                }
                if (c is not (' ' or '\t'))
                {
                    lastNonWs = valueEnd;
                }
                valueEnd++;
            }

            int end = lastNonWs + 1;

            // Continuation: value (trimmed) ends with backslash
            if (end > pos && line[end - 1] == '\\')
            {
                // The backslash must be the final character on the line:
                // no comment after it, no trailing whitespace after it.
                bool backslashIsLineEnd = end == line.Length;
                if (sawComment || !backslashIsLineEnd)
                {
                    throw EnvironmentException.Env005(line);
                }

                sb.Append(line, pos, end - 1 - pos);
                sb.Append('\n');

                string? next = reader.ReadLine();
                if (next is null)
                {
                    // Continuation at EOF is invalid.
                    throw EnvironmentException.Env005(line);
                }

                line = next;
                pos = SkipWhitespace(line, 0);
                continue;
            }

            sb.Append(line, pos, end - pos);
            pos = end;

            // Trailing whitespace on an unquoted value is not allowed.
            if (valueEnd > end)
            {
                throw EnvironmentException.Env001(line);
            }

            break;
        }

        return sb.ToString();
    }

    private static bool IsCommentStart(string line, int pos)
    {
        return line[pos] == '#'
            || line[pos] == ';'
            || (line[pos] == '/' && pos + 1 < line.Length && line[pos + 1] == '/');
    }

    private static int SkipWhitespace(string line, int pos)
    {
        while (pos < line.Length && line[pos] is ' ' or '\t')
        {
            pos++;
        }
        return pos;
    }

    private static bool IsKeyStart(char c)
        => char.IsAsciiLetter(c) || c == '_';

    private static bool IsKeyChar(char c)
        => char.IsAsciiLetterOrDigit(c) || c == '_';
}
