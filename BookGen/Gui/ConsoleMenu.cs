//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Gui.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Gui
{
    internal class ConsoleMenu : ConsoleMenuBase
    {
        private readonly GeneratorRunner _runner;

        public ConsoleMenu(GeneratorRunner runner)
        {
            Renderer.SetWindowTitle("BookGen");
            _runner = runner;
        }

        public override IEnumerable<ConsoleUiElement> CreateElements()
        {
            yield return new TextBlock
            {
                Content = ResourceLocator.GetResourceFile<ConsoleMenu>("Resources/Splash.txt")
            };
            yield return new TextBlock
            {
                Content = $"Work dir: {_runner.WorkDirectory}\r\n\r\n"
            };
            yield return new TextBlock
            {
                Content = "Config Actions:\r\n\r\n"
            };
            yield return new Button
            {
                Action = () => _runner.DoInteractiveInitialize(),
                Content = "Interactive Initialize"
            };
            yield return new Button
            {
                Action = () => _runner.Initialize(),
                Content = "Validate config"
            };
            yield return new TextBlock
            {
                Content = "\r\nBuild:\r\n\r\n"
            };
            yield return new Button
            {
                Action = () =>
                {
                    _runner.InitializeAndExecute(x => x.DoClean());
                },
                Content = "Clean output directory"
            };
            yield return new Button
            {
                Action = () =>
                {
                    _runner.InitializeAndExecute(x => x.DoTest());
                },
                Content = "Build test website"
            };
            yield return new Button
            {
                Action = () =>
                {
                    _runner.InitializeAndExecute(x => x.DoBuild());
                },
                Content = "Build release website"
            };
            yield return new Button
            {
                Action = () =>
                {
                    _runner.InitializeAndExecute(x => x.DoPrint());
                },
                Content = "Build print html"
            };
            yield return new Button
            {
                Action = () =>
                {
                    _runner.InitializeAndExecute(x => x.DoEpub());
                },
                Content = "Build E-pub"
            };
            yield return new Button
            {
                Action = () =>
                {
                    _runner.InitializeAndExecute(x => x.DoWordpress());
                },
                Content = "Build Wordpress export file"
            };
            yield return new TextBlock
            {
                Content = "\r\nGeneral\r\n"
            };
            yield return new Button
            {
                Action = () => _runner.RunHelp(),
                Content = "Display usage info"
            };
            yield return new Button
            {
                Action = () => Environment.Exit(0),
                Content = "Exit program"
            };
        }

        protected override void ProcessInputs()
        {
            int? index = Renderer.GetInputChoice();

            Button? actionToDo = null;

            if (index.HasValue)
            {
                actionToDo = (from item in Elements
                              where
                                 item is Button b
                                 && b.Entry == index.Value
                              select
                                 item as Button).FirstOrDefault();
            }

            if (actionToDo == null)
            {
                DoRender();
                Renderer.DisplayError($"\rUnrecognised item: {index}\r\n");

            }
            else
            {
                actionToDo.Action?.Invoke();
                Renderer.PressKeyContinue();
                DoRender();
            }
        }
    }
}
