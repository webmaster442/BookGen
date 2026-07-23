//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Lib.Rendering.Templates.Expressions;

internal readonly struct Token(string value, TokenType type)
{
    public string Value { get; } = value;
    public TokenType Type { get; } = type;

    public override string ToString()
        => $"{Value} | {Type}";
}
