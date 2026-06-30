namespace Bookgen.Experiments.Expressions;

public enum TokenType
{
    None = 0,
    Integer = 1,
    Double = 2,
    Boolean = 4,
    String = 8,
    ArgumentDelimiter = 16,
    Variable = 32,
    Function = 64,
    OpenParen  = 128,
    CloseParen = 256,
    EOF = int.MaxValue,
}
