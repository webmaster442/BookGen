//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Gui.Renderering
{
    public class Color
    {
        public bool IsTransparent { get; private set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public static Color Black { get => new Color(0, 0, 0); }
        public static Color White { get => new Color(0xFF, 0xFF, 0xFF); }
        public static Color Green { get => new Color(0, 0xFF, 0); }
        public static Color Red { get => new Color(0xFF, 0, 0); }
        public static Color Blue { get => new Color(0, 0, 0xFF); }
        public static Color Transparent
        {
            get
            {
                Color result = new Color(0, 0, 0);
                result.IsTransparent = true;
                return result;
            }
        }


        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        private static int ParseHexChar(char c)
        {
            int intChar = (int)c;

            if ((intChar >= '0') && (intChar <= ('0' + 9)))
            {
                return (intChar - '0');
            }

            if ((intChar >= 'a') && (intChar <= ('a' + 5)))
            {
                return (intChar - 'a' + 10);
            }

            if ((intChar >= 'A') && (intChar <= ('A' + 5)))
            {
                return (intChar - 'A' + 10);
            }
            throw new ArgumentException($"Invalid char: {c}");
        }

        public static Color Parse(string s)
        {
            int r, g, b;

            if (!s.StartsWith('#') || s.Length > 7)
                throw new ArgumentException("String is not a valid color");

            var trimmedColor = s.Substring(1);

            if(trimmedColor.Length > 7)
            {
                r = (ParseHexChar(trimmedColor[1]) * 16) + ParseHexChar(trimmedColor[2]);
                g = (ParseHexChar(trimmedColor[3]) * 16) + ParseHexChar(trimmedColor[4]);
                b = (ParseHexChar(trimmedColor[5]) * 16) + ParseHexChar(trimmedColor[6]);
            }
            else if (trimmedColor.Length > 5)
            {
                r = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
                g = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
                b = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
            }
            else if (trimmedColor.Length > 4)
            {
                r = ParseHexChar(trimmedColor[1]);
                r = r + r * 16;
                g = ParseHexChar(trimmedColor[2]);
                g = g + g * 16;
                b = ParseHexChar(trimmedColor[3]);
                b += b * 16;
            }

            r = ParseHexChar(trimmedColor[1]);
            r = r + r * 16;
            g = ParseHexChar(trimmedColor[2]);
            g = g + g * 16;
            b = ParseHexChar(trimmedColor[3]);
            b = b + b * 16;

            return new Color((byte)r, (byte)g, (byte)b);
        }

        public string GetForeground()
        {
            return $"38;2;{R};{G};{B}";
        }

        public string GetBackgound()
        {
            return $"48;2;{R};{G};{B}";
        }
    }
}
