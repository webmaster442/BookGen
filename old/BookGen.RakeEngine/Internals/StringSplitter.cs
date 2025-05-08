//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.RakeEngine.Internals;

internal ref struct StringSplitter
{
    private readonly ReadOnlySpan<char> _text;
    private readonly char _seperator;
    private int _position;

    public StringSplitter(ReadOnlySpan<char> text, char seperator)
    {
        _text = text;
        _seperator = seperator;
        _position = 0;
    }

    public bool TryGetNext(out ReadOnlySpan<char> result)
    {
        if (_position >= _text.Length)
        {
            result = default;
            return false;
        }

        int start = _position;
        int commaIndex = _text[_position..].IndexOf(_seperator);

        if (commaIndex > -1)
        {
            _position += commaIndex + 1;
            result = _text.Slice(start, commaIndex);
        }
        else
        {
            _position = _text.Length;
            result = _text[start..];
        }

        return true;

    }
}
