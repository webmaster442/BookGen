//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Renderering;
using System;

namespace BookGen.Gui.Elements
{
    internal class Button: ConsoleUiElement, IHaveContent, IHaveEntry
    {
        public Action? Action { get; set; }
        public string Content { get; set; }
        public int Entry { get; set; }

        public Button()
        {
            Content = string.Empty;
        }

        public override void Render(ITerminalRenderer target)
        {
            string activatorkey = $"{Entry}.".PadRight(4);

            target.Text("   {0}", Color.Green, Background, TextFormat.BoldBright, activatorkey);
            target.Text(" : {0}", Foreground, Background, TextFormat.Default, Content);
            target.NewLine();
        }
    }
}
