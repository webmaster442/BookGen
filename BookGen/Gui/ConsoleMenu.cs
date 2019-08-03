//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Gui.Elements;
using System;

namespace BookGen.Gui
{
    internal class ConsoleMenu: ConsoleMenuBase
    {
        public ConsoleMenu(GeneratorRunner runner) : base(runner)
        {
            Renderer.SetWindowTitle("BookGen");
        }

        public override ConsoleUiElement[] CreateElements()
        {
            return new ConsoleUiElement[]
            {
                new TextBlock
                {
                    Text = ResourceLocator.GetResourceFile<ConsoleMenu>("Resources/Splash.txt")
                },
                new TextBlock
                {
                    Text = "Config Actions:\r\n\r\n"
                },
                new Button
                {
                    Action = () => Runner.DoCreateConfig(),
                    Content = "Create config"
                },
                new Button
                {
                    Action = () => Runner.Initialize(),
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
                        if (Runner.Initialize()) Runner.DoClean();
                    },
                    Content = "Clean output directory"
                },
                new Button
                {
                    Action = () =>
                    {
                        if (Runner.Initialize()) Runner.DoTest();
                    },
                    Content = "Build test website"
                },
                new Button
                {
                    Action = () =>
                    {
                        if (Runner.Initialize()) Runner.DoBuild();
                    },
                    Content = "Build release website"
                },
                new Button
                {
                    Action = () =>
                    {
                        if (Runner.Initialize()) Runner.DoPrint();
                    },
                    Content = "Build print html"
                },
                new Button
                {
                    Action = () =>
                    {
                        if (Runner.Initialize()) Runner.DoEpub();
                    },
                    Content = "Build E-pub"
                },
                new TextBlock
                {
                    Text = "\r\nGeneral\r\n"
                },
                new Button
                {
                    Action = () => UsageInfo(),
                    Content = "Display usage info"
                }
            };
        }

        private void UsageInfo()
        {
            Console.WriteLine(ResourceLocator.GetResourceFile<ConsoleMenu>("Resources/Help.txt"));
            Renderer.PressKeyContinue();
        }

    }
}
