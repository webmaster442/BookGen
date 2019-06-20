//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Terminal.Gui;

namespace BookGen.Gui
{
    internal class MainWindow : Window
    {
        private readonly TextView logView;
        private readonly Label workDir;


        public MainWindow() : base("BookGen Gui")
        {
            const int indent = 2;

            X = 0;
            Y = 1;
            Width = Dim.Fill();
            Height = Dim.Fill() - 1;

            logView = new TextView
            {
                Text = "",
                ReadOnly = true
            };

            var frame = new FrameView("Log:")
            {
                X = 0,
                Y = 4,
                Height = Dim.Fill(),
            };
            frame.Add(logView);

            workDir = new Label("")
            {
                X = indent * 2,
                Y = 2
            };

            View[] Views =
            {
                new Label($"Working directory:")
                {
                    X = indent,
                    Y = 1,
                },
                workDir,
                frame,
            };

            Add(Views);
        }

        public void UpdateLog(string content, int lines)
        {
            logView.Text = content;
            logView.ScrollTo(lines - 1);
        }

        public void UpdateWorkDir(string dir)
        {
            workDir.Text = dir;
        }
    }
}
