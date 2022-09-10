using System;
using System.Windows.Media;

namespace BookGen.Launcher.Controls
{
    internal class ColorConverter : ConverterBase<string, SolidColorBrush>
    {
        protected override SolidColorBrush ConvertToTTo(string tFrom, object parameter)
        {
            int hue = CalculateHueHash(tFrom);
            return new SolidColorBrush(HSLtoRGB(hue, 0.5, 0.5));
        }

        private static byte Round(double input)
        {
            return (byte)Math.Round(input, 4);
        }

        private static Color HSLtoRGB(double Hue, double Luminance, double Saturation)
        {
            if (Saturation == 0)
            {
                // achromatic color (gray scale)
                return Color.FromRgb(
                    Round(Luminance * 255.0),
                    Round(Luminance * 255.0),
                    Round(Luminance * 255.0));
            }
            else
            {
                double q = Luminance < 0.5 ? Luminance * (1.0 + Saturation) : Luminance + Saturation - Luminance * Saturation;
                double p = 2.0 * Luminance - q;

                double Hk = Hue / 360.0;
                double[] T = new double[3];
                T[0] = Hk + 1.0 / 3.0;    // Tr
                T[1] = Hk;              // Tb
                T[2] = Hk - 1.0 / 3.0;    // Tg

                for (int i = 0; i < 3; i++)
                {
                    if (T[i] < 0) T[i] += 1.0;
                    if (T[i] > 1) T[i] -= 1.0;

                    if (T[i] * 6 < 1)
                    {
                        T[i] = p + (q - p) * 6.0 * T[i];
                    }
                    else if (T[i] * 2.0 < 1) //(1.0/6.0)<=T[i] && T[i]<0.5
                    {
                        T[i] = q;
                    }
                    else if (T[i] * 3.0 < 2) // 0.5<=T[i] && T[i]<(2.0/3.0)
                    {
                        T[i] = p + (q - p) * (2.0 / 3.0 - T[i]) * 6.0;
                    }
                    else T[i] = p;
                }

                return Color.FromRgb(
                    Round(T[0] * 255.0),
                    Round(T[1] * 255.0),
                    Round(T[2] * 255.0));
            }
        }

        private static int CalculateHueHash(string str)
        {
            ulong val = 14695981039346656037L;
            foreach (var chr in str)
            {
                val = val * 1099511628211 ^ chr;
            }
            return (int)(val % 360);
        }
    }
}
