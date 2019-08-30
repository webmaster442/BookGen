//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace BookGen.Editor.EditorControl
{
    internal class SpellingColorizer : IBackgroundRenderer
    {
        public SpellingColorizer()
        {
            Errors = new TextSegmentCollection<TextSegment>();
        }

        public KnownLayer Layer => KnownLayer.Selection;

        public TextSegmentCollection<TextSegment> Errors
        {
            get;
        }

        private IEnumerable<Point> CreatePoints(Point start, double offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Point(start.X + (i * offset), start.Y - (((i + 1) % 2 == 0) ? offset : 0.0));
            }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            foreach (TextSegment current in Errors)
            {
                foreach (Rect current2 in BackgroundGeometryBuilder.GetRectsForSegment(textView, current, false))
                {
                    Point bottomLeft = current2.BottomLeft;
                    Point bottomRight = current2.BottomRight;
                    Pen pen = new Pen(new SolidColorBrush(Colors.Red), 0.5);
                    pen.Freeze();
                    const double num = 1.0;
                    int count = Math.Max((int)((bottomRight.X - bottomLeft.X) / num) + 1, 4);
                    StreamGeometry streamGeometry = new StreamGeometry();
                    using (StreamGeometryContext context = streamGeometry.Open())
                    {
                        context.BeginFigure(bottomLeft, false, false);
                        context.PolyLineTo(CreatePoints(bottomLeft, num, count).ToArray(), true, false);
                    }
                    streamGeometry.Freeze();
                    drawingContext.DrawGeometry(Brushes.Transparent, pen, streamGeometry);
                }
            }
        }
    }
}
