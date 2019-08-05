//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Elements;
using BookGen.Gui.Renderering;
using System.Collections.Generic;

namespace BookGen.Gui
{
    internal abstract class ConsoleMenuBase
    {
        protected ITerminalRenderer Renderer { get; }
        protected GeneratorRunner Runner { get; }

        public static bool ShouldRun { get; set; }

        protected List<ConsoleUiElement> Elements { get; private set; }

        public abstract ConsoleUiElement[] CreateElements();

        protected ConsoleMenuBase(GeneratorRunner runner)
        {
            ShouldRun = true;
            Renderer = NativeWrapper.GetRenderer();
            Runner = runner;
        }

        protected void DoRender()
        {
            if (Elements == null)
            {
                Elements = new List<ConsoleUiElement>(CreateElements());
                ReindexButtonsInElements();
            }
            Renderer.Clear();
            foreach (var uiElement in Elements)
            {
                uiElement.Render(Renderer);
            }
        }

        private void ReindexButtonsInElements()
        {
            int btnEntry = 1;
            foreach (var element in Elements)
            {
                if (element is Button button)
                {
                    button.Entry = btnEntry;
                    ++btnEntry;
                }
            }
        }

        public virtual void Run()
        {
            //Base method
        }
    }
}
