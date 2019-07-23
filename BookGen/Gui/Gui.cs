//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Elements;
using BookGen.Gui.Renderering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Gui
{
    internal class ConsoleGui
    {
        public List<ConsoleUiElement> _uiElements;
        private readonly Renderer _renderer;

        public ConsoleGui(GeneratorRunner runner)
        {
            ShouldRun = true;
            _renderer = new Renderer();
            _renderer.SetWindowTitle("BookGen");
            _uiElements = new List<ConsoleUiElement>
            {
                new TextBlock
                {
                    Text = Properties.Resources.Splash
                },
                new TextBlock
                {
                    Text = "Config Actions:\r\n\r\n"
                },
                new Button
                {
                    Action = () => runner.DoCreateConfig(),
                    ActivatorKey = ConsoleKey.F2,
                    Content = "Create config"
                },
                new Button
                {
                    Action = () => runner.Initialize(),
                    ActivatorKey = ConsoleKey.F3,
                    Content = "Validate config"
                },
                new TextBlock
                {
                    Text = "\r\nBuild:\r\n\r\n"
                },
                new Button
                {
                    Action = () =>
                    {
                        if (runner.Initialize()) runner.DoClean();
                    },
                    ActivatorKey = ConsoleKey.F4,
                    Content = "Clean output directory"
                },
                new Button
                {
                    Action = () =>
                    {
                        if (runner.Initialize()) runner.DoTest();
                    },
                    ActivatorKey = ConsoleKey.F5,
                    Content = "Build test website"
                },
                new Button
                {
                    Action = () =>
                    {
                        if (runner.Initialize()) runner.DoBuild();
                    },
                    ActivatorKey = ConsoleKey.F6,
                    Content = "Build release website"
                },
                new Button
                {
                    Action = () =>
                    {
                        if (runner.Initialize()) runner.DoPrint();
                    },
                    ActivatorKey = ConsoleKey.F7,
                    Content = "Build print html"
                },
                new Button
                {
                    Action = () =>
                    {
                        if (runner.Initialize()) runner.DoEpub();
                    },
                    ActivatorKey = ConsoleKey.F8,
                    Content = "Build E-pub"
                },
                new TextBlock
                {
                    Text = "\r\nGeneral\r\n"
                },
                new Button
                {
                    Action = () => UsageInfo(),
                    ActivatorKey = ConsoleKey.F1,
                    Content = "Display usage info"
                },
                new Button
                {
                    Action = () => Environment.Exit(0),
                    ActivatorKey = ConsoleKey.Escape,
                    Content = "Exit program"
                },
            };
        }

        private void UsageInfo()
        {
            Console.WriteLine(Properties.Resources.Help);
            _renderer.PressKeyContinue();
        }

        private void Render()
        {
            _renderer.Clear();
            foreach (var uiElement in _uiElements)
            {
                uiElement.Render(_renderer);
            }
        }

        public static bool ShouldRun { get; set; }

        public void Run()
        {
            Render();
            while (ShouldRun)
            {
                var keyInfo = Console.ReadKey();
                var actionToDo = (from item in _uiElements
                                  where
                                     item is Button b
                                     && b.ActivatorKey == keyInfo.Key
                                  select
                                     item as Button).FirstOrDefault();

                if (actionToDo == null)
                {
                    Render();
                    _renderer.DisplayError($"\rUnrecognised key: {keyInfo.Key}\r\n");
                }
                else
                {
                    actionToDo.Action();
                    _renderer.PressKeyContinue();
                    Render();
                }
            }
        }
    }
}
