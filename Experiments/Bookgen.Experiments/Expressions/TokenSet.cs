namespace Bookgen.Experiments.Expressions;

internal readonly struct TokenSet
{
    private readonly uint _tokens;

    public TokenSet(TokenType token)
        => _tokens = (uint)token;

    public TokenSet(TokenSet set)
        => _tokens = set._tokens;

    private TokenSet(uint tokens)
        => _tokens = tokens;

    public TokenSet(params TokenType[] tokens)
    {
        foreach (var token in tokens)
        {
            _tokens |= (uint)token;
        }
    }

    public static TokenSet operator +(TokenSet t1, TokenSet t2)
        => new TokenSet(t1._tokens | t2._tokens);

    public static TokenSet operator +(TokenSet t1, TokenType t2)
        => new TokenSet(t1._tokens | (uint)t2);

    public bool Contains(TokenType type)
        => (_tokens & (uint)type) != 0;

    public override string ToString()
    {
        var tokens = Enum.GetValues<TokenType>();
        var containedTokens = tokens.Where(Contains).Select(t => t.ToString());
        return string.Join(", ", containedTokens);
    }
}
