//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Spectre.Console;

namespace BookGen.Gui;

internal class Palette
{
    private readonly Color[] _colors;
    private int _index;

    public Palette()
    {
        Random rnd = new(2);
        _colors = Generate(rnd, 32);
        _index = 0;
    }

    private static Color[] Generate(Random rnd, int count)
    {
        static Color Generate(Random rnd)
        {
            byte r = (byte)(rnd.Next(128) + 127);
            byte g = (byte)(rnd.Next(128) + 127);
            byte b = (byte)(rnd.Next(128) + 127);
            return new Color(r, g, b);
        }

        Color[] result = new Color[count];
        for(int i=0; i<count; i++)
        {
            result[i] = Generate(rnd);
        }
        return result;
    }

    public void Reset()
        => _index = 0;

    public Color GetNextColor()
    {
        Color ret = _colors[_index];
        _index = (_index + 1) % _colors.Length;
        return ret;
    }
}
