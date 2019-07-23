//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Gui
{
    internal class Button
    {
        public Action Action { get; set; }
        public string Content { get; set; }
        public ConsoleKey ActivatorKey { get; set; }
    }
}
