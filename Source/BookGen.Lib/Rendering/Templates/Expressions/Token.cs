namespace Bookgen.Lib.Rendering.Templates.Expressions;

internal readonly struct Token(string value, TokenType type)
{
    public string Value { get; } = value;
    public TokenType Type { get; } = type;

    public override string ToString()
        => $"{Value} | {Type}";
}
