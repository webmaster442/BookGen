//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Renderering;
using System;

namespace BookGen.Gui.Elements
{
    internal class Button: ConsoleUiElement
    {
        public Action Action { get; set; }
        public string Content { get; set; }
        public ConsoleKey ActivatorKey { get; set; }

        public override void Render(Renderer target)
        {
            string activatorkey = ActivatorKey.ToString().PadRight(8);

            target.Text("   {0}", Color.Green, Background, TextFormat.BoldBright, activatorkey);
            target.Text(" : {0}", Foreground, Background, TextFormat.Default, Content);
            target.NewLine();
        }
    }
}
