namespace Bookgen.Experiments.Expressions;

internal sealed class TokenCollection
{
    private readonly List<Token> _tokens;
    private int _index;

    public TokenCollection()
    {
        _tokens = new List<Token>();
        CurrentToken = new Token(string.Empty, TokenType.None);
        _index = -1;
    }

    public int Count => _tokens.Count;

    public void Add(Token token)
        => _tokens.Add(token);

    public Token CurrentToken
    {
        get; private set;
    }

    public bool Next()
    {
        ++_index;

        if (CurrentToken.Type == TokenType.EOF)
            throw new InvalidOperationException("Out of tokens");

        CurrentToken = _tokens[_index];

        return CurrentToken.Type != TokenType.EOF;
    }

    public void Eat(TokenType type)
    {
        if (CurrentToken.Type != type)
            throw new InvalidOperationException($"Expected a {type} token, got: {CurrentToken.Type}");

        Next();
    }
}
