//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace BookGen.Gui.Renderering
{
    public sealed class Color : IEquatable<Color>
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
                Color result = new Color(0, 0, 0)
                {
                    IsTransparent = true
                };
                return result;
            }
        }


        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public string GetForeground()
        {
            return $"38;2;{R};{G};{B}";
        }

        public string GetBackgound()
        {
            return $"48;2;{R};{G};{B}";
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Color);
        }

        public bool Equals(Color? other)
        {
            return 
                IsTransparent == other?.IsTransparent &&
                R == other?.R &&
                G == other?.G &&
                B == other?.B;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsTransparent, R, G, B);
        }

        public static bool operator ==(Color left, Color right)
        {
            return EqualityComparer<Color>.Default.Equals(left, right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !(left == right);
        }
    }
}
