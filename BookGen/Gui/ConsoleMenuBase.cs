using BookGen.Gui.Renderering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BookGen.Gui.Elements;

namespace BookGen.Gui
{
    internal abstract class ConsoleMenuBase
    {
        private bool DoRenderWarCalled = false;

        protected ITerminalRenderer Renderer { get; }
        protected GeneratorRunner Runner { get; }

        public static bool ShouldRun { get; set; }

        private readonly List<ConsoleUiElement> Elements;

        public abstract ConsoleUiElement[] CreateElements();

        protected ConsoleMenuBase(GeneratorRunner runner)
        {
            ShouldRun = true;
            Renderer = NativeWrapper.GetRenderer();
            Runner = runner;
            Elements = new List<ConsoleUiElement>(CreateElements());
            Elements.Add(new TextBlock
            {
                Text = "\r\nExit program\r\n"
            });
            Elements.Add(new Button
            {
                Action = () => Environment.Exit(0),
                Content = "Exit program"
            });
        }

        private void DoRender()
        {
            if (!DoRenderWarCalled)
            {
                ReindexButtonsInElements();
                DoRenderWarCalled = true;
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
                var button = element as Button;
                if (button != null)
                {
                    button.Entry = btnEntry;
                    ++btnEntry;
                }
            }
        }

        public void Run()
        {
            DoRender();
            while (ShouldRun)
            {
                int index = Renderer.GetInputChoice();

                var actionToDo = (from item in Elements
                                  where
                                     item is Button b
                                     && b.Entry == index
                                  select
                                     item as Button).FirstOrDefault();

                if (actionToDo == null)
                {
                    DoRender();
                    Renderer.DisplayError($"\rUnrecognised item: {index}\r\n");

                }
                else
                {
                    actionToDo.Action();
                    Renderer.PressKeyContinue();
                    DoRender();
                }
            }
        }
    }
}
