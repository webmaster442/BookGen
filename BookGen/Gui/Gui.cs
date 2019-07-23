//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace BookGen.Gui
{
    internal class ConsoleGui
    {
        public List<Button> _buttons;

        public ConsoleGui(GeneratorRunner runner)
        {
            ShouldRun = true;
            _buttons = new List<Button>
            {
                new Button
                {
                    Action = () => runner.DoCreateConfig(),
                    ActivatorKey = ConsoleKey.F1,
                    Content = "Create config"
                },
                new Button
                {
                    Action = () => runner.Initialize(),
                    ActivatorKey = ConsoleKey.F2,
                    Content = "Validate config"
                },
                new Button
                {
                    Action = () => Environment.Exit(0),
                    ActivatorKey = ConsoleKey.Escape,
                    Content = "Exit program"
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
                }
            };
        }

        private void Render()
        {
            Console.WriteLine("Actions: ");
            foreach (var button in _buttons)
            {
                Console.WriteLine("   {0}: {1}", button.ActivatorKey, button.Content);
            }
        }

        public static bool ShouldRun { get; set; }

        public void Run()
        {
            while (ShouldRun)
            {
                Render();
                var keyInfo = Console.ReadKey();
                var action = _buttons.Find(b => b.ActivatorKey == keyInfo.Key);

                if (action == null)
                {
                    Console.Write("\rUnrecognised Option");
                }
                else
                {
                    action.Action();
                }
            }
        }
    }
}
