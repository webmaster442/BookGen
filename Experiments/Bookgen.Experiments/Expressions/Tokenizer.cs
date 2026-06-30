using System.Text;

namespace Bookgen.Experiments.Expressions;

internal ref struct Tokenizer
{
    private static bool IsAdditionalyAllowedInNumber(char c)
        => IsFloatCharacter(c) || c == '_';

    private static bool IsFloatCharacter(char c)
    {
        return c == '.'
            || c == '-'
            || c == '+'
            || c == 'E'
            || c == 'e';
    }

    private static Token HandleNumber(SharedStringBuilder sb, string input, int start, out int newIndex)
    {
        int index = start;
        bool containsAtLeastOneDigit = false;
        bool isInScientificMode = false;
        bool sciencemodePrefixed = false;
        bool dotFound = false;
        while (index < input.Length)
        {
            if (char.IsDigit(input[index]))
            {
                sb.Append(input[index]);
                ++index;
                containsAtLeastOneDigit = true;
            }
            else if (containsAtLeastOneDigit && IsAdditionalyAllowedInNumber(input[index]))
            {
                if (input[index] == '.' && !dotFound)
                {
                    dotFound = true;
                    sb.Append(input[index]);
                    ++index;
                }
                else if ((input[index] == '+' || input[index] == '-') && isInScientificMode)
                {
                    if (!sciencemodePrefixed)
                    {
                        sciencemodePrefixed = true;
                        sb.Append(input[index]);
                        ++index;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (input[index] == 'e' || input[index] == 'E' && !isInScientificMode)
                {
                    isInScientificMode = true;
                    sb.Append(input[index]);
                    ++index;
                }
                else if (input[index] == '_')
                {
                    sb.Append(input[index]);
                    ++index;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
        newIndex = index;
        string tokenValue = sb.ToString();

        if (tokenValue.Any(IsFloatCharacter))
            return new Token(tokenValue.Replace("_", ""), TokenType.Double);
        else
            return new Token(tokenValue.Replace("_", ""), TokenType.Integer);
    }


    private static Token HandleStringLiteral(SharedStringBuilder sb, string input, int start, char matcher, out int newIndex)
    {
        int index = start + 1;
        while (index < input.Length)
        {
            if (input[index] == matcher)
            {
                index++;
                break;
            }
            else
            {
                sb.Append(input[index]);
                index++;
            }
        }
        newIndex = index;
        return new Token(sb.ToString(), TokenType.String);
    }

    private static bool IsIdentifier(char c)
        => c == '_' || ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z');

    private static Token HandleIdentifier(SharedStringBuilder sb,
                                         string input,
                                         int start,
                                         Predicate<string> isFunctionCheck,
                                         out int newIndex)
    {
        int index = start;
        while (index < input.Length)
        {
            if (IsIdentifier(input[index]))
            {
                sb.Append(input[index]);
                ++index;
            }
            else
            {
                break;
            }
        }

        string identifier = sb.ToString();
        newIndex = index;
        switch (identifier)
        {
            case "true":
            case "false":
                {
                    return new Token(identifier, TokenType.Boolean);
                }
            default:
                {
                    bool isFunction = isFunctionCheck.Invoke(identifier);
                    return isFunction
                        ? new Token(identifier, TokenType.Function)
                        : new Token(identifier, TokenType.Variable);
                }
        }
    }

    public static TokenCollection Tokenize(string input, Predicate<string> isFunctionCheck)
    {
        SharedStringBuilder sb = new(512);
        TokenCollection tokens = new();
        int index = 0;
        int newIndex = 0;
        while (index < input.Length)
        {
            if (char.IsNumber(input[index]))
            {
                Token number = HandleNumber(sb, input, index, out newIndex);
                tokens.Add(number);
                index = newIndex;
            }
            else if (input[index] == '"')
            {
                Token stringLiteral = HandleStringLiteral(sb, input, index, '"', out newIndex);
                tokens.Add(stringLiteral);
                index = newIndex;
            }
            else if (input[index] == '\'')
            {
                Token stringLiteral = HandleStringLiteral(sb, input, index, '\'', out newIndex);
                tokens.Add(stringLiteral);
                index = newIndex;
            }
            else if (IsIdentifier(input[index]))
            {
                Token identifier = HandleIdentifier(sb, input, index, isFunctionCheck, out newIndex);
                tokens.Add(identifier);
                index = newIndex;
            }
            else
            {
                if (input[index] <= ' ')
                {
                    ++index;
                    continue;
                }

                char current = input[index];
                char next = index + 1 < input.Length ? input[index + 1] : '\0';
                Token @operator = HandleOperator(current, next, index, out newIndex);
                tokens.Add(@operator);
                index = newIndex;
            }
        }
        tokens.Add(new Token(string.Empty, TokenType.EOF));
        return tokens;

    }

    private static Token HandleOperator(char current, char next, int index, out int newIndex)
    {
        newIndex = index + 1;
        return current switch
        {
            '(' => new Token("(", TokenType.OpenParen),
            ')' => new Token(")", TokenType.CloseParen),
            ',' => new Token(",", TokenType.ArgumentDelimiter),
            _ => throw new InvalidOperationException($"Invalid operator `{current}` at position {index}"),
        };
    }
}
