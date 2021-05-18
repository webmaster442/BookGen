using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace BookGen.Launch
{
    public class TileColorProvider : MarkupExtension
    {
        private static Color[] Colors = new Color[]
        {
            Color.FromRgb(164, 196, 0),
            Color.FromRgb(96, 169, 23),
            Color.FromRgb(0, 138, 0),
            Color.FromRgb(0, 171, 169),
            Color.FromRgb(27, 161, 226),
            Color.FromRgb(0, 80, 239),
            Color.FromRgb(106, 0, 255),
            Color.FromRgb(170, 0, 255),
            Color.FromRgb(244, 114, 208),
            Color.FromRgb(216, 0, 115),
            Color.FromRgb(162, 0, 37),
            Color.FromRgb(229, 20, 0),
            Color.FromRgb(250, 104, 0),
            Color.FromRgb(240, 163, 10),
            Color.FromRgb(227, 200, 0),
            Color.FromRgb(130, 90, 44),
            Color.FromRgb(109, 135, 100),
            Color.FromRgb(100, 118, 135),
            Color.FromRgb(118, 96, 138),
            Color.FromRgb(135, 121, 78),
        };

        private static int Index = 0;

        public SolidColorBrush Brush
        {
            get
            {
                var brush = new SolidColorBrush(Colors[Index]);
                ++Index;
                return brush;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
